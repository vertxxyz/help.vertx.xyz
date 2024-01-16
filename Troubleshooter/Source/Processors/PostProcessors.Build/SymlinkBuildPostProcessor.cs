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

			string sourceOutput = source.OutputFilePath; // The path of the source file we're copying.
			string resourceOutput = resource.OutputFilePath; // The destination path.

			if ((source.Flags & ResourceFlags.ExistsInOutput) != 0) // Check that the source was actually written.
				RedirectFile(sourceOutput, resourceOutput, source.OutputLink, logger);

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
				 <!DOCTYPE html>
				 <html lang="en" class="rider-dark">
				 <head>
				     <meta charset="utf-8">
				     <meta http-equiv='refresh' content='0; url=/{fromUrl}'>
				     <meta name="viewport" content="width=device-width,user-scalable=no,minimum-scale=1,maximum-scale=1">
				     <meta name="robots" content="noai, noimageai">
				     <title>Unity, huh, how?</title>
				     <!--suppress HtmlUnknownTarget -->
				     <link rel="stylesheet" href="/Styles/style.css?v=2.4.2">
				     <!--suppress HtmlUnknownTarget -->
				     <link rel="preconnect" href="https://fonts.googleapis.com">
				     <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
				     <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;700&family=Roboto:wght@400;700&subset=latin,latin-ext&display=swap"
				           rel="stylesheet">
				     <link rel="icon" type="image/png" href="/Images/favicon-16x16.png" sizes="16x16">
				     <link rel="icon" type="image/png" href="/Images/favicon-32x32.png" sizes="32x32">
				     <link rel="icon" type="image/png" href="/Images/favicon-96x96.png" sizes="96x96">
				 </head>
				 <body>
				 <div id="local-developer-tools" class="header dev-tools-header hidden">
				     <div class="interactive-button" onclick="postText('tools', 'rebuild-all')">Rebuild All</div>
				     <div class="interactive-button" onclick="postText('tools', 'rebuild-content')">Rebuild Content</div>
				 </div>
				 <div class="header">
				     <div class="header__contents">
				         <div class="header__title header__title--large">
				             <a class="" href="/"><img class="emoji" draggable="false" alt="🤔"
				                                       src="https://twemoji.maxcdn.com/v/13.0.1/72x72/1f914.png">
				                 Unity, huh, how?
				             </a></div>
				         <div class="header__title header__title--small">
				             <a class="" href="/"><img class="emoji" draggable="false" alt="🤔"
				                                       src="https://twemoji.maxcdn.com/v/13.0.1/72x72/1f914.png"></a>
				         </div>
				     </div>
				 </div>
				 <div id="container" class="container">
				     <div id="contents" class="contents">
				         <p>If you are not redirected, <a href="/{fromUrl}">click here</a>.</p>
				     </div>
				 </div>
				 </body>
				 </html>
				 """,
				to,
				IOUtility.RecordType.Duplicate
			);
		}
	}
}
