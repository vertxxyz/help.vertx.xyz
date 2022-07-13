using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DartSassHost;
using JavaScriptEngineSwitcher.V8;
using Troubleshooter.Constants;
using static Troubleshooter.IOUtility;

namespace Troubleshooter;

public static partial class SiteBuilder
{
	private static async Task BuildContent(Arguments arguments, Site site)
	{
		// Copy content to destination
		CopyAll(new DirectoryInfo(site.ContentDirectory), new DirectoryInfo(arguments.Path!), fileProcessor: FileProcessor);

		await DownloadAndReplaceSourceFiles();

		async Task DownloadAndReplaceSourceFiles()
		{
			// Source files
			Regex r = new Regex(@"<script src=""(\/\/unpkg\.com\/.+?.js)""><\/script>");
			Regex rVersion = new Regex(@"@([\d.]+?)\/");

			string indexOutputPath = Path.Combine(arguments.Path!, "index.html");
			if (!File.Exists(indexOutputPath))
				throw new BuildException($"\"{indexOutputPath}\" was not found when replacing source files.");

			HttpClient? client = null;
			int matchIndex = 0;
			List<(Task<string> versionTask, string fileName, int start, int end)> fileRequests = new();

			string indexText = await File.ReadAllTextAsync(indexOutputPath);
			bool changed = false;
			do
			{
				Match match = r.Match(indexText, matchIndex);
				if (!match.Success)
					break;
				matchIndex = match.Index + match.Length;
				changed = true;
				Group group = match.Groups[1];
				string value = group.Value;

				client ??= new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
				var requestUri = $"https:{value}";
				string fileName = value[(value.LastIndexOf('/') + 1)..];
				Console.WriteLine($"Requesting \"{requestUri}\"...");

				fileRequests.Add((RequestFile(requestUri, fileName, client), fileName, group.Index, group.Index + group.Length));
			} while (true);

			async Task<string> RequestFile(string uri, string fileName, HttpClient c)
			{
				string output = Path.Combine(arguments.Path!, "Scripts", fileName);
				HttpResponseMessage message = await c.GetAsync(uri, HttpCompletionOption.ResponseContentRead);
				if (!message.IsSuccessStatusCode)
					throw new BuildException($"\"{uri}\" was not found when replacing source files. {message.StatusCode}: {message.RequestMessage}");
				Match matchVersion = rVersion.Match(message.RequestMessage!.RequestUri!.OriginalString);
				if (!matchVersion.Success)
					throw new BuildException($"No version was found was not found after requesting \"{uri}\". {message.RequestMessage.RequestUri.OriginalString}");
				await CreateFileIfDifferentAsync(output, await message.Content.ReadAsStringAsync());
				return matchVersion.Groups[1].Value;
			}

			if (changed)
			{
				await Task.WhenAll(fileRequests.Select(request => request.versionTask));

				int offset = 0;
				foreach ((Task<string> versionTask, string fileName, int start, int end) in fileRequests)
				{
					string version = versionTask.Result;
					string newValue = $"/Scripts/{fileName}?={version}";
					indexText = $"{indexText[..(start + offset)]}{newValue}{indexText[(end + offset)..]}";
					offset += newValue.Length - (end - start);
				}
				
				await CreateFileIfDifferentAsync(indexOutputPath, indexText);
			}
		}

		int siteContent = 0;
		int totalContent = 0;

		// Copy all files that are not pages to the destination
		foreach (string path in Directory.EnumerateFiles(site.Directory, "*", SearchOption.AllDirectories))
		{
			string extension = Path.GetExtension(path);
			if (extension.Equals(".md")) continue; // Ignore pages
			if (extension.Equals(".gen")) continue; // Ignore generated content

			string fullPath = Path.GetFullPath(path);
			string outputPath = ConvertRootFullSitePathToLinkPath(fullPath, extension, site, arguments);

			totalContent++;
			if (CopyFileIfDifferent(outputPath, new FileInfo(fullPath)))
				siteContent++;
		}

		Generate404();

		void Generate404()
		{
			string indexHtml = Path.Combine(site.ContentDirectory, "index.html");
			if (!File.Exists(indexHtml))
				throw new BuildException($"\"{indexHtml}\" was not found when generating 404 page.");
			string indexText = File.ReadAllText(indexHtml);
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
			if (extension.Equals(".md") || extension.Equals(".rtf") || extension.Equals(".html") ||
			    extension.Equals(".nomnoml")) continue; // Ignore pages & nomnoml

			string fullPath = Path.GetFullPath(path);
			string outputPath = ConvertFullEmbedPathToLinkPath(fullPath, extension, site, arguments);

			totalContent++;
			if (CopyFileIfDifferent(outputPath, new FileInfo(fullPath)))
				embedContent++;
		}

		arguments.VerboseLog(
			$"{siteContent + embedContent} content files were written to disk. ({totalContent} total)");
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