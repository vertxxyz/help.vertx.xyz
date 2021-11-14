using System;
using System.IO;
using System.Linq;
using DartSassHost;
using JavaScriptEngineSwitcher.V8;
using Troubleshooter.Constants;
using static Troubleshooter.IOUtility;

namespace Troubleshooter;

public static partial class SiteBuilder
{
	private static void BuildContent(Arguments arguments, Site site)
	{
		// Copy content to destination
		CopyAll(new DirectoryInfo(site.ContentDirectory), new DirectoryInfo(arguments.Path), fileProcessor: FileProcessor);
			
		int siteContent = 0;
		int totalContent = 0;
			
		// Copy all files that are not pages to the destination
		foreach (var path in Directory.EnumerateFiles(site.Directory, "*", SearchOption.AllDirectories))
		{
			string extension = Path.GetExtension(path);
			if(extension.Equals(".md")) continue; // Ignore pages
				
			string fullPath = Path.GetFullPath(path);
			string outputPath = ConvertRootFullSitePathToLinkPath(fullPath, extension, site, arguments);

			totalContent++;
			if (CopyFileIfDifferent(outputPath, new FileInfo(fullPath)))
				siteContent++;
		}


		int embedContent = 0;
		// Copy all embed files that are not pages to the destination/Embeds
		foreach (var path in Directory.EnumerateFiles(site.EmbedsDirectory, "*", SearchOption.AllDirectories))
		{
			string extension = Path.GetExtension(path);
			if(extension.Equals(".md") || extension.Equals(".rtf") || extension.Equals(".html")) continue; // Ignore pages
				
			string fullPath = Path.GetFullPath(path);
			string outputPath = ConvertFullEmbedPathToLinkPath(fullPath, extension, site, arguments);
				
			totalContent++;
			if(CopyFileIfDifferent(outputPath, new FileInfo(fullPath)))
				embedContent++;
		}
			
		arguments.VerboseLog($"{siteContent + embedContent} content files were written to disk. ({totalContent} total)");
	}

	private static FileResult.Validity FileProcessor(FileInfo file, out FileResult result)
	{
		switch (file.Extension)
		{
			case ".scss" when file.Name[0] == '_':
				result = null;
				return FileResult.Validity.Skipped;
			case ".scss":
			{
				try
				{
					CompilationOptions options = new CompilationOptions
					{
						IncludePaths = file.Directory!.GetFiles("_*.scss").Select(f => f.FullName).ToList(),
						IndentType = IndentType.Tab, IndentWidth = 1
					};

					using SassCompiler sassCompiler = new SassCompiler(new V8JsEngineFactory(), options);
					CompilationResult compilationResult = sassCompiler.CompileFile(file.FullName);
					result = new FileResult(compilationResult.CompiledContent, Path.ChangeExtension(file.Name, "css"));
				}
				catch (Exception e)
				{
					throw new BuildException(e, "SCSS failure");
				}

				return FileResult.Validity.Processed;
			}
			default:
				result = null;
				return FileResult.Validity.NotProcessed;
		}
	}
}