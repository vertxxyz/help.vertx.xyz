using System.IO;
using Troubleshooter.Constants;

namespace Troubleshooter;

public static partial class SiteBuilder
{
	/// <summary>
	/// Converts a full site path (a path that includes the drive root) to a link path that is relative to the destination site folder.
	/// </summary>
	private static string ConvertRootFullSitePathToLinkPath(string fullPath, string extension, Site site, Arguments arguments)
	{
		(string fileNameWithoutExtension, string fullPathWithoutFileName) = ProcessPath(fullPath, extension);
		string outputPath = Path.Combine(arguments.HtmlOutputDirectory, site.ConvertFullSitePathToLinkPath(fullPathWithoutFileName), $"{fileNameWithoutExtension}{extension}");
		return outputPath;
	}

	/// <summary>
	/// Converts a full embed path (a path that includes the drive root) to a link path that is relative to the Embeds folder in the destination.
	/// </summary>
	private static string ConvertFullEmbedPathToLinkPath(string fullPath, string extension, Site site, Arguments arguments)
	{
		(string fileNameWithoutExtension, string fullPathWithoutFileName) = ProcessPath(fullPath, extension);
		string outputPath = Path.Combine(arguments.HtmlOutputDirectory, "Embeds", site.ConvertFullEmbedPathToLinkPath(fullPathWithoutFileName), $"{fileNameWithoutExtension}{extension}");
		return outputPath;
	}

	/// <summary>
	/// Returns common information about a path.
	/// </summary>
	private static (string fileNameWithoutExtension, string fullPathWithoutFileName) ProcessPath(string fullPath, string extension)
	{
		string fileNameWithExtension = Path.GetFileName(fullPath);
		return (fileNameWithExtension[..^extension.Length], fullPath[..^fileNameWithExtension.Length]);
	}
}