using System;
using System.IO;

namespace Troubleshooter;

public static class PathUtility
{
	/// <summary>
	/// Replaces \ with /. Consistency with urls.
	/// </summary>
	public static string ToOutputPath(this string path)
		=> path.Replace('\\', '/');

	/// <summary>
	/// Replaces / with \. Consistency with windows paths.
	/// </summary>
	public static string ToWorkingPath(this string path)
		=> path.Replace('/', '\\');

	/// <summary>
	/// Replaces tokens like %20 with spaces.
	/// </summary>
	public static string ToUnTokenized(this string path)
		=> path.Replace("%20", " ");

	/// <summary>
	/// Replaces & with and, / with \, and makes the path lowercase.
	/// </summary>
	public static string ToFinalisedWorkingPath(this string path)
		=> StringUtility.ToLowerSnakeCase(path.Replace("&", "and").ToWorkingPath());
	
	public static string ConvertMarkdownPathToSidebarPath(string path) => $"{Path.GetFileNameWithoutExtension(path)}{Constants.SidebarSuffix}";

	/// <summary>
	/// Returns a path where only the directory has been passed through <see cref="ToFinalisedWorkingPath"/>.
	/// </summary>
	public static string ToFinalisedWorkingPathDirectoryOnly(this string path)
	{
		if (!Path.HasExtension(path))
			return path.ToFinalisedWorkingPath();
		string fileName = Path.GetFileName(path);
		string directory = Path.GetDirectoryName(path)!;
		return Path.Combine(directory.ToFinalisedWorkingPath(), fileName).ToWorkingPath();
	}

	public static string ToFinalisedWorkingPath(string path, int safeRootIndex)
	{
		ReadOnlySpan<char> pathSpan = path.AsSpan();
		ReadOnlySpan<char> extension = Path.GetExtension(pathSpan);

		if (safeRootIndex > 0)
		{
			ReadOnlySpan<char> pathFromHtmlRoot = pathSpan[safeRootIndex..];
			ReadOnlySpan<char> rootPath = pathSpan[..safeRootIndex];
			switch (extension)
			{
				case "":
				case ".html":
				case ".md":
					return Path.Combine(rootPath.ToString(), pathFromHtmlRoot.ToString().ToFinalisedWorkingPath());
				default:
					ReadOnlySpan<char> directory = Path.GetDirectoryName(pathFromHtmlRoot);
					return Path.Combine(rootPath.ToString(), directory.ToString().ToFinalisedWorkingPath(), Path.GetFileName(pathFromHtmlRoot).ToString());
			}
		}
		
		switch (extension)
		{
			case "":
			case ".html":
			case ".md":
				return pathSpan.ToString().ToFinalisedWorkingPath();
			default:
				ReadOnlySpan<char> directory = Path.GetDirectoryName(pathSpan);
				return Path.Combine(directory.ToString().ToFinalisedWorkingPath(), Path.GetFileName(pathSpan).ToString());
		}
	}
	
	/// <summary>
	/// The embeds directory as local to the HTML directory.
	/// </summary>
	public static readonly string EmbedsDirectory = Path.Combine(Arguments.HtmlOutputDirectoryName, "Embeds").ToWorkingPath();
	
	/// <summary>
	/// The embeds directory as local to the HTML directory in output (link) path format.
	/// </summary>
	public static readonly string EmbedsDirectoryLink = EmbedsDirectory.ToOutputPath();
}