using System;
using System.Text;

namespace Troubleshooter
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				// Retrieve arguments
				Arguments arguments = new Arguments(args);
				// Register this for RtfPipe
				Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

				MainLoop(arguments);
			}
			catch (Exception e)
			{
				LogExitException(e, "Startup Failed!");
			}
		}

		private static void MainLoop(Arguments arguments)
		{
			try
			{
				ProgramLoop();
			}
			catch (Exception e)
			{
				LogExitException(e, "Operation Failed!");
			}

			void ProgramLoop()
			{
				while (true)
				{
					Console.WriteLine("Press Key to Proceed:");
					Console.WriteLine("B - Build Site");
					Console.WriteLine("C - Content Build Only");
					Console.WriteLine("U - Log External URLs");
					Console.WriteLine("Other - Exit");
					var key = Console.ReadKey().Key;
					Console.Clear();
					switch (key)
					{
						case ConsoleKey.B:
							// Build Site
							SiteBuilder.Build(arguments);
							Console.WriteLine("Successful Build.");
							break;
						case ConsoleKey.C:
							// Content Build
							SiteBuilder.ContentBuild(arguments);
							Console.WriteLine("Successful Content Build.");
							break;
						case ConsoleKey.U:
							SiteLogging.LogAllExternalUrls(arguments);
							break;
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
}