using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Web;
using Markdig.Extensions.CustomContainers;
using Markdig.Extensions.Yaml;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using static System.Web.HttpUtility;

namespace Troubleshooter.Renderers;

public interface IHeadRenderer
{
	HeadProducer HeadWriter { get; }
}

public sealed class HeadProducer(StringWriter writer)
{
	public void SetSiteName(in ReadOnlySpan<char> value)
	{
		_siteName = true;
		_writer.WriteLine($"""    <meta property="og:site_name" content="{YamlStringToHtmlString(value)}" />""");
	}

	public void SetTitle(in ReadOnlySpan<char> value)
	{
		_title = true;
		_writer.WriteLine($"""    <meta property="og:title" content="{YamlStringToHtmlString(value)}" />""");
	}

	public void SetType(in ReadOnlySpan<char> value)
	{
		_type = true;
		_writer.WriteLine($"""    <meta property="og:type" content="{YamlStringToHtmlString(value)}" />""");
	}

	public void SetUrl(in ReadOnlySpan<char> value)
	{
		_url = true;
		_writer.WriteLine($"""    <meta property="og:url" content="{YamlStringToHtmlString(value)}" />""");
	}

	public void SetImage(in ReadOnlySpan<char> value, in ReadOnlySpan<char> alt)
	{
		_image = true;
		_writer.WriteLine($"""    <meta property="og:image" content="{YamlStringToHtmlString(value)}" />""");
		if (!value.IsEmpty && !alt.IsEmpty)
			SetImageAlt(alt);
	}

	public void SetLargeImage(in ReadOnlySpan<char> value, in ReadOnlySpan<char> alt)
	{
		_writer.WriteLine("""    <meta name="twitter:card" content="summary_large_image">""");
		_writer.WriteLine($"""    <meta property="twitter:image:src" content="{YamlStringToHtmlString(value)}" />""");
		if (!value.IsEmpty && !alt.IsEmpty)
			SetImageAlt(alt);
	}

	public void SetImageAlt(in ReadOnlySpan<char> value)
		=> _writer.WriteLine($"""    <meta property="og:image:alt" content="{YamlStringToHtmlString(value)}" />""");

	public void SetDescription(in ReadOnlySpan<char> value)
	{
		string encodedValue = YamlStringToHtmlString(value);
		_writer.WriteLine($"""    <meta property="og:description" content="{encodedValue}" />""");
		_writer.WriteLine($"""    <meta property="description" content="{encodedValue}" />""");
	}

	public void SetVideo(in ReadOnlySpan<char> value)
		=> _writer.WriteLine($"""    <meta property="og:video" content="{YamlStringToHtmlString(value)}" />""");

	private static string YamlStringToHtmlString(ReadOnlySpan<char> value)
	{
		// Replace \" with "
		if (value.ContainsAny(SearchValues.Create(@"\""")))
		{
			value = value.ToString().Replace(@"\""", "\"");
		}

		return HtmlEncode(value.ToString());
	}

	// You must reset this value in Reset().
	private bool _siteName, _title, _type, _url, _image;

	// ReSharper disable once ReplaceWithPrimaryConstructorParameter
	private readonly StringWriter _writer = writer;

	public void WriteRequired(string url)
	{
		StringBuilder stringBuilder = _writer.GetStringBuilder();
		// Write nothing if no metadata was specified.
		if (stringBuilder.Length == 0)
			return;
		if (!_siteName) SetSiteName("🤔 Unity, Huh, How?");
		if (!_title) SetTitle("🤔 Unity, Huh, How?");
		if (!_type) SetType("website");
		if (!_url) SetUrl($"https://unity.huh.how/{url}");
		if (!_image) SetImage("", "");
	}

	public void Reset()
	{
		_siteName = _title = _type = _url = _image = false;
		_writer.GetStringBuilder().Length = 0;
	}
}

public sealed class CustomHtmlRenderer : HtmlRenderer, IHeadRenderer
{
	private readonly TextWriter _headWriter;
	private readonly HeadProducer _headProducer;
	private readonly YamlFrontMatterHtmlOverrideRenderer _yamlFrontMatterRenderer;

	private PageResource _currentResource = null!;

	public PageResource CurrentResource
	{
		get => _currentResource;
		set
		{
			_currentResource = value;
			_yamlFrontMatterRenderer.CurrentResource = value;
		}
	}

	public Site Site
	{
		set => _yamlFrontMatterRenderer.Site = value;
	}

	public CustomHtmlRenderer(StringWriter headWriter, TextWriter writer) : base(writer)
	{
		_yamlFrontMatterRenderer = new YamlFrontMatterHtmlOverrideRenderer();
		ObjectRenderers.Insert(0, _yamlFrontMatterRenderer);
		_headWriter = headWriter;
		_headProducer = new HeadProducer(headWriter);
	}

	public void Setup()
	{
		for (int i = ObjectRenderers.Count - 1; i >= 0; i--)
		{
			switch (ObjectRenderers[i])
			{
				case HeadingRenderer:
					ObjectRenderers[i] = new HeadingOverrideRenderer();
					break;
				case HtmlCustomContainerRenderer:
					ObjectRenderers[i] = new HtmlCustomContainerOverrideRenderer();
					break;
				case YamlFrontMatterHtmlRenderer:
					ObjectRenderers.RemoveAt(i);
					break;
				default:
					continue;
			}
		}
	}

	public void DoReset()
	{
		_headProducer.Reset();
		Reset();
	}

	HeadProducer IHeadRenderer.HeadWriter => _headProducer;

	public (string head, string body) ToHtml()
	{
		Writer.Flush();
		_headProducer.WriteRequired(CurrentResource.OutputLink);
		_headWriter.Flush();
		return (
			_headWriter.ToString() ?? string.Empty,
			Writer.ToString() ?? string.Empty
		);
	}
}
