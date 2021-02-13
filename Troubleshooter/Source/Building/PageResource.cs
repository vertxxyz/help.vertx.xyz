using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
		/// GUID used in the final output to remove paths from the html files
		/// </summary>
		public readonly string Guid;
		
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
			Guid = System.Guid.NewGuid().ToString();
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

		public void BuildText(Site site, Dictionary<string, PageResource> allResources, MarkdownPipeline markdownPipeline)
		{
			switch (Type)
			{
				case ResourceType.None:
					SetHtmlTextAsEmpty();
					return;
				case ResourceType.Markdown:
					BuildMarkdown(site, allResources, markdownPipeline);
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
		private void BuildMarkdown(Site site, Dictionary<string, PageResource> allResources, MarkdownPipeline markdownPipeline)
		{
			string allText = File.ReadAllText(FullPath);
			StringBuilder stringBuilder = new StringBuilder();

			// Find and replace embed anchors with their content
			{
				var embeds = PageUtility.EmbedsAsLocalEmbedPaths(allText);

				int last = 0;
				foreach (var embed in embeds)
				{
					string fullPath = PageUtility.LocalEmbedToFullPath(embed.localPath, site);
					if (!allResources.TryGetValue(fullPath, out var embeddedPage))
						throw new LogicException($"\"{fullPath}\" is missing from {nameof(allResources)} while processing \"{FullPath}\".{nameof(embeds)}");

					stringBuilder.Append(allText[last..(embed.group.Index - 2)]);
					stringBuilder.Append(embeddedPage.HtmlText);
					last = embed.group.Index + embed.group.Length + 2;
				}

				stringBuilder.Append(allText[last..]);
			}

			//
			var markdownWithEmbeds = stringBuilder.ToString();
			stringBuilder.Clear();
			//
			
			// Find and replace local links with jquery guid loading

			{
				var links = PageUtility.LinksAsFullPaths(markdownWithEmbeds, FullPath);

				int last = 0;
				foreach (var link in links)
				{
					if (!allResources.TryGetValue(link.fullPath, out var linkedPage))
						throw new LogicException($"\"{link.fullPath}\" is missing from {nameof(allResources)} while processing \"{FullPath}\".{nameof(links)}");
					
					stringBuilder.Append(markdownWithEmbeds[last..link.group.Index]);
					stringBuilder.Append(linkedPage.Guid);
					last = link.group.Index + link.group.Length;
				}

				stringBuilder.Append(markdownWithEmbeds[last..]);
			}



			HtmlText =
				HtmlPostProcessors.Process(
					Markdown.ToHtml(
						stringBuilder.ToString(), // markdown
						markdownPipeline
					)
				);
		}

		public void WriteToDisk(Arguments arguments)
		{
			switch (Location)
			{
				case ResourceLocation.Site:
					break;
				default:
					//Do not write to disk if this is not a part of the site.
					return;
			}

			string path = Path.Combine(arguments.HtmlOutputDirectory, $"{Guid}.html");
			File.WriteAllText(path, HtmlText);
		}
	}
}