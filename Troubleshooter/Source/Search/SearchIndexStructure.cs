using System.Collections.Generic;

namespace Troubleshooter.Search
{
	[System.Serializable]
	public class SearchIndexStructure
	{
		public IList<string> FilePaths { get; private set; }
		public IList<string> FileHeaders { get; private set; }
		public Dictionary<string, int[]> WordsToFileIndices { get; private set; }

		public SearchIndexStructure(IList<string> filePaths, IList<string> fileHeaders, Dictionary<string, int[]> wordsToFileIndices)
		{
			FilePaths = filePaths;
			FileHeaders = fileHeaders;
			WordsToFileIndices = wordsToFileIndices;
		}
	}
}