using System;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter
{
	public static class HtmlPostProcessors
	{
		private static readonly IHtmlPostProcessor[] All =
			typeof(IHtmlPostProcessor).Assembly.GetTypes()
				.Where(t => typeof(IHtmlPostProcessor).IsAssignableFrom(t) && !t.IsAbstract)
				.Select(t => (IHtmlPostProcessor) Activator.CreateInstance(t)).ToArray();

		public static string Process(string html) => All.Aggregate(html, (current, processor) => processor.Process(current));
	}
	
	[UsedImplicitly]
	public class RelativeLinkConverter : IHtmlPostProcessor
	{
		private readonly Regex regex = new(@"(?<=<a )href=""([^""]+\.md)""", RegexOptions.Compiled);
		
		public string Process(string html)
		{
			//return Regex.Replace(html, @"(?<=<a )href=""([^""]+\.md)""", "onclick=\"loadPage(\'$1\')\"");
			return StringUtility.ReplaceMatch(html, regex, (group, stringBuilder) =>
			{
				var insert = group.Replace("&amp;", "and");
				insert = insert.Replace("&", "and");
				stringBuilder.Append($"onclick=\"loadPage(\'{insert}\')\"");
			}, 1);
		}
	}

	[UsedImplicitly]
	public class BooleanTableConverter : IHtmlPostProcessor
	{
		public string Process(string html)
		{
			html = html.Replace("<td>Y</td>", "<td onmouseover=\"highlightTable(this)\" onmouseout=\"unhighlightTable(this)\" class=\"tableYes\"></td>");
			return html.Replace("<td>N</td>", "<td onmouseover=\"highlightTable(this)\" onmouseout=\"unhighlightTable(this)\" class=\"tableNo\"></td>");
		}
	}

	[UsedImplicitly]
	public class InfoBoxConverter : IHtmlPostProcessor
	{
		public string Process(string html)
		{
			html = html.Replace("<div class=\"info\"><p>",
				"<div class=\"info\"><img src=\"https://help.vertx.xyz/Images/information.svg\" class=\"info\" alt=\"information\" /><p class=\"info\">");
			html = html.Replace("<div class=\"warning\"><p>",
				"<div class=\"info\"><img src=\"https://help.vertx.xyz/Images/warning.svg\" class=\"info\" alt=\"warning\" /><p class=\"info\">");
			return html.Replace("<div class=\"error\"><p>",
				"<div class=\"info\"><img src=\"https://help.vertx.xyz/Images/error.svg\" class=\"info\" alt=\"error\" /><p class=\"info\">");
		}
	}

	[UsedImplicitly]
	public class SliderConverter : IHtmlPostProcessor
	{
		private readonly Regex regex = new("<div.* class=\"slider\"></div>", RegexOptions.Compiled);
		
		public string Process(string html)
		{
			return StringUtility.ReplaceMatch(html, regex, (group, stringBuilder) =>
			{
				stringBuilder.Append(group[..^6]);
				{
					stringBuilder.Append("<div class=\"slider_container\">");
					stringBuilder.Append("<div class=\"slider_left_gutter\"></div>");
					stringBuilder.Append("<div class=\"slider_right_gutter\"></div>");
					stringBuilder.Append("<div class=\"slider_knob_container\">");
					stringBuilder.Append("<div class=\"slider_knob\"></div>");
					stringBuilder.Append("</div>");
					stringBuilder.Append("</div>");
					//<div class="slider_container">
					//	<div class="slider_left_gutter"></div>
					//	<div class="slider_right_gutter"></div>
					//	<div class="slider_knob_container">
					//		<div class="slider_knob"></div>
					//	</div>
					//</div>
				}
				stringBuilder.Append("</div>");
			});
		}
	}
}