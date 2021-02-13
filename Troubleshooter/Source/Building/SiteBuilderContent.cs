using System.IO;
using Troubleshooter.Constants;

namespace Troubleshooter
{
	public static partial class SiteBuilder
	{
		public static void BuildContent(Arguments arguments, Site site)
			=> IOUtility.CopyAll(new DirectoryInfo(site.ContentDirectory), new DirectoryInfo(arguments.Path));
	}
}