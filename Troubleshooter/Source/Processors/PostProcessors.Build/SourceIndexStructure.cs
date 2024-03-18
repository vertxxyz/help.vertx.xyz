using System.Collections.Generic;

namespace Troubleshooter.Issues;

[System.Serializable]
public sealed class SourceIndexStructure(Dictionary<string, string> pageToSourcePath)
{
	public Dictionary<string, string> PageToSourcePath { get; } = pageToSourcePath;
}
