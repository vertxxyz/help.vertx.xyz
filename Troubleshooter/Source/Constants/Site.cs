using System.IO;

namespace Troubleshooter.Constants
{
	public class Site
	{
		public readonly string
			Root,
			AssetsRoot,
			Directory,
			EmbedsDirectory,
			ContentDirectory,
			Main;

		public Site(string root = null)
		{
			Root = root ?? System.IO.Directory.GetCurrentDirectory();
			AssetsRoot = Path.Combine(Root, "Assets");
			Directory = Path.Combine(AssetsRoot, "Site");
			EmbedsDirectory = Path.Combine(AssetsRoot, "Embeds");
			ContentDirectory = Path.Combine(AssetsRoot, "Content");
			Main = Path.Combine(Directory, "Main.md");
		}
	}
}