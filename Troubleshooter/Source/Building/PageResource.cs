using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Markdig;
using Troubleshooter.Constants;

namespace Troubleshooter
{
	public enum ResourceType
	{
		None,
		Markdown,
		RichText
	}

	public enum ResourceLocation
	{
		/// <summary>
		/// Content is embedded in other pages and is never root content in the output
		/// </summary>
		Embed,
		/// <summary>
		/// Content is root content in the html output
		/// </summary>
		Site
	}

	public class PageResource
	{
		/// <summary>
		/// Full path to the source file
		/// </summary>
		public readonly string FullPath;
		
		/// <summary>
		/// What type of data this resource contains, markdown, rich text, etc.
		/// </summary>
		public readonly ResourceType Type;
		
		/// <summary>
		/// Where the resource is - site content, embedded page content, etc.
		/// </summary>
		public readonly ResourceLocation Location;

		/// <summary>
		/// Processed output html
		/// </summary>
		public string HtmlText { get; private set; }

		// -------- Unbuilt resources --------
		/// <summary>
		/// Resources that are embedded into this page
		/// </summary>
		private HashSet<string> embedded;
		/// <summary>
		/// Resources that are embedded into other pages
		/// </summary>
		private HashSet<string> embeddedInto;
		// -----------------------------------

		/// <summary>
		/// Resources that are embedded into this page
		/// </summary>
		public HashSet<string> Embedded => embedded;
		/// <summary>
		/// Resources that are embedded into other pages
		/// </summary>
		public HashSet<string> EmbeddedInto => embeddedInto;

		public PageResource(string fullPath, ResourceType type, ResourceLocation location)
		{
			Type = type;
			Location = location;
			FullPath = fullPath;
		}

		public void AddEmbeddedInto(string page)
		{
			embeddedInto ??= new HashSet<string>();
			embeddedInto.Add(page);
		}

		public void AddEmbedded(string page)
		{
			embedded ??= new HashSet<string>();
			embedded.Add(page);
		}

		public void BuildText(Site site, Dictionary<string, PageResource> allResources, MarkdownPipeline markdownPipeline, int siteRootIndex, int embedRootIndex)
		{
			switch (Type)
			{
				case ResourceType.None:
					SetHtmlTextAsEmpty();
					return;
				case ResourceType.Markdown:
					BuildMarkdown(site, allResources, markdownPipeline, siteRootIndex, embedRootIndex);
					break;
				case ResourceType.RichText:
					try
					{
						HtmlText = RtfUtility.RtfToHtml(File.ReadAllText(FullPath));
					}
					catch
					{
						Console.WriteLine($"Error when parsing {FullPath} into RTF");
						throw;
					}

					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void SetHtmlTextAsEmpty() => HtmlText = string.Empty;

		/// <summary>
		/// Builds page to <see cref="HtmlText"/> to be embedded in other content or written to disk
		/// </summary>
		private void BuildMarkdown(Site site, Dictionary<string, PageResource> allResources, MarkdownPipeline markdownPipeline, int siteRootIndex, int embedRootIndex)
		{
			string allText = File.ReadAllText(FullPath);
			StringBuilder stringBuilder = new StringBuilder();

			// Find and replace embed anchors with their content
			{
				var embeds = PageUtility.EmbedsAsLocalEmbedPaths(allText);

				int last = 0;
				foreach ((string localPath, Group group) in embeds)
				{
					string fullPath = PageUtility.LocalEmbedToFullPath(localPath, site);
					if (!allResources.TryGetValue(fullPath, out var embeddedPage))
						throw new LogicException($"\"{fullPath}\" is missing from {nameof(allResources)} while processing \"{FullPath}\".{nameof(embeds)}");

					stringBuilder.Append(allText[last..(group.Index - 2)]);
					stringBuilder.Append(embeddedPage.HtmlText);
					last = group.Index + group.Length + 2;
				}

				stringBuilder.Append(allText[last..]);
			}
			
			// Find and replace image links with root-based links
			{
				allText = stringBuilder.ToString();
				stringBuilder.Clear();

				int rootIndex;
				string directoryRoot;
				string embedsDirectory = Path.Combine(Arguments.HtmlOutputDirectoryName, "Embeds");
				switch (Location)
				{
					case ResourceLocation.Embed:
						rootIndex = embedRootIndex;
						directoryRoot = embedsDirectory;
						break;
					case ResourceLocation.Site:
						rootIndex = siteRootIndex;
						directoryRoot = Arguments.HtmlOutputDirectoryName;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				
				string directory = Path.GetDirectoryName(SiteBuilder.ConvertFullPathToLocalPath(FullPath, rootIndex));

				int last = 0;
				foreach ((string image, Group group) in PageUtility.ImagesAsRootPaths(allText))
				{
					if(group.Value.StartsWith(embedsDirectory))
						continue;
					
					stringBuilder.Append(allText[last..group.Index]);
					stringBuilder.Append(HttpUtility.UrlPathEncode(Path.Combine(directoryRoot, directory, image).Replace('\\', '/')));
					last = group.Index + group.Length;
				}
				stringBuilder.Append(allText[last..]);
			}

			switch (Location)
			{
				case ResourceLocation.Embed:
					// Embeds are not fully processed into HTML until they are built when embedded into site content.
					HtmlText = stringBuilder.ToString(); // markdown
					break;
				case ResourceLocation.Site:
					HtmlText =
						HtmlPostProcessors.Process(
							Markdown.ToHtml(
								stringBuilder.ToString(), // markdown
								markdownPipeline
							)
						);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			
		}

		public enum WriteStatus
		{
			Ignored,
			Skipped,
			Written
		}

		public WriteStatus WriteToDisk(Arguments arguments, int siteRootIndex)
		{
			switch (Location)
			{
				case ResourceLocation.Site:
					break;
				default:
					// Do not write to disk if this is not a part of the site.
					return WriteStatus.Ignored;
			}

			// Check the previously built file to see whether it ought to be re-written.
			var path = Path.Combine(arguments.HtmlOutputDirectory, $"{SiteBuilder.ConvertFullPathToLinkPath(FullPath, siteRootIndex)}.html");
			return IOUtility.CreateFileIfDifferent(path, HtmlText) ? WriteStatus.Written : WriteStatus.Skipped;
		}
	}
}