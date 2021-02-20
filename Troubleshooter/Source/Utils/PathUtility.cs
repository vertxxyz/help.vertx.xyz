namespace Troubleshooter
{
	public static class PathUtility
	{
		public static string ToConsistentPath(this string path) => path.Replace('/', '\\');
		public static string ToUnTokenized(this string path) => path.Replace("%20", " ");
	}
}