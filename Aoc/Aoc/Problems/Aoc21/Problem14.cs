using System;
using System.Collections.Generic;
using System.Linq;
using Aoc.Utils;

namespace Aoc.Problems.Aoc21;

public class Problem14 : IProblem
{
    public object Solve1(string input)
    {
        return SolveSmart(input, 10);
    }

    public object Solve2(string input)
    {
        return SolveSmart(input, 40);
    }

    // Does not work.
    public long SolveSmart(string input, int steps)
    {
        var (text, instructions) = ParseInput(input);

        // Count initial pairs.
        var (pairCounts, startLetter, endLetter) = ParsePairCounts(text);

        // Make sure all instruction keys have a counter too.
        foreach (var key in instructions.Keys)
        {
            pairCounts.TryAdd(key, 0);
        }

        for (var step = 1; step <= steps; ++step)
        {
            var newPairCounts = instructions.Keys.ToDictionary(k => k, k => 0L);
            foreach (var instruction in instructions)
            {
                // If the key exists, increment the new pair counts and reduce the original.
                var count = pairCounts[instruction.Key];
                if (count > 0)
                {
                    // Build the increment pairs.
                    var pair1 = instruction.Key[0] + instruction.Value;
                    var pair2 = instruction.Value + instruction.Key[1];

                    newPairCounts[instruction.Key] = newPairCounts[instruction.Key] - count;
                    newPairCounts[pair1] = newPairCounts[pair1] + count;
                    newPairCounts[pair2] = newPairCounts[pair2] + count;
                }
            }

            // Update pair counts.
            foreach (var key in newPairCounts.Keys)
            {
                pairCounts[key] = pairCounts[key] + newPairCounts[key];
            }
        }

        return CalculateScore(pairCounts, startLetter, endLetter);
    }

    private (string Text, Dictionary<string, string> Instructions) ParseInput(string input)
    {
        var lines = input.SplitLines();

        var instructions = new Dictionary<string, string>();

        // First line is the template, second is the empty separator line.
        foreach (var line in lines.Skip(2))
        {
            var parts = line.Split(" -> ");
            instructions.Add(parts[0], parts[1]);
        }

        return (lines[0], instructions);
    }

    public static (Dictionary<string, long> PairCounts, char StartChar, char EndChar) ParsePairCounts(string input)
    {
        // Count pairs, and characters at once.
        var pairCounts = new Dictionary<string, long>();

        // Memorize the first and last letters, these need to be used to correct for the final sum.
        char startLetter = input[0];
        char endLetter = input[input.Length - 1];

        // Fill initial pair count.
        for (var i = 0; i < input.Length; ++i)
        {
            if (i < input.Length - 1)
            {
                var pair = input.Substring(i, 2);
                pairCounts.TryAdd(pair, 0);
                pairCounts[pair] = pairCounts[pair] + 1;
            }
        }

        return (pairCounts, startLetter, endLetter);
    }

    public static long CalculateScore(Dictionary<string, long> pairCounts, char startChar, char endChar)
    {
        // Count each character.
        var charCounts = new Dictionary<char, long>();

        // For each pair in the chain, count only the first letter.
        // The last letter is the first letter of the following pair.
        foreach (var pair in pairCounts)
        {
            charCounts.TryAdd(pair.Key[0], 0);
            charCounts[pair.Key[0]] = charCounts[pair.Key[0]] + pair.Value;
        }

        // Correct for start and end letters.
        //charCounts[startChar] = charCounts[startChar] + 1;

        // Add the final letter once for correction.
        charCounts.TryAdd(endChar, 0);
        charCounts[endChar] = charCounts[endChar] + 1;

        char maxKey = '-', minKey = '-';
        long maxValue = long.MinValue, minValue = long.MaxValue;

        foreach (var kvp in charCounts)
        {
            if (kvp.Value > maxValue)
            {
                maxKey = kvp.Key;
                maxValue = kvp.Value;
            }
            // Ignore zeroes.
            if (kvp.Value > 0 && kvp.Value < minValue)
            {
                minKey = kvp.Key;
                minValue = kvp.Value;
            }
        }

        Console.WriteLine($"Most common: ({maxKey}: {maxValue}), least common: ({minKey}: {minValue}). Result: {maxValue - minValue}");
        return maxValue - minValue;
    }
}
