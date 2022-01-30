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
		CopyAll(new(site.ContentDirectory), new(arguments.Path!), fileProcessor: FileProcessor);

		int siteContent = 0;
		int totalContent = 0;

		// Copy all files that are not pages to the destination
		foreach (string path in Directory.EnumerateFiles(site.Directory, "*", SearchOption.AllDirectories))
		{
			string extension = Path.GetExtension(path);
			if (extension.Equals(".md")) continue; // Ignore pages

			string fullPath = Path.GetFullPath(path);
			string outputPath = ConvertRootFullSitePathToLinkPath(fullPath, extension, site, arguments);

			totalContent++;
			if (CopyFileIfDifferent(outputPath, new(fullPath)))
				siteContent++;
		}

		Generate404();

		void Generate404()
		{
			string indexhtml = Path.Combine(site.ContentDirectory, "index.html");
			if (!File.Exists(indexhtml))
				throw new BuildException($"\"{indexhtml}\" was not found when generating 404 page.");
			string indexText = File.ReadAllText(indexhtml);
			//int indexOfContent = indexText.IndexOf("</head>", StringComparison.Ordinal);
			//if (indexOfContent < 0)
			//	throw new BuildException("\"</head>\" not found when generating 404 page from index.html.");
			//string text404 = indexText.Insert(indexOfContent, "    <script src=\"/Scripts/404.js\"></script>\n");
			CreateFileIfDifferent(Path.Combine(arguments.Path!, "404.html"), indexText);
		}

		int embedContent = 0;
		// Copy all embed files that are not pages to the destination/Embeds
		foreach (string path in Directory.EnumerateFiles(site.EmbedsDirectory, "*", SearchOption.AllDirectories))
		{
			string extension = Path.GetExtension(path);
			if (extension.Equals(".md") || extension.Equals(".rtf") || extension.Equals(".html") || extension.Equals(".nomnoml")) continue; // Ignore pages & nomnoml

			string fullPath = Path.GetFullPath(path);
			string outputPath = ConvertFullEmbedPathToLinkPath(fullPath, extension, site, arguments);

			totalContent++;
			if (CopyFileIfDifferent(outputPath, new(fullPath)))
				embedContent++;
		}

		arguments.VerboseLog($"{siteContent + embedContent} content files were written to disk. ({totalContent} total)");
	}

	private static FileResult.Validity FileProcessor(FileInfo file, out FileResult? result)
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
					CompilationOptions options = new()
					{
						IncludePaths = file.Directory!.GetFiles("_*.scss").Select(f => f.FullName).ToList(),
						IndentType = IndentType.Tab, IndentWidth = 1
					};

					using SassCompiler sassCompiler = new(new V8JsEngineFactory(), options);
					CompilationResult compilationResult = sassCompiler.CompileFile(file.FullName);
					result = new(compilationResult.CompiledContent, Path.ChangeExtension(file.Name, "css"));
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