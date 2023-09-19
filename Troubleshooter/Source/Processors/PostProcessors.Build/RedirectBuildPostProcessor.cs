using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Handles "<see cref="Constants.RedirectSuffix"/>" files that contain one link to a page:<br/>
/// [Page](page.md)<br/>
/// and optional links to folders:<br/>
/// [Folder](folder)<br/>
/// These resources will be moved relative to the file, and their originating locations will serve a link to the new location.
/// </summary>
[UsedImplicitly]
public sealed class RedirectBuildPostProcessor : IBuildPostProcessor
{
	public void Process(Arguments arguments, PageResourcesLookup resources, Site site)
	{
		KeyValuePair<string, PageResource>[] redirects = resources.Where(r => r.Value.Type is ResourceType.Redirection).ToArray();
		foreach ((string context, PageResource resource) in redirects)
		{
			string siteDirectory = site.Directory;
			string relativePath = GetRelativePathSimplified(siteDirectory, Path.GetDirectoryName(resource.FullPath)!);
			string outputDirectory = Path.Combine(arguments.HtmlOutputDirectory!, relativePath.ToFinalisedWorkingPath());

			Console.WriteLine(outputDirectory);

			resource.MarkdownText ??= File.ReadAllText(resource.FullPath);
			bool hasMovedPage = false;
			foreach (Match match in CommonRegex.InternalLinks.Matches(resource.MarkdownText))
			{
				// string key = match.Groups[1].Value;
				string localTokenizedLink = PathUtility.ToFinalisedWorkingPath(match.Groups[2].Value, 0);

				if (Path.HasExtension(localTokenizedLink))
				{
					if (hasMovedPage)
						throw new BuildException($"\"{context}\" contains multiple pages. Only one page should be redirected at a time.");

					// File
					RedirectFileFromTokenizedLink(localTokenizedLink, outputDirectory, site, context);
					hasMovedPage = true;
				}
				else
				{
					RedirectFolderContentsFromTokenizedLink(localTokenizedLink, outputDirectory, site, context);
				}
			}
		}

		return;

		static string GetRelativePathSimplified(string from, string to) => from == to ? "" : Path.GetRelativePath(from, to);

		static string RemapExtensionsFromSourceToDestination(string path)
		{
			if (path.EndsWith(".md"))
				path = Path.ChangeExtension(path, ".html");
			return path;
		}

		static void RedirectFileFromTokenizedLink(string localTokenizedLink, string destinationDirectory, Site site, string context)
		{
			string fullPath = RemapExtensionsFromSourceToDestination(
				PageUtility.GetFullPathFromTokenizedLink(destinationDirectory, localTokenizedLink, site.ContentDirectory)
			);
			string fileName = Path.GetFileName(fullPath);
			RedirectFileFromRelativePaths(fullPath, destinationDirectory, fileName, context);
		}

		static void RedirectFileFromRelativePaths(string originPath, string destinationDirectory, string destinationRelativePath, string context)
		{
			if (!File.Exists(originPath))
				throw new BuildException($"\"{originPath}\" was not found when processing $\"{context}\" redirect.");

			// Files are moved by assuming the relativePath passed in was the intended to be relative to the destination
			string destinationPath = Path.Combine(destinationDirectory, destinationRelativePath);
			RedirectFile(originPath, destinationPath);
			
			// Redirect sidebar.
			// This logic is only necessary for single files, as directories detect sidebars and move them correctly.
			if (!originPath.EndsWith(".html"))
				return;

			string sidebarOriginPath = PathUtility.ConvertMarkdownPathToSidebarPath(originPath);
			if (!File.Exists(sidebarOriginPath))
				return;

			string sidebarDestinationPath = PathUtility.ConvertMarkdownPathToSidebarPath(destinationPath);
			RedirectFile(sidebarOriginPath, sidebarDestinationPath);
		}

		static void RedirectFile(string from, string to)
		{
			Console.WriteLine($"""
			                   "{from}" -> "{to}"
			                   """);

			// TODO redirect original urls. Currently just copying files.

			// Make a copy of the file at the destination directory.
			RedirectBuildUtilities.CopyFileIfDifferentAndRedirectInternalLinks(new FileInfo(from), to);
		}

		static void RedirectFolderContentsFromTokenizedLink(string localTokenizedLink, string destinationDirectory, Site site, string context)
		{
			string fullPath = PageUtility.GetFullPathFromTokenizedLink(destinationDirectory, localTokenizedLink, site.ContentDirectory);
			fullPath = RemapExtensionsFromSourceToDestination(fullPath);
			string directoryName = Path.GetFileName(fullPath);
			RedirectFolderContents(fullPath, destinationDirectory, directoryName, context);
		}

		static void RedirectFolderContents(string originPath, string destinationDirectory, string destinationRelativePath, string context)
		{
			if (!Directory.Exists(originPath))
				throw new BuildException($"\"{originPath}\" was not found when processing $\"{context}\" redirect.");

			string destinationPath = Path.Combine(destinationDirectory, destinationRelativePath);

			foreach (string originFilePath in Directory.EnumerateFiles(originPath, "*", SearchOption.AllDirectories))
			{
				string destinationFilePath = Path.Combine(destinationPath, Path.GetRelativePath(originPath, originFilePath));
				RedirectFile(originFilePath, destinationFilePath);
			}
		}
	}
}