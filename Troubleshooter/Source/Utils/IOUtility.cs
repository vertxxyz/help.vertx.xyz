using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Troubleshooter
{
	public static class IOUtility
	{
		public static void CopyAll(DirectoryInfo source, DirectoryInfo target, StringBuilder log = null)
		{
			if (string.Equals(source.FullName, target.FullName, StringComparison.Ordinal))
				return;

			// Check if the target directory exists, if not, create it.
			if (Directory.Exists(target.FullName) == false)
				Directory.CreateDirectory(target.FullName);

			// Copy each file into it's new directory.
			foreach (FileInfo fi in source.EnumerateFiles())
			{
				string destination = Path.GetFullPath(Path.Combine(target.ToString(), fi.Name));
				if(File.Exists(destination) && AreFileContentsEqual(destination, fi)) continue;
				log?.AppendLine(destination);
				fi.CopyTo(destination, true);
			}

			// Copy each subdirectory using recursion.
			foreach (DirectoryInfo diSourceSubDir in source.EnumerateDirectories())
			{
				DirectoryInfo nextTargetSubDir =
					target.CreateSubdirectory(diSourceSubDir.Name);
				CopyAll(diSourceSubDir, nextTargetSubDir, log);
			}
		}
		
		public static bool AreFileContentsEqual(FileInfo fi1, FileInfo fi2) =>
			fi1.Length == fi2.Length &&
			(fi1.Length == 0 || File.ReadAllBytes(fi1.FullName).SequenceEqual(
				File.ReadAllBytes(fi2.FullName)));
		
		public static bool AreFileContentsEqual(string path1, string path2) =>
			AreFileContentsEqual(new FileInfo(path1), new FileInfo(path2));
		
		public static bool AreFileContentsEqual(string path1, FileInfo fi2) =>
			AreFileContentsEqual(new FileInfo(path1), fi2);
	}
}