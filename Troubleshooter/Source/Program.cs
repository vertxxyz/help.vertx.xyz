using System;
using System.Text;
using System.Threading.Tasks;
using Troubleshooter.Search;

namespace Troubleshooter;

class Program
{
	static async Task Main(string[] args)
	{
		try
		{
			// Retrieve arguments
			Arguments arguments = new Arguments(args);
			// Register this for RtfPipe
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			await MainLoop(arguments);
		}
		catch (Exception e)
		{
			LogExitException(e, "Startup failed!");
		}
	}

	private static async Task MainLoop(Arguments arguments)
	{
		try
		{
			await ProgramLoop();
		}
		catch (Exception e)
		{
			LogExitException(e, "Operation failed!");
		}

		async Task ProgramLoop()
		{
			while (true)
			{
				Console.WriteLine("Press key to proceed:");
				Console.WriteLine("  B - Build site");
				Console.WriteLine("  C - Content build only");
				Console.WriteLine("  U - Log external URLs");
#if WINDOWS
				Console.WriteLine("  R - Create rich-text embed from clipboard");
#endif
				Console.WriteLine("  Other - Exit");
				var key = Console.ReadKey().Key;
				Console.WriteLine();
				Console.Clear();
				switch (key)
				{
					case ConsoleKey.B:
						// Build Site
						if (SiteBuilder.Build(arguments))
						{
							Console.WriteLine("Successful build, generating search index.");
							await SearchIndex.Generate(arguments);
							Console.WriteLine("Search index generated.");
						}
						else
						{
							Console.WriteLine("Build failed! Press key to continue.");
							Console.ReadKey();
							Console.Clear();
						}
						break;
					case ConsoleKey.C:
						// Content Build
						SiteBuilder.ContentBuild(arguments);
						Console.WriteLine("Successful Content Build.");
						break;
					case ConsoleKey.U:
						SiteLogging.LogAllExternalUrls(arguments);
						break;
#if WINDOWS
					case ConsoleKey.R:
						RtfClip.CreateRtfFile(arguments);
						break;
#endif
					default:
						return;
				}
			}
		}
	}

	private static void LogExitException(Exception e, string message)
	{
		Console.WriteLine("----------------");
		Console.WriteLine(message);
		Console.WriteLine("----------------");
		Console.WriteLine(e.Message);
		Console.WriteLine();
		Console.WriteLine(e.StackTrace);
		Console.WriteLine("----------------");

		Console.WriteLine("Press any key to exit.");
		Console.ReadKey();
	}
}