using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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
		public string Guid { get; private set; }
		
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

			//
			var markdownWithEmbeds = stringBuilder.ToString();
			stringBuilder.Clear();
			//
			
			// Find and replace local links with jquery guid loading
			{
				var links = PageUtility.LinksAsFullPaths(markdownWithEmbeds, FullPath);

				int last = 0;
				foreach ((string fullPath, Group group) in links)
				{
					if (!allResources.TryGetValue(fullPath, out var linkedPage))
						throw new LogicException($"\"{fullPath}\" is missing from {nameof(allResources)} while processing \"{FullPath}\".{nameof(links)}");
					
					stringBuilder.Append(markdownWithEmbeds[last..group.Index]);
					stringBuilder.Append(linkedPage.Guid);
					last = group.Index + group.Length;
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

		public enum WriteStatus
		{
			Ignored,
			Skipped,
			Written
		}

		public WriteStatus WriteToDisk(Arguments arguments, Links builtLinks, int siteRootIndex)
		{
			switch (Location)
			{
				case ResourceLocation.Site:
					break;
				default:
					// Do not write to disk if this is not a part of the site.
					return WriteStatus.Ignored;
			}

			if (builtLinks != null)
			{
				// Check the previously built file to see whether it ought to be re-written.
				if (builtLinks.LinksToGuids.TryGetValue(SiteBuilder.ConvertFullPathToLinkPath(FullPath, siteRootIndex), out var oldGuid))
				{
					var oldPath = Path.Combine(arguments.HtmlOutputDirectory, $"{oldGuid}.html");
					if (File.Exists(oldPath))
					{
						if (File.ReadAllText(oldPath).Equals(HtmlText, StringComparison.Ordinal))
						{
							// Reset the GUID to maintain the link to the previously built file.
							Guid = oldGuid;
							return WriteStatus.Skipped;
						}
					}
				}
			}
			
			string path = Path.Combine(arguments.HtmlOutputDirectory, $"{Guid}.html");
			File.WriteAllText(path, HtmlText);
			return WriteStatus.Written;
		}
	}
}