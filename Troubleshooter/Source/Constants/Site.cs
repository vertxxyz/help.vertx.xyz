using System;
using System.IO;

namespace Troubleshooter;

public interface IRootPathProvider
{
	string Root { get; }
}

public sealed class Site
{
	public readonly string
		Root,
		AssetsRoot,
		Directory,
		EmbedsDirectory,
		ContentDirectory;
	
	public Site(IRootPathProvider arguments)
	{
		Root = arguments.Root;
		AssetsRoot = Path.Combine(Root, "Assets");
		Directory = Path.Combine(AssetsRoot, "Site");
		EmbedsDirectory = Path.Combine(AssetsRoot, "Embeds");
		ContentDirectory = Path.Combine(AssetsRoot, "Content");
			
		RootIndex = GetSiteRootIndex(ResourceLocation.Site);
		EmbedRootIndex = GetSiteRootIndex(ResourceLocation.Embed);
	}

	public readonly int RootIndex, EmbedRootIndex;

	public int GetSiteRootIndex(ResourceLocation resourceLocation)
	{
		string directory = resourceLocation switch
		{
			ResourceLocation.Embed => EmbedsDirectory,
			ResourceLocation.Site => Directory,
			_ => throw new ArgumentOutOfRangeException(nameof(resourceLocation), resourceLocation, null)
		};

		string rootDirectory = Path.GetFullPath(directory);
		int siteRootIndex = rootDirectory.Length + 1;
		return siteRootIndex;
	}
		
	public string ConvertFullSitePathToLinkPath(string fullPath)
	{
		string localPath = FinalisePathWithRootIndex(fullPath, RootIndex);
		return Path.ChangeExtension(localPath, null);
	}
		
	public string ConvertFullEmbedPathToLinkPath(string embed)
	{
		string localPath = FinalisePathWithRootIndex(embed, EmbedRootIndex);
		return Path.ChangeExtension(localPath, null);
	}
		
	public static string FinalisePathWithRootIndex(string fullPath, int rootIndex) => fullPath[rootIndex..].FinalisePath();
}