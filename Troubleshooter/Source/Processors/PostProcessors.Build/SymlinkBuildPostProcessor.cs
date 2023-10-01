using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Troubleshooter;

/// <summary>
/// Copies any linked pages that are associated with pages marked as symlinks.<br/>
/// Also modifies their links so they are root-level.
/// </summary>
[UsedImplicitly]
public sealed class SymlinkBuildPostProcessor : IBuildPostProcessor
{
	public void Process(Arguments arguments, PageResourcesLookup resources, Site site, ILogger logger)
	{
		foreach ((string context, PageResource resource) in resources.Where(r => (r.Value.Flags & ResourceFlags.Symlink) != 0))
		{
			string targetLink = resource.SymlinkFullPath!;

			if (!resources.TryGetValue(targetLink, out PageResource? source))
				throw new BuildException($"Symlinked resource \"{targetLink}\" was not found when processing \"{context}\".");

			string sourceOutput = source.OutputFilePath; // The path of the source file we're copying.
			string resourceOutput = resource.OutputFilePath; // The destination path.
			if ((source.Flags & ResourceFlags.ExistsInOutput) != 0) // Check that the source was actually written.
				RedirectFile(sourceOutput, resourceOutput, logger);

			// Redirect resources generated from the base resource if they exist...
			if (!source.HasGeneratedChildren)
				continue;

			string sourceDirectory = Path.GetDirectoryName(sourceOutput)!; // The directory of the source file we're copying.
			string resourceDirectory = Path.GetDirectoryName(resourceOutput)!; // The destination directory.

			foreach (PageResource child in source.GeneratedChildren)
			{
				string childSourceOutput = child.OutputFilePath; // The path of the source file we're copying.
				string childResourceOutput = Path.GetFullPath(Path.GetRelativePath(sourceDirectory, childSourceOutput), resourceDirectory); // The destination path.
				// string childSourceDirectory = Path.GetDirectoryName(childSourceOutput)!; // The directory of the source file we're copying.
				if ((child.Flags & ResourceFlags.ExistsInOutput) != 0)
					RedirectFile(childSourceOutput, childResourceOutput, logger);
			}
		}

		return;

		static void RedirectFile(string from, string to, ILogger logger)
		{
			logger.LogDebug(
				"""
				"{from}" -> "{to}"
				""",
				from,
				to
			);
			// Make a copy of the file at the destination directory.
			CopyFileIfDifferentAndRedirectInternalLinks(new FileInfo(from), to);
		}
	}

	// ReSharper disable once UnusedMethodReturnValue.Local
	private static bool CopyFileIfDifferentAndRedirectInternalLinks(FileInfo from, string to)
	{
		string text = File.ReadAllText(from.FullName);

		MatchCollection matches = CommonRegex.LoadPage.Matches(text);

		if (matches.Count > 0)
		{
			string toDirectory = Path.GetDirectoryName(to)!;
			string fromDirectory = Path.GetDirectoryName(from.FullName)!;

			// Redirect local site links so they redirect to the base resource.
			text = StringUtility.ReplaceMatch(text, matches, (match, builder) =>
			{
				if (match.Groups[1].Value.StartsWith('/'))
				{
					builder.Append(match.Groups[0].ValueSpan);
					return;
				}

				// Redirect local site links
				string relativePath = Path.GetRelativePath(toDirectory, Path.GetFullPath(match.Groups[1].Value, fromDirectory)).ToOutputPath();
				builder.Append("loadPage('");
				builder.Append(relativePath);
				builder.Append("')");
			});
		}


		// TODO check if different via text. WriteFileTextIfDifferent
		return IOUtility.WriteFileTextIfDifferent(text, to, IOUtility.RecordType.Duplicate);
	}
}
