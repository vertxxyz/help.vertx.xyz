using System.Collections.Generic;
using System.Linq;

namespace Troubleshooter.Search;

[System.Serializable]
public class SearchIndexStructure
{
	public IList<string> FilePaths { get; private set; }
	public IList<string> FileHeaders { get; private set; }
	public Dictionary<string, int[]> TermsToFileIndices { get; private set; }
	public Dictionary<string, int> Common { get; private set; }

	public SearchIndexStructure(IList<string> filePaths, IList<string> fileHeaders, Dictionary<string, int[]> termsToFileIndices, IList<string> common)
	{
		FilePaths = filePaths;
		FileHeaders = fileHeaders;
		TermsToFileIndices = termsToFileIndices;
		Common = common.ToDictionary(a => a, _ => 1);
	}
}