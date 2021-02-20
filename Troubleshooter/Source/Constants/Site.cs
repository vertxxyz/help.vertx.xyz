using System.IO;

namespace Troubleshooter.Constants
{
	public class Site
	{
		public readonly string
			AssetsRoot,
			Directory,
			EmbedsDirectory,
			ContentDirectory,
			Main;

		public Site(string root = null)
		{
			AssetsRoot = root != null ? Path.Combine(root, "Assets") : Path.GetFullPath("Assets");
			Directory = Path.Combine(AssetsRoot, "Site");
			EmbedsDirectory = Path.Combine(AssetsRoot, "Embeds");
			ContentDirectory = Path.Combine(AssetsRoot, "Content");
			Main = Path.Combine(Directory, "Main.md");
		}
	}
}