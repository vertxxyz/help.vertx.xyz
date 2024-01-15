using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Troubleshooter;

/// <summary>
/// Copies any linked pages that are associated with pages marked as symlinks.<br/>
/// Also modifies their links so they are root-level.
/// </summary>
[UsedImplicitly]
public sealed class SymlinkBuildPostProcessor : IBuildPostProcessor
{
	public void Process(Arguments arguments, PageResourcesLookup resources, Site site, ILogger logger)
	{
		foreach ((string context, PageResource resource) in resources.Where(r => (r.Value.Flags & ResourceFlags.Symlink) != 0))
		{
			string targetLink = resource.SymlinkFullPath!;

			if (!resources.TryGetValue(targetLink, out PageResource? source))
				throw new BuildException($"Symlinked resource \"{targetLink}\" was not found when processing \"{context}\".");

			string sourceOutput = source.OutputContentFilePath; // The path of the source file we're copying.
			string resourceOutput = resource.OutputContentFilePath; // The destination path.

			if ((source.Flags & ResourceFlags.ExistsInOutput) != 0) // Check that the source was actually written.
				RedirectFile(sourceOutput, resourceOutput, source.OutputLink, logger);

			// Redirect resources generated from the base resource if they exist...
			if (!source.HasGeneratedChildren)
				continue;

			string sourceDirectory = Path.GetDirectoryName(sourceOutput)!; // The directory of the source file we're copying.
			string resourceDirectory = Path.GetDirectoryName(resourceOutput)!; // The destination directory.

			foreach (PageResource child in source.GeneratedChildren)
			{
				string childSourceOutput = child.OutputContentFilePath; // The path of the source file we're copying.
				string childResourceOutput = Path.GetFullPath(Path.GetRelativePath(sourceDirectory, childSourceOutput), resourceDirectory); // The destination path.
				// string childSourceDirectory = Path.GetDirectoryName(childSourceOutput)!; // The directory of the source file we're copying.
				if ((child.Flags & ResourceFlags.ExistsInOutput) != 0)
					RedirectFile(childSourceOutput, childResourceOutput, child.OutputLink, logger);
			}
		}

		return;

		static void RedirectFile(string from, string to, string fromUrl, ILogger logger)
		{
			if (to.EndsWith(Constants.SidebarSuffix, StringComparison.Ordinal))
				return;

			logger.LogDebug(
				"""
				"{From}" -> "{To}"
				""",
				from,
				to
			);

			IOUtility.WriteFileTextIfDifferent(
				// language=html
				$"""
				 <div id="force-redirect" class="hidden">/{fromUrl}</div>
				 <p>If you are not redirected, <a href="/{fromUrl}">click here</a>.</p>
				 """,
				to,
				IOUtility.RecordType.Duplicate
			);
		}
	}
}
