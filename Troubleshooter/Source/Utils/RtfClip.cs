#if WINDOWS
using System;
using System.Diagnostics;
using System.IO;
using Troubleshooter.Constants;

namespace Troubleshooter
{
	public static class RtfClip
	{
		public static void CreateRtfFile(Arguments arguments)
		{
			var site = new Site(arguments.TroubleshooterRoot);

			string exePath = Path.Combine(site.Root, "RtfClip.exe");

			string rtf = Get(exePath);
			if (string.IsNullOrEmpty(rtf))
			{
				Console.WriteLine("No rich-text was found in the clipboard.");
				return;
			}
			
			Console.WriteLine("Enter file name without extension:");
			string fileName = Console.ReadLine();
			string codeEmbedDirectory = Path.Combine(site.EmbedsDirectory, "Code");
			
			// Find valid file name.
			string outputPath = Path.Combine(codeEmbedDirectory, $"{fileName}.rtf");
			int index = 0;
			while (File.Exists(outputPath))
			{
				index++;
				outputPath = Path.Combine(codeEmbedDirectory, $"{fileName} {index.ToString()}.rtf");
			}
			
			File.WriteAllText(outputPath, rtf);
			
			Console.Write('\"');
			Console.Write(outputPath);
			Console.WriteLine("\" was created.");
		}
		
		/// <summary>
		/// Gets RTF content from the clipboard.
		/// </summary>
		/// <param name="path">The path of RTFClip.exe</param>
		/// <returns>RTF content</returns>
		private static string Get(string path) => Run(path, "paste");

		/// <summary>
		/// Fills the clipboard's RTF value with content.
		/// </summary>
		/// <param name="path">The path of RTFClip.exe</param>
		/// <param name="rtf">RTF content</param>
		/// <exception cref="LogicException">Thrown if set operation fails</exception>
		private static void Set(string path, string rtf)
		{
			string result = Run(path, $"copy \"{rtf}\"");
			if (string.IsNullOrEmpty(result)) return;
			throw new LogicException($"{nameof(RtfClip)}.{nameof(Set)} has failed with the result of: \"{result}\".");
		}

		private static string Run(string filename, string arguments)
		{
			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = filename,
					Arguments = arguments,
					RedirectStandardOutput = true,
					UseShellExecute = false,
					CreateNoWindow = true
				}
			};
			process.Start();
			string result = process.StandardOutput.ReadToEnd();
			process.WaitForExit();
			return result;
		}
	}
}
#endif