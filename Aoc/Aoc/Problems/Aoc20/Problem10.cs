using Aoc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Problems.Aoc20
{
    public class Problem10 : IProblem
    {
        public object Solve1(string input)
        {
            // Parse adapters.
            var adapters = (input)
                .SplitLines()
                .Select(int.Parse)
                // Part 1 seems to just work by ordering the array.
                .OrderBy(x => x)
                .ToList();

            // Treat the charging outlet near your seat as having an effective joltage rating of 0.
            adapters.Insert(0, 0);

            // Your device has a built-in joltage adapter rated for 3 jolts higher than the highest-rated adapter in your bag.
            adapters.Add(adapters.Last() + 3);

            // What is the number of 1-jolt differences multiplied by the number of 3-jolt differences?
            int diff1 = 0, diff3 = 0;
            for (int i = 0; i < adapters.Count - 1; ++i)
            {
                if (adapters[i + 1] - adapters[i] == 3) ++diff3;
                if (adapters[i + 1] - adapters[i] == 1) ++diff1;
            }

            return diff1 * diff3;
        }

        public object Solve2(string input)
        {
            // Start with the conditions of part 1.
            // Parse adapters.
            var adapters = (input)
                .SplitLines()
                .Select(int.Parse)
                // Part 1 seems to just work by ordering the array.
                .OrderBy(x => x)
                .ToList();

            // Treat the charging outlet near your seat as having an effective joltage rating of 0.
            adapters.Insert(0, 0);

            // Your device has a built-in joltage adapter rated for 3 jolts higher than the highest-rated adapter in your bag.
            adapters.Add(adapters.Last() + 3);

            // Index -> count cache.
            var cache = new Dictionary<int, long>();

            // Return the amount of possible adapters.
            return CountArrangements(adapters, 0, cache);
        }

        /// <summary>
        /// Counts the possible arrangements of the input adapters from the given starting index.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startIndex"></param>
        /// <param name="cache"></param>
        /// <returns></returns>
        public static long CountArrangements(List<int> input, int startIndex = 0, Dictionary<int, long> cache = null)
        {
            long cachedResult = -1;
            if (cache?.TryGetValue(startIndex, out cachedResult) == true) return cachedResult;

            // Start from the given index.
            for (var i = startIndex; i < input.Count; ++i)
            {
                if (i >= input.Count - 1)
                {
                    cache?.TryAdd(i, 1);
                    return 1; // At the end: only one possible arrangement.
                }

                var possibleAdapters = new List<int>();

                for (var j = i + 1; j < input.Count; ++j)
                {
                    if (j >= input.Count) break;

                    // Check if we're over 3 "Jolt" difference.
                    if (input[j] - input[i] > 3) break;

                    // Otherwise, add the adapter index to the list of possibilities.
                    possibleAdapters.Add(j);
                }

                // Exactly 1 possible adapter - continue the loop.
                if (possibleAdapters.Count == 1) continue;

                // Recurse on the possible positions.
                var result = possibleAdapters.Sum(index => CountArrangements(input, index, cache));
                cache?.TryAdd(i, result);
                return result;
            }

            return 1;
        }


    }
}
