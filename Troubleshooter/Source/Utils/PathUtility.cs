using System.IO;

namespace Troubleshooter;

public static class PathUtility
{
	public static string ToConsistentPath(this string path) => path.Replace('\\', '/');
	public static string ToUnTokenized(this string path) => path.Replace("%20", " ");
		
	public static string FinalisePath(this string path)
	{
		path = path.Replace("&", "and");
		path = path.Replace('\\', '/');
		return StringUtility.ToLowerSnakeCase(path);
	}

	public static string FinaliseDirectoryPathOnly(this string path)
	{
		if (!Path.HasExtension(path))
			return path.FinalisePath();
		string fileName = Path.GetFileName(path);
		string directory = Path.GetDirectoryName(path)!;
		return Path.Combine(directory.FinalisePath(), fileName).ToConsistentPath();
	}
}