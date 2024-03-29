using System;
using System.Collections.Generic;
using System.IO;

namespace Troubleshooter;

public enum LoggingLevel
{
	Default,
	Verbose
}

public class Arguments : IRootPathProvider
{
	/// <summary>
	/// Output directory
	/// </summary>
	public readonly string Path;
	/// <summary>
	/// Output directory + Json folder
	/// </summary>
	public readonly string? JsonOutputDirectory;
	/// <summary>
	/// Project root (Contains Assets and Source)
	/// </summary>
	public string Root { get; }
	/// <summary>
	/// The localhost url that is running the project's testing environment.
	/// </summary>
	public string Host
	{
		get => _host;
		init => _host = value;
	}
	private string _host;
	public readonly LoggingLevel LoggingLevel;
	public const string JsonOutputDirectoryName = "Json";

	public void OverrideHost(string host) => _host = host;

	public Arguments(IReadOnlyList<string> args)
	{
		_host = "";
		Path = "";
		LoggingLevel = LoggingLevel.Default;
		Root = Directory.GetCurrentDirectory();
		JsonOutputDirectory = null;

		for (var i = 0; i < args.Count; i++)
		{
			string arg = args[i].ToLower().TrimStart('-');

			switch (arg)
			{
				case "h":
				case "help":
				case "/?":
					PrintHelp();
					break;
				case "path":
				case "p":
				case "output":
				case "o":
				{
					if (!TryGetParameter(out var param))
					{
						Console.WriteLine($"\"{args[i]}\" was not followed by a path.");
						continue;
					}

					if (!ValidatePath(param))
						throw new ArgumentException($"\"{param}\" is not a valid path.");

					Path = param!;
					JsonOutputDirectory = System.IO.Path.Combine(Path, JsonOutputDirectoryName);
					Directory.CreateDirectory(JsonOutputDirectory);
					break;
				}
				case "logging":
				case "l":
				{
					if (!TryGetParameter(out var param))
					{
						Console.WriteLine($"\"{args[i]}\" was not followed by a parameter.");
						Console.WriteLine("Valid parameters are \"verbose\".");
						continue;
					}

					switch (param!.ToLower())
					{
						case "verbose":
						case "v":
							LoggingLevel = LoggingLevel.Verbose;
							break;
					}

					break;
				}
			}

			continue;

			bool TryGetParameter(out string? param)
			{
				if (i + 1 >= args.Count)
				{
					param = null;
					return false;
				}

				param = args[i + 1];
				return true;
			}

			static bool ValidatePath(string? path)
			{
				// Path does not exist
				if (string.IsNullOrEmpty(path))
				{
					Console.WriteLine("No \"--path\" argument was passed to the program.");
					Console.WriteLine("Press any key to exit.");
					Console.ReadKey();
					return false;
				}

				// Path is not valid
				try
				{
					string _ = System.IO.Path.GetFullPath(path);
				}
				catch
				{
					return false;
				}

				return true;
			}
		}
	}

	private static void PrintHelp()
	{
		Console.WriteLine("---Arguments:----");
		Console.WriteLine("--help, -h, /?");
		Console.WriteLine("This very output!");
		Console.WriteLine("--path, p, --output, -o");
		Console.WriteLine("The output path for the site content. Must be followed by a valid path.");
		Console.WriteLine("--logging, -l");
		Console.WriteLine("The logging level. Follow by \"verbose\"/\"v\" to get detailed logging.");
		Console.WriteLine("--root-offset");
		Console.WriteLine("If running the project from a directory nested below the Troubleshooter root " +
		                  "you can follow with the relative path to your directory to locate the correct root directory.");
	}

	public void VerboseLog(object @object) => VerboseLog(@object.ToString()!);

	public void VerboseLog(string message)
	{
		if (LoggingLevel == LoggingLevel.Verbose)
			Console.WriteLine(message);
	}
}
