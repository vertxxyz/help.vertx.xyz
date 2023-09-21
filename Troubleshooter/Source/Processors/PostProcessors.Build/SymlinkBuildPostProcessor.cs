using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Copies any linked pages that are associated with pages marked as symlinks.<br/>
/// Also modifies their links so they are root-level.
/// </summary>
[UsedImplicitly]
public sealed class SymlinkBuildPostProcessor : IBuildPostProcessor
{
	public void Process(Arguments arguments, PageResourcesLookup resources, Site site)
	{
		foreach ((string context, PageResource resource) in resources.Where(r => (r.Value.Flags & ResourceFlags.Symlink) != 0))
		{
			string targetLink = resource.SymlinkFullPath!;

			if (!resources.TryGetValue(targetLink, out PageResource? source))
				throw new BuildException($"Symlinked resource \"{targetLink}\" was not found when processing \"{context}\".");

			string sourceOutput = source.OutputFilePath; // The path of the source file we're copying.
			string resourceOutput = resource.OutputFilePath; // The destination path.
			if ((source.Flags & ResourceFlags.ExistsInOutput) != 0) // Check that the source was actually written.
				RedirectFile(sourceOutput, resourceOutput);

			// Redirect resources generated from the base resource if they exist...
			if (!source.HasGeneratedChildren)
				continue;

			string sourceDirectory = Path.GetDirectoryName(sourceOutput)!; // The directory of the source file we're copying.
			string resourceDirectory = Path.GetDirectoryName(resourceOutput)!; // The destination directory.

			foreach (PageResource child in source.GeneratedChildren)
			{
				string childSourceOutput = child.OutputFilePath; // The path of the source file we're copying.
				string childResourceOutput = Path.GetFullPath(Path.GetRelativePath(sourceDirectory, childSourceOutput), resourceDirectory); // The destination path.
				// string childSourceDirectory = Path.GetDirectoryName(childSourceOutput)!; // The directory of the source file we're copying.
				if ((child.Flags & ResourceFlags.ExistsInOutput) != 0)
					RedirectFile(childSourceOutput, childResourceOutput);
			}
		}

		return;

		static void RedirectFile(string from, string to)
		{
			Console.WriteLine($"""
			                   "{from}" -> "{to}"
			                   """);
			// Make a copy of the file at the destination directory.
			RedirectBuildUtilities.CopyFileIfDifferentAndRedirectInternalLinks(new FileInfo(from), to);
		}
	}
}