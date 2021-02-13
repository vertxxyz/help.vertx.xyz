using System;
using System.IO;

namespace Troubleshooter
{
	public static class PathUtility
	{
		//https://stackoverflow.com/questions/275689/how-to-get-relative-path-from-absolute-path
		/// <summary>
		/// Creates a relative path from one file or folder to another.
		/// </summary>
		/// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
		/// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
		/// <returns>The relative path from the start directory to the end path or <c>toPath</c> if the paths are not related.</returns>
		/// <exception cref="System.ArgumentNullException"></exception>
		/// <exception cref="System.UriFormatException"></exception>
		/// <exception cref="System.InvalidOperationException"></exception>
		public static string MakeRelativePath(string fromPath, string toPath)
		{
			if (string.IsNullOrEmpty(fromPath)) throw new ArgumentNullException(nameof(fromPath));
			if (string.IsNullOrEmpty(toPath)) throw new ArgumentNullException(nameof(toPath));

			Uri fromUri = new Uri(fromPath);
			Uri toUri = new Uri(toPath);

			if (fromUri.Scheme != toUri.Scheme)
			{
				return toPath;
			} // path can't be made relative.

			Uri relativeUri = fromUri.MakeRelativeUri(toUri);
			string relativePath = relativeUri.ToString();

			if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
				relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

			return relativePath;
		}

		public static string ToConsistentPath(this string path) => path.Replace('/', '\\');
		public static string ToUnTokenized(this string path) => path.Replace("%20", " ");
	}
}