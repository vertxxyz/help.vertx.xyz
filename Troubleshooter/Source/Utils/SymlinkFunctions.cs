using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Troubleshooter;

public class SymlinkFunctions
{
	private readonly Site _site;
	private readonly ILogger<SymlinkFunctions> _logger;

	public SymlinkFunctions(Site site, ILogger<SymlinkFunctions> logger)
	{
		_site = site;
		_logger = logger;
	}

	/// <summary>
	/// Moves any symlinks that were created in the Site directory to the Site Redirects directory.<br/>
	/// Also makes sure that any fully-qualified symlink paths are modified to relative paths.
	/// </summary>
	public void PortAndRepairSymlinks()
	{
		string siteDirectory = _site.Directory;
		string siteRedirectsDirectory = _site.RedirectsDirectory;

		// Force symlinks to relative links.
		foreach (string entry in Directory.EnumerateDirectories(siteRedirectsDirectory, "*", SearchOption.AllDirectories))
		{
			var directory = new DirectoryInfo(entry);
			if (directory.LinkTarget == null) continue;
			if (directory.LinkTarget.Contains(Path.AltDirectorySeparatorChar))
			{
				Directory.Delete(entry);
				Directory.CreateSymbolicLink(entry, directory.LinkTarget.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar));
				directory = new DirectoryInfo(entry);
			}

			if (!Path.IsPathFullyQualified(directory.LinkTarget!))
				continue;
			Directory.Delete(entry);
			Directory.CreateSymbolicLink(entry, Path.GetRelativePath(entry, directory.LinkTarget!));
		}

		foreach (string entry in Directory.EnumerateFiles(siteRedirectsDirectory, "*", SearchOption.AllDirectories))
		{
			var file = new FileInfo(entry);
			if (file.LinkTarget == null) continue;
			if (file.LinkTarget.Contains(Path.AltDirectorySeparatorChar))
			{
				File.Delete(entry);
				File.CreateSymbolicLink(entry, file.LinkTarget.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar));
				file = new FileInfo(entry);
			}

			if (!Path.IsPathFullyQualified(file.LinkTarget!))
				continue;
			File.Delete(entry);
			File.CreateSymbolicLink(entry, Path.GetRelativePath(Path.GetDirectoryName(entry)!, file.LinkTarget!));
		}

		// Port symlinks from Site to Site Redirects.
		Dictionary<string, string> lookup = PageUtility.CollectSymlinkedFilesLookup(siteDirectory);

		foreach (string entry in Directory.EnumerateDirectories(siteDirectory, "*", SearchOption.AllDirectories))
		{
			var directory = new DirectoryInfo(entry);
			if (directory.LinkTarget == null) continue;
			string to = Path.GetFullPath(directory.LinkTarget, Path.GetDirectoryName(entry)!);
			string from = $"{siteRedirectsDirectory}{entry[siteDirectory.Length..]}";
			Map(from, to, false);
		}

		foreach (string entry in Directory.EnumerateFiles(siteDirectory, "*", SearchOption.AllDirectories))
		{
			var file = new FileInfo(entry);
			if (file.LinkTarget == null) continue;
			string to = lookup[entry];
			string from = $"{siteRedirectsDirectory}{entry[siteDirectory.Length..]}";
			Map(from, to, true);
		}

		return;

		void Map(string from, string to, bool isFile)
		{
			string directory = Path.GetDirectoryName(from)!;
			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);

			if (isFile)
			{
				if (File.Exists(from))
					return;
				File.CreateSymbolicLink(from, Path.GetRelativePath(from, to));
			}
			else
			{
				if (Directory.Exists(from))
					return;
				Directory.CreateSymbolicLink(from, Path.GetRelativePath(from, to));
			}

			_logger.LogInformation("Directory: {from} to {to}", from, to);
		}
	}
}
