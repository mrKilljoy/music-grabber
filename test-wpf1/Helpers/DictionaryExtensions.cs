using System.Collections.Generic;

namespace test_wpf1.Helpers
{
    public static class DictionaryExtensions
    {
		public static T TryGet<T>(this IDictionary<string, object> dict, string key)
		{
			if (!dict.ContainsKey(key))
				return default(T);

			if (dict[key].GetType() != typeof(T))
				return default(T);

			return (T)dict[key];
		}
	}
}
