using System;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Troubleshooter;

[UsedImplicitly]
public class MainContentToGrid : IHtmlPostProcessor
{
	public int Order => 999;

	public string Process(string html, string fullPath)
	{
		if (!fullPath.EndsWith(@"Assets\Site\Main.md", StringComparison.OrdinalIgnoreCase))
			return html;

		HtmlDocument doc = new HtmlDocument();
		doc.LoadHtml(html);
		
		HtmlNode node = doc.GetElementbyId("main-page--content");
		if (node == null)
			throw new BuildException("Main.md did not have #main-page--content root!");

		ProcessH3ToTables(node);
		
		return doc.DocumentNode.OuterHtml;
	}

	private static void ProcessH3ToTables(HtmlNode node)
	{
		const string containerHtml = "<div class=\"grid-container\"></div>";
		const string itemHtml = "<div class=\"grid-item\"></div>";
		const int columnCount = 3;

		HtmlNode? h2 = null;
		HtmlNode? grid = null;
		HtmlNode? item = null;
		HtmlNode? child = node.FirstChild;
		while (child != null)
		{
			switch (child.Name)
			{
				case "h2":
				{
					item = null;
					InsertGrid();
					h2 = child;
					grid = HtmlNode.CreateNode(containerHtml);
					break;
				}
				case "h3":
				{
					CreateAndInsertItem();
					HtmlNode h3 = child;
					child = child.PreviousSibling;
					item!.MoveChild(h3);
					break;
				}
				case "#text":
				{
					break;
				}
				default:
				{
					if (item == null)
					{
						grid ??= HtmlNode.CreateNode(containerHtml);
						CreateAndInsertItem();
					}

					HtmlNode n = child;
					child = child.PreviousSibling;
					item!.MoveChild(n);
					break;
				}
			}
			child = child.NextSibling;
		}

		InsertGrid();

		void InsertGrid()
		{
			if (grid == null || h2 == null)
				return;

			if (grid.ChildNodes.Count != 0 && grid.ChildNodes[^1].FirstChild.Name == "h3")
			{
				while (grid.ChildNodes.Count % columnCount != 0)
				{
					grid.AppendChild(HtmlNode.CreateNode("<div class=\"grid-item\"><h3>‎</h3></div>"));
				}
			}

			// Insert grid after the last h2.
			node.InsertAfter(grid, h2);
			grid = null;
		}

		void CreateAndInsertItem()
		{
			if (grid == null)
				return;
			
			item = HtmlNode.CreateNode(itemHtml);
			grid.AppendChild(item);
		}
	}
}