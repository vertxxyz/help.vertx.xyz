using System.IO;

namespace Troubleshooter.Tests;

public class RootPath : IRootPathProvider
{
	public RootPath(string root)
	{
		Root = root;
	}
	public string Root { get; }
}

public static class TestUtility
{
	public static readonly Site TestSite =
		new(new RootPath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../../../Troubleshooter"))));
}