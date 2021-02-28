using System.IO;
using Troubleshooter.Constants;

namespace Troubleshooter.Tests
{
	public class TestUtility
	{
		public static readonly Site TestSite =
			new Site(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../../../Troubleshooter")));
	}
}