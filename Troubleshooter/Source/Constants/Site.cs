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
		RedirectsDirectory,
		EmbedsDirectory,
		ContentDirectory;

	public Site(IRootPathProvider arguments)
	{
		Root = arguments.Root;
		AssetsRoot = Path.Combine(Root, "Assets");
		Directory = Path.Combine(AssetsRoot, "Site");
		RedirectsDirectory = Path.Combine(AssetsRoot, "Site Redirects");
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

	/// <summary>
	/// Converts a Full Path that is in the Site directory to a local path with the necessary text replacement to be link-appropriate. Removes extensions.
	/// </summary>
	public string ConvertFullSitePathToLinkPath(string fullPath)
	{
		string localPath = FinalisePathWithRootIndex(fullPath, RootIndex);
		return Path.ChangeExtension(localPath, null).ToOutputPath();
	}

	/// <summary>
	/// Converts a Full Path that is in the Embeds directory to a local path with the necessary text replacement to be link-appropriate. Removes extensions.
	/// </summary>
	public string ConvertFullEmbedPathToLinkPath(string embed)
	{
		string localPath = FinalisePathWithRootIndex(embed, EmbedRootIndex);
		return Path.ChangeExtension(localPath, null);
	}

	/// <summary>
	/// Converts a Full Path that is in a directory at the rootIndex depth to a local path with the necessary text replacement to be link-appropriate.
	/// </summary>
	public static string FinalisePathWithRootIndex(string fullPath, int rootIndex)
	{
		if (rootIndex > fullPath.Length)
		{
			Console.Error.WriteLine($"{fullPath} failed:");
		}

		return fullPath[rootIndex..].ToFinalisedWorkingPath();
	}
}
