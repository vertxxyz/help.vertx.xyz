using System;
using System.IO;
using Markdig.Extensions.Yaml;
using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Troubleshooter.Renderers;

public sealed class YamlFrontMatterHtmlOverrideRenderer : HtmlObjectRenderer<YamlFrontMatterBlock>
{
	public PageResource CurrentResource { get; set; } = null!;
	public Site Site { get; set; } = null!;

	protected override void Write(HtmlRenderer renderer, YamlFrontMatterBlock obj)
	{
		if (renderer is not IHeadRenderer headRenderer)
		{
			Console.WriteLine($"No IHeadWriter passed, {nameof(YamlFrontMatterHtmlOverrideRenderer)} was unused.");
			return;
		}

		foreach (StringLine line in obj.Lines.Lines)
		{
			var text = line.Slice.AsSpan();
			if (text.IsWhiteSpace()) continue;

			if (GetContentMatching(text, "title", out var title))
			{
				headRenderer.HeadWriter.SetTitle(title);
			}
			else if (GetContentMatching(text, "type", out var type))
			{
				headRenderer.HeadWriter.SetType(type);
			}
			else if (GetContentMatching(text, "url", out var url))
			{
				headRenderer.HeadWriter.SetUrl(url);
			}
			else if (GetContentMatching(text, "image", out var image))
			{
				headRenderer.HeadWriter.SetImage(ToAbsoluteUrl(image), ReadOnlySpan<char>.Empty);
			}
			else if (GetContentMatching(text, "image-large", out var largeImage))
			{
				headRenderer.HeadWriter.SetLargeImage(ToAbsoluteUrl(largeImage), ReadOnlySpan<char>.Empty);
			}
			else if (GetContentMatching(text, "image:alt", out var alt))
			{
				headRenderer.HeadWriter.SetImageAlt(alt);
			}
			else if (GetContentMatching(text, "description", out var description))
			{
				headRenderer.HeadWriter.SetDescription(description);
			}
			else if (GetContentMatching(text, "video", out var video))
			{
				headRenderer.HeadWriter.SetVideo(ToAbsoluteUrl(video));
			}
			if (GetContentMatching(text, "site-name", out var siteName))
			{
				headRenderer.HeadWriter.SetSiteName(siteName);
			}

			continue;

			string ToAbsoluteUrl(in ReadOnlySpan<char> url)
			{
				string directoryName = Path.GetDirectoryName(CurrentResource.FullPath) ?? "";
				string fullPathToUrl = Path.GetFullPath(Path.Combine(directoryName, url.ToString()));
				string localPath = Site.FinalisePathWithRootIndex(fullPathToUrl, Site.RootIndex).ToOutputPath();
				string path = $"https://unity.huh.how/{localPath}";
				return path;
			}

			bool GetContentMatching(in ReadOnlySpan<char> text, in ReadOnlySpan<char> tag, out ReadOnlySpan<char> content)
			{
				if (!text.StartsWith(tag, StringComparison.OrdinalIgnoreCase))
				{
					content = ReadOnlySpan<char>.Empty;
					return false;
				}

				ReadOnlySpan<char> following = text[tag.Length..];
				if (!following.StartsWith(": "))
				{
					content = ReadOnlySpan<char>.Empty;
					return false;
				}

				content = following[2..].Trim('"').Trim();
				return true;
			}
		}
	}
}
