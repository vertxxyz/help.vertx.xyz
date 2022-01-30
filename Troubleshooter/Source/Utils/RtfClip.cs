#if WINDOWS
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Troubleshooter.Constants;

namespace Troubleshooter
{
	public static class RtfClip
	{
		[Flags]
		public enum Stripping
		{
			None,
			Bold = 1,
			Italics = 1 << 1,
			All = Bold | Italics
		}

		private static string PerformStripping(this Stripping value, string input)
		{
			if (value.HasFlag(Stripping.Bold))
			{
				input = Replace(input, "b");
				input = Replace(input, "b0");
			}

			if (value.HasFlag(Stripping.Italics))
			{
				input = Replace(input, "i");
				input = Replace(input, "i0");
			}

			return input;

			static string Replace(string input, string rtfTag) => Regex.Replace(input, @$"\\{rtfTag}\b", string.Empty);
		}
		
		public static void CreateRtfFile(Arguments arguments, Stripping stripping = Stripping.None)
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
			string fileName = Console.ReadLine()!;
			string codeEmbedDirectory = Path.Combine(site.EmbedsDirectory, "Code");
			
			// Find valid file name.
			string outputPath = Path.Combine(codeEmbedDirectory, $"{fileName}.rtf");
			int index = 0;
			while (File.Exists(outputPath))
			{
				index++;
				outputPath = Path.Combine(codeEmbedDirectory, $"{fileName} {index.ToString()}.rtf");
			}

			rtf = stripping.PerformStripping(rtf);
			
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