#define DONT_WRITE_FILES
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troubleshooter;

public static class IOUtility
{
	public delegate FileResult.Validity FileProcessor(FileInfo file, out FileResult? result);

	public class FileResult
	{
		public enum Validity
		{
			Skipped, // The file is to be skipped (not copied)
			NotProcessed, // The file is to be copied
			Processed // Processed to a FileResult
		}

		public string Content { get; }
		public string FileNameWithExtension { get; }

		public FileResult(string content, string fileNameWithExtension)
		{
			Content = content;
			FileNameWithExtension = fileNameWithExtension;
		}
	}

	public static void CopyAll(DirectoryInfo source, DirectoryInfo target, StringBuilder? log = null, FileProcessor? fileProcessor = null)
	{
#if !DONT_WRITE_FILES
		if (string.Equals(source.FullName, target.FullName, StringComparison.Ordinal))
			return;

		Directory.CreateDirectory(target.FullName);

		// Copy each file into it's new directory.
		foreach (FileInfo fi in source.EnumerateFiles())
		{
			FileResult? result = null;
			var validity = fileProcessor?.Invoke(fi, out result) ?? FileResult.Validity.NotProcessed;
			string destination;
			switch (validity)
			{
				case FileResult.Validity.Skipped:
					continue; // Skipped, do not copy.
				case FileResult.Validity.NotProcessed:
					destination = Path.GetFullPath(Path.Combine(target.ToString(), fi.Name));
					if (!CopyFileIfDifferent(destination, fi)) continue;
					break;
				case FileResult.Validity.Processed:
					destination = Path.GetFullPath(Path.Combine(target.ToString(), result!.FileNameWithExtension));
					File.WriteAllText(destination, result.Content);
					recordedPaths.TryAdd(destination, 0);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			log?.AppendLine(destination);
		}

		// Copy each subdirectory using recursion.
		foreach (DirectoryInfo diSourceSubDir in source.EnumerateDirectories())
		{
			DirectoryInfo nextTargetSubDir =
				target.CreateSubdirectory(diSourceSubDir.Name);
			CopyAll(diSourceSubDir, nextTargetSubDir, log, fileProcessor);
		}
#endif
	}

	public static bool AreFileContentsEqual(FileInfo fi1, FileInfo fi2) =>
		fi1.Length == fi2.Length &&
		(fi1.Length == 0 || File.ReadAllBytes(fi1.FullName).SequenceEqual(
			File.ReadAllBytes(fi2.FullName)));

	public static bool AreFileContentsEqual(string path1, string path2) =>
		AreFileContentsEqual(new FileInfo(path1), new(path2));

	public static bool AreFileContentsEqual(string path1, FileInfo fi2) =>
		AreFileContentsEqual(new FileInfo(path1), fi2);

	public static bool IsFileTextEqual(string text, string destinationPath) =>
		text.Length == 0 || text.SequenceEqual(File.ReadAllText(destinationPath));

	public static bool CreateFileIfDifferent(string fullPath, string contents)
	{
		recordedPaths.TryAdd(fullPath, 0);
		
#if DONT_WRITE_FILES
		return false;
#endif

		string directory = Path.GetDirectoryName(fullPath)!;
		Directory.CreateDirectory(directory);
		if (File.Exists(fullPath) && string.Equals(File.ReadAllText(fullPath), contents, StringComparison.Ordinal))
			return false;
		File.WriteAllText(fullPath, contents);
		return true;
	}

	public static async Task<bool> CreateFileIfDifferentAsync(string fullPath, string contents)
	{
		recordedPaths.TryAdd(fullPath, 0);
		
#if DONT_WRITE_FILES
		return false;
#endif

		string directory = Path.GetDirectoryName(fullPath)!;
		Directory.CreateDirectory(directory);
		if (File.Exists(fullPath) && string.Equals(await File.ReadAllTextAsync(fullPath), contents, StringComparison.Ordinal))
			return false;
		await File.WriteAllTextAsync(fullPath, contents);
		return true;
	}

	public static IEnumerable<string> RecordedPaths => recordedPaths.Keys;
	private static readonly ConcurrentDictionary<string, byte> recordedPaths = new();

	public static void RecordFakeFile(string fullPath) => recordedPaths.TryAdd(fullPath, 0);

	public static void ResetRecording() => recordedPaths.Clear();

	public static bool CopyFileIfDifferent(string destinationFullPath, FileInfo file)
	{
		recordedPaths.TryAdd(destinationFullPath, 0);
		
#if DONT_WRITE_FILES
		return false;
#endif

		string directory = Path.GetDirectoryName(destinationFullPath)!;
		Directory.CreateDirectory(directory);
		if (File.Exists(destinationFullPath) && AreFileContentsEqual(destinationFullPath, file)) return false;
		file.CopyTo(destinationFullPath, true);
		return true;
	}

	public static bool WriteFileTextIfDifferent(string text, FileInfo from, string destinationFullPath)
	{
		recordedPaths.TryAdd(destinationFullPath, 0);
		
#if DONT_WRITE_FILES
		return false;
#endif

		string directory = Path.GetDirectoryName(destinationFullPath)!;
		Directory.CreateDirectory(directory);
		if (File.Exists(destinationFullPath) && IsFileTextEqual(text, destinationFullPath)) return false;
		from.CopyTo(destinationFullPath, true);
		return true;
	}
}