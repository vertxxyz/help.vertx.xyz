using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DartSassHost;
using JavaScriptEngineSwitcher.V8;
using static Troubleshooter.IOUtility;

namespace Troubleshooter;

public static partial class SiteBuilder
{
	private static Task BuildContent(Arguments arguments, Site site)
	{
		// Copy content to destination
		CopyAll(new DirectoryInfo(site.ContentDirectory), new DirectoryInfo(arguments.Path), fileProcessor: FileProcessor);

		string indexOutputPath = Path.Combine(arguments.Path, "index.html");
		int siteContent = 0;
		int totalContent = 0;

		// Copy all files that are not pages to the destination
		foreach (string path in Directory.EnumerateFiles(site.Directory, "*", SearchOption.AllDirectories))
		{
			string extension = Path.GetExtension(path);
			switch (extension)
			{
				case ".md": // Ignore pages
				case ".gen": // Ignore generated content
				case "": // Ignore files that lack an extension.
					continue;
			}


			string fullPath = Path.GetFullPath(path);

			FileInfo originFile = new(fullPath);

			// Don't copy symlinks.
			if (originFile.LinkTarget != null)
				continue;

			string outputPath = ConvertRootFullSitePathToLinkPath(fullPath, extension, site, arguments);

			totalContent++;
			if (CopyFileIfDifferent(outputPath, originFile, RecordType.Normal))
				siteContent++;
		}

		Generate404();

		int embedContent = 0;
		// Copy all embed files that are not pages to the destination/Embeds
		foreach (string path in Directory.EnumerateFiles(site.EmbedsDirectory, "*", SearchOption.AllDirectories))
		{
			string extension = Path.GetExtension(path);
			switch (extension)
			{
				// Ignore pages & nomnoml
				case ".md":
				case ".rtf":
				case ".html":
				case ".nomnoml":
					continue;
			}

			string fullPath = Path.GetFullPath(path);
			string outputPath = ConvertFullEmbedPathToLinkPath(fullPath, extension, site, arguments);

			totalContent++;
			if (CopyFileIfDifferent(outputPath, new FileInfo(fullPath), RecordType.Normal))
				embedContent++;
		}

		arguments.VerboseLog($"{siteContent + embedContent} content files were written to disk. ({totalContent} total)");

		return Task.CompletedTask;

		void Generate404()
		{
			if (!File.Exists(indexOutputPath))
				throw new BuildException($"\"{indexOutputPath}\" was not found when generating 404 page.");
			string indexText = File.ReadAllText(indexOutputPath);
			CreateFileIfDifferent(Path.Combine(arguments.Path, "404.html"), indexText, RecordType.Index);
		}
	}

	private static FileResult.Validity FileProcessor(FileInfo file, out FileResult? result)
	{
		switch (file.Extension)
		{
			case ".gen":
			case ".scss" when file.Name[0] == '_':
				result = null;
				return FileResult.Validity.Skipped;
			case ".scss":
				{
					try
					{
						CompilationOptions options = new() { IncludePaths = file.Directory!.GetFiles("_*.scss").Select(f => f.FullName).ToList(), IndentType = IndentType.Tab, IndentWidth = 1 };

						using SassCompiler sassCompiler = new(new V8JsEngineFactory(), options);
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
