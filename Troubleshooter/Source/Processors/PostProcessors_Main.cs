using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Troubleshooter;

[UsedImplicitly]
public class MainContentToGrid : IHtmlPostProcessor
{
	public int Order => 999;

	private static readonly ImmutableList<string> s_MainPages = ImmutableList.Create(
	
		@"Assets\Site\Main.md"
		// @"Assets\Site\Programming\Entities.md"
	);
	
	public string Process(string html, string fullPath)
	{
		if(!s_MainPages.Any(page => fullPath.EndsWith(page, StringComparison.OrdinalIgnoreCase)))
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
		const string fakeContainerHtml = "<div class=\"grid-container--fake\"></div>";
		const string itemHtml = "<div class=\"grid-item\"></div>";

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
					grid = HasUpcomingH3Header(child) ? HtmlNode.CreateNode(containerHtml) : HtmlNode.CreateNode(fakeContainerHtml);

					break;

					bool HasUpcomingH3Header(HtmlNode htmlNode)
					{
						do
						{
							htmlNode = htmlNode.NextSibling;
							if (htmlNode == null)
								return false;
							if (htmlNode.Name == "h3")
								return true;
						} while (htmlNode.Name != "h2");

						return false;
					}
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
				grid.AppendChild(
					HtmlNode.CreateNode(
						grid.ChildNodes.Count % 2 != 0
							? "<div class=\"grid-item grid-item__extra--even\"><h3>‎</h3></div>" // even
							: "<div class=\"grid-item grid-item__extra--odd\"><h3>‎</h3></div>" // odd
					)
				);
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