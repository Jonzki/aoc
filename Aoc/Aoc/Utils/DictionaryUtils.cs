using System.Collections.Generic;

namespace Aoc.Utils
{
    public static class DictionaryUtils
    {
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.TryAdd(key, value)) dictionary[key] = value;
        }
    }
}