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


	/// <summary>
	/// Returns a path where only the directory has been passed through <see cref="ToFinalisedWorkingPath(string)"/>.
	/// </summary>
	public static string ToFinalisedWorkingPathDirectoryOnly(this string path)
	{
		if (!Path.HasExtension(path))
			return path.ToFinalisedWorkingPath();
		string fileName = Path.GetFileName(path);
		string directory = Path.GetDirectoryName(path)!;
		return Path.Combine(directory.ToFinalisedWorkingPath(), fileName).ToWorkingPath();
	}

	/// <summary>
	/// The embeds directory as local to the HTML directory.
	/// </summary>
	public static readonly string EmbedsDirectory = "Embeds";

	/// <summary>
	/// The embeds directory as local to the HTML directory in output (link) path format.
	/// </summary>
	public static readonly string EmbedsDirectoryLink = EmbedsDirectory.ToOutputPath();
}
