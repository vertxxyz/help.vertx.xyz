using System.IO;
using Troubleshooter.Constants;

namespace Troubleshooter.Tests
{
	public class TestUtility
	{
		public static readonly Site TestSite = new Site(Directory.GetCurrentDirectory()[..^".Tests\\bin\\Release\\netcoreapp3.1".Length]);
	}
}