namespace Aoc.Utils;

public static class DictionaryUtils
{
    /// <summary>
    /// Adds an item to the Dictionary, 
    /// or updates the existing item if it already exists.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (!dictionary.TryAdd(key, value)) dictionary[key] = value;
    }
}
