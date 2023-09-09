using System.Collections.Generic;

namespace Troubleshooter.Issues;

[System.Serializable]
public sealed class SourceIndexStructure
{
	public Dictionary<string, string> PageToSourcePath { get; }

	public SourceIndexStructure(Dictionary<string, string> pageToSourcePath)
	{
		PageToSourcePath = pageToSourcePath;
	}
}