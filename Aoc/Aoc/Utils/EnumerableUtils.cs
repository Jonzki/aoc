using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Utils
{
    public static class EnumerableUtils
    {
        public static bool AddRange<T>(this HashSet<T> set, params T[] values)
        {
            var output = true;
            foreach (var value in values) output &= set.Add(value);
            return output;
        }

        public static bool RemoveRange<T>(this HashSet<T> set, params T[] values)
        {
            var output = true;
            foreach (var value in values) output &= set.Remove(value);
            return output;
        }
    }
}
