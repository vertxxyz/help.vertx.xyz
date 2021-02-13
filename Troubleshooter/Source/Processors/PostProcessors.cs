using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Troubleshooter
{
	public static class HtmlPostProcessors
	{
		public static readonly IHtmlPostProcessor[] All =
			Assembly.GetAssembly(typeof(IHtmlPostProcessor)).GetTypes()
				.Where(t => typeof(IHtmlPostProcessor).IsAssignableFrom(t) && !t.IsAbstract)
				.Select(t => (IHtmlPostProcessor) Activator.CreateInstance(t)).ToArray();

		public static string Process(string html) => All.Aggregate(html, (current, processor) => processor.Process(current));
	}
	
	// ReSharper disable once UnusedType.Global
	public class RelativeLinkConverter : IHtmlPostProcessor
	{
		public string Process(string html) => Regex.Replace(html, @"(?<=<a )href=""([\w/-]+)""", "onclick=\"loadPage(\'$1\')\"");
	}

	// ReSharper disable once UnusedType.Global
	public class BooleanTableConverter : IHtmlPostProcessor
	{
		public string Process(string html)
		{
			html = html.Replace("<td>Y</td>", "<td onmouseover=\"highlightTable(this)\" onmouseout=\"unhighlightTable(this)\" class=\"tableYes\"></td>");
			return html.Replace("<td>N</td>", "<td onmouseover=\"highlightTable(this)\" onmouseout=\"unhighlightTable(this)\" class=\"tableNo\"></td>");
		}
	}

	// ReSharper disable once UnusedType.Global
	public class InfoBoxConverter : IHtmlPostProcessor
	{
		public string Process(string html)
		{
			html = html.Replace("<div class=\"info\"><p>",
				"<div class=\"info\"><img src=\"http://help.vertx.xyz/Images/information.svg\" class=\"info\" alt=\"information\" /><p class=\"info\">");
			html = html.Replace("<div class=\"warning\"><p>",
				"<div class=\"info\"><img src=\"http://help.vertx.xyz/Images/warning.svg\" class=\"info\" alt=\"warning\" /><p class=\"info\">");
			return html.Replace("<div class=\"error\"><p>",
				"<div class=\"info\"><img src=\"http://help.vertx.xyz/Images/error.svg\" class=\"info\" alt=\"error\" /><p class=\"info\">");
		}
	}
}