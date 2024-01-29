// #define DONT_WRITE_FILES

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troubleshooter;

public static class IOUtility
{
	public delegate FileResult.Validity FileProcessor(FileInfo file, out FileResult? result);

	public class FileResult(string content, string fileNameWithExtension)
	{
		public enum Validity
		{
			Skipped, // The file is to be skipped (not copied)
			NotProcessed, // The file is to be copied
			Processed // Processed to a FileResult
		}

		public string Content { get; } = content;
		public string FileNameWithExtension { get; } = fileNameWithExtension;
	}

	public enum RecordType : byte
	{
		/// <summary>
		/// A normal page.
		/// </summary>
		Normal,
		/// <summary>
		/// An index page, this is recorded to avoid considering them when generating the search index.
		/// </summary>
		Index,
		/// <summary>
		/// Pages that are duplicates of others (usually via a symlink).
		/// </summary>
		Duplicate
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
					if (!CopyFileIfDifferent(destination, fi, RecordType.Normal)) continue;
					break;
				case FileResult.Validity.Processed:
					destination = Path.GetFullPath(Path.Combine(target.ToString(), result!.FileNameWithExtension));
					File.WriteAllText(destination, result.Content);
					Record(destination, RecordType.Normal);
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

	private static bool AreFileContentsEqual(FileInfo fi1, FileInfo fi2) =>
		fi1.Length == fi2.Length &&
		(fi1.Length == 0 || File.ReadAllBytes(fi1.FullName).SequenceEqual(
			File.ReadAllBytes(fi2.FullName)));

	private static bool AreFileContentsEqual(string path1, FileInfo fi2) =>
		AreFileContentsEqual(new FileInfo(path1), fi2);

	private static bool IsFileTextEqual(string text, string destinationPath) =>
		text.Length == 0 || text.SequenceEqual(File.ReadAllText(destinationPath));

	public static bool CreateFileIfDifferent(string fullPath, string contents, RecordType type)
	{
		Record(fullPath, type);

#if DONT_WRITE_FILES
		return false;
#else
		string directory = Path.GetDirectoryName(fullPath)!;
		Directory.CreateDirectory(directory);
		if (File.Exists(fullPath) && string.Equals(File.ReadAllText(fullPath), contents, StringComparison.Ordinal))
			return false;
		File.WriteAllText(fullPath, contents);
		return true;
#endif
	}

	public static async Task<bool> CreateFileIfDifferentAsync(string fullPath, string contents, RecordType type)
	{
		Record(fullPath, type);

#if DONT_WRITE_FILES
		return false;
#else
		string directory = Path.GetDirectoryName(fullPath)!;
		Directory.CreateDirectory(directory);
		if (File.Exists(fullPath) && string.Equals(await File.ReadAllTextAsync(fullPath), contents, StringComparison.Ordinal))
			return false;
		await File.WriteAllTextAsync(fullPath, contents);
		return true;
#endif
	}

	/// <summary>
	/// A copy of the recorded paths.
	/// </summary>
	public static IEnumerable<string> RecordedPaths => s_recordedPaths.Keys;
	private static readonly ConcurrentDictionary<string, RecordType> s_recordedPaths = new();

	/// <summary>
	/// A copy of the recorded paths and their associated values.
	/// </summary>
	public static ReadOnlyDictionary<string, RecordType> RecordedPathsLookup => s_recordedPaths.ToDictionary(kvp => kvp.Key, kvp => kvp.Value).AsReadOnly();

	public static void ResetRecording() => s_recordedPaths.Clear();

	/// <summary>
	/// Copy <see cref="File"/> to <see cref="path"/>, and mark it as <see cref="type"/>.
	/// </summary>
	/// <returns>True if the file was written to disk.</returns>
	public static bool CopyFileIfDifferent(string path, FileInfo file, RecordType type)
	{
		Record(path, type);

#if DONT_WRITE_FILES
		return false;
#else
		string directory = Path.GetDirectoryName(path)!;
		Directory.CreateDirectory(directory);
		if (File.Exists(path) && AreFileContentsEqual(path, file)) return false;
		file.CopyTo(path, true);
		return true;
#endif
	}

	/// <summary>
	/// Copy <see cref="text"/> to <see cref="path"/>, and mark it as <see cref="type"/>.
	/// </summary>
	/// <returns>True if the file was written to disk.</returns>
	public static bool WriteFileTextIfDifferent(string text, string path, RecordType type)
	{
		Record(path, type);

#if DONT_WRITE_FILES
		return false;
#else
		string directory = Path.GetDirectoryName(path)!;
		Directory.CreateDirectory(directory);
		if (File.Exists(path) && IsFileTextEqual(text, path)) return false;
		File.WriteAllText(path, text);
		return true;
#endif
	}

	private static void Record(string path, RecordType type)
	{
		if (!s_recordedPaths.TryAdd(path, type))
			s_recordedPaths[path] = (RecordType)Math.Min((byte)type, (byte)s_recordedPaths[path]);
	}
}
