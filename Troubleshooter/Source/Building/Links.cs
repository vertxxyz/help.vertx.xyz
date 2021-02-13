using System;
using System.Collections.Generic;

namespace Troubleshooter
{
	[Serializable]
	public class Links
	{
		public Links(Dictionary<string, string> linksToGuids)
		{
			LinksToGuids = linksToGuids;
			GuidsToLinks = Reverse(linksToGuids);
		}

		public Dictionary<string, string> LinksToGuids { get; }
		public Dictionary<string, string> GuidsToLinks { get; }

		private static Dictionary<TValue, TKey> Reverse<TKey, TValue>(IDictionary<TKey, TValue> source)
		{
			var dictionary = new Dictionary<TValue, TKey>();
			foreach (var entry in source)
			{
				if (!dictionary.ContainsKey(entry.Value))
					dictionary.Add(entry.Value, entry.Key);
			}

			return dictionary;
		}
	}
}