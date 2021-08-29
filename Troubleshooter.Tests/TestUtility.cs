using System.IO;
using Troubleshooter.Constants;

namespace Troubleshooter.Tests
{
	public static class TestUtility
	{
		public static readonly Site TestSite =
			new(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../../../Troubleshooter")));
	}
}