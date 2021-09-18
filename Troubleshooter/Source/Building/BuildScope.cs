using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Troubleshooter
{
	public class BuildScope : IDisposable
	{
		private readonly Arguments arguments;
		private bool failedBuild;
		public BuildScope(Arguments arguments)
		{
			this.arguments = arguments;
			IOUtility.ResetRecording();
		}

		public void MarkBuildAsFailed() => failedBuild = true;

		public void Dispose()
		{
			if(!failedBuild)
				CleanupBuildOutput();
			IOUtility.ResetRecording();
		}

		private void CleanupBuildOutput()
		{
			HashSet<string> recordedFilePaths = new HashSet<string>(IOUtility.RecordedPaths.Select(Path.GetFullPath));
			HashSet<string> redundantFilePaths = new HashSet<string>();
			
			foreach (var file in Directory.EnumerateFiles(arguments.Path, "*", SearchOption.AllDirectories))
			{
				if(recordedFilePaths.Contains(file)) continue;
				redundantFilePaths.Add(file);
			}

			if (redundantFilePaths.Count <= 0)
				return;
			
			Console.WriteLine();
			Console.WriteLine($"Run cleaning step? {redundantFilePaths.Count} redundant files were found.");
			Console.WriteLine($"Example: \"{redundantFilePaths.First()}\"");
			Console.WriteLine("y/n?");

			if (Console.ReadKey().Key != ConsoleKey.Y)
			{
				Console.Clear();
				return;
			}

			Console.Clear();

			foreach (string filePath in redundantFilePaths)
				File.Delete(filePath);
			
			DeleteEmptyDirectories(arguments.Path);
			
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
}