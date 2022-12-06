using System.Collections.Generic;
using System.Linq;

namespace Aoc.Utils;

public static class EnumerableUtils
{
    /// <summary>
    /// Adds a set of values to the HashSet.
    /// The return value is an AND operation of each individual item adding result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="set"></param>
    /// <param name="values"></param>
    /// <returns>True if all values were added; false if any value collided and was not added.</returns>
    public static bool AddRange<T>(this HashSet<T> set, params T[] values)
    {
        var output = true;
        foreach (var value in values) output &= set.Add(value);
        return output;
    }

    /// <summary>
    /// Removes all input values from the HashSet.
    /// The return value is an AND operation of each individual item removal result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="set"></param>
    /// <param name="values"></param>
    /// <returns>True if each item was removed; False if any of the items did not exist in the Set.</returns>
    public static bool RemoveRange<T>(this HashSet<T> set, params T[] values)
    {
        var output = true;
        foreach (var value in values) output &= set.Remove(value);
        return output;
    }

    /// <summary>
    /// Removes all input values from the HashSet.
    /// The return value is an AND operation of each individual item removal result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="set"></param>
    /// <param name="values"></param>
    /// <returns>True if each item was removed; False if any of the items did not exist in the Set.</returns>
    public static bool RemoveRange<T>(this HashSet<T> set, IEnumerable<T> values) => RemoveRange(set, values.ToArray());
}
