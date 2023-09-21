using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Troubleshooter.Search;

namespace Troubleshooter;

public sealed class BuildScope : IDisposable
{
	private readonly Arguments _arguments;
	private readonly bool _cleanup;
	private bool _failedBuild;
	public BuildScope(Arguments arguments, bool cleanup)
	{
		_arguments = arguments;
		_cleanup = cleanup;
		IOUtility.ResetRecording();
	}

	public void MarkBuildAsFailed() => _failedBuild = true;

	public void Dispose()
	{
		if(!_failedBuild && _cleanup)
			CleanupBuildOutput();
		IOUtility.ResetRecording();
	}

	private void CleanupBuildOutput()
	{
		HashSet<string> recordedFilePaths = new(IOUtility.RecordedPaths.Select(Path.GetFullPath));
		HashSet<string> redundantFilePaths = new();

		foreach (var file in Directory.EnumerateFiles(_arguments.Path, "*", SearchOption.AllDirectories))
		{
			if(recordedFilePaths.Contains(file)) continue;
			redundantFilePaths.Add(file);
		}

		redundantFilePaths.Remove(SearchIndex.GetJsonFilePath(_arguments));
		redundantFilePaths.RemoveWhere(path =>
		{
			var remaining = path.AsSpan()[(_arguments.Path.Length + 1)..];
			return !remaining.Contains('\\') || remaining.StartsWith(".git\\", StringComparison.Ordinal);
		});

		if (redundantFilePaths.Count <= 0)
			return;

		Console.WriteLine();
		Console.WriteLine($"Run cleaning step? {redundantFilePaths.Count} redundant files were found.");
		Console.WriteLine("Examples:");
		foreach (string path in redundantFilePaths.Take(Math.Min(redundantFilePaths.Count, 10)))
		{
			Console.Write("\"");
			Console.Write(path);
			Console.WriteLine("\"");
		}
		Console.WriteLine("y/n?");

		if (Console.ReadKey().Key != ConsoleKey.Y)
		{
			Console.Clear();
			return;
		}

		Console.Clear();

		foreach (string filePath in redundantFilePaths)
			File.Delete(filePath);

		DeleteEmptyDirectories(_arguments.Path);

		Console.WriteLine("Files and folders cleaned.");
	}

	private static void DeleteEmptyDirectories(string startLocation)
	{
		foreach (var directory in Directory.GetDirectories(startLocation))
		{
			DeleteEmptyDirectories(directory);
			if (!Directory.EnumerateFileSystemEntries(directory).Any())
				Directory.Delete(directory, false);
		}
	}
}
