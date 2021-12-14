using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aoc.Utils;

namespace Aoc.Problems.Aoc21;

public class Problem14 : IProblem
{
    public object Solve1(string input)
    {
        var (text, instructions) = ParseInput(input);

        //Console.WriteLine("Template:     " + text);
        for (var step = 1; step <= 10; ++step)
        {
            // Print character counts before taking the step.
            foreach (var charCount in text.GroupBy(c => c).OrderBy(c => c.Key))
            {
                Console.Write($"{charCount.Key}: {charCount.Count()}".PadRight(11));
            }
            Console.WriteLine();

            text = ApplyPairInsert(text, instructions);
        }
        var slowOutput = CalculateScore(text);
        return slowOutput;
    }

    public object Solve2(string input)
    {
        return -1;
        return SolveSmart(input, 40);
    }

    // Does not work.
    public long SolveSmart(string input, int steps)
    {
        var (text, instructions) = ParseInput(input);

        var keys = instructions.Keys.ToList();

        // Count pairs, and characters at once.
        var charCounts = keys.SelectMany(s => s.ToCharArray()).Distinct().ToDictionary(key => key, key => 0L);
        var pairCounts = keys.ToDictionary(key => key, key => 0L);

        // Fill initial pair count.
        for (var i = 0; i < text.Length; ++i)
        {
            charCounts[text[i]]++;
            if (i < text.Length - 1)
            {
                var pair = text.Substring(i, 2);
                pairCounts[pair] = pairCounts[pair] + 1;
            }

        }

        for (var step = 1; step <= steps; ++step)
        {
            PrintCharCounts(charCounts);

            var newPairCounts = new Dictionary<string, long>();
            foreach (var instruction in instructions)
            {
                var pair1 = instruction.Key[0] + instruction.Value;
                var pair2 = instruction.Value + instruction.Key[1];

                // Increase character count.
                charCounts[instruction.Value[0]] = charCounts[instruction.Value[0]] + pairCounts[instruction.Key];

                // Increase both pair counts.
                if (!newPairCounts.TryAdd(pair1, 1))
                {
                    newPairCounts[pair1] = newPairCounts[pair1] + pairCounts[pair1];
                }
                if (!newPairCounts.TryAdd(pair2, 1))
                {
                    newPairCounts[pair2] = newPairCounts[pair2] + pairCounts[pair2];
                }
            }

            // Update pair counts.
            foreach (var key in newPairCounts.Keys)
            {
                pairCounts[key] = pairCounts[key] + newPairCounts[key];
            }
        }

        // Calculate the score based on most and least common character.
        char maxKey = '-', minKey = '-';
        long maxValue = long.MinValue, minValue = long.MaxValue;

        foreach (var kvp in charCounts)
        {
            if (kvp.Value > maxValue)
            {
                maxKey = kvp.Key;
                maxValue = kvp.Value;
            }
            if (kvp.Value < minValue)
            {
                minKey = kvp.Key;
                minValue = kvp.Value;
            }
        }

        Console.WriteLine($"Most common: ({maxKey}, {maxValue}), least common: ({minKey}, {minValue})");
        return maxValue - minValue;
    }

    private void PrintCharCounts(Dictionary<char, long> charCounts)
    {
        foreach (var c in charCounts.Keys.OrderBy(c => c))
        {
            Console.Write($"{c}: {charCounts[c]}".PadRight(11));
        }
        Console.WriteLine();
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

    private string ApplyPairInsert(string text, Dictionary<string, string> instructions)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < text.Length; ++i)
        {
            sb.Append(text[i]);
            if (i < text.Length - 1 && instructions.TryGetValue("" + text[i] + text[i + 1], out var c))
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    private long CalculateScore(string text)
    {
        // Count each character.
        var charCounts = new Dictionary<char, long>();
        foreach (var c in text)
        {
            charCounts.TryAdd(c, 0);
            charCounts[c] = charCounts[c] + 1;
        }

        char maxKey = '-', minKey = '-';
        long maxValue = long.MinValue, minValue = long.MaxValue;

        foreach (var kvp in charCounts)
        {
            if (kvp.Value > maxValue)
            {
                maxKey = kvp.Key;
                maxValue = kvp.Value;
            }
            if (kvp.Value < minValue)
            {
                minKey = kvp.Key;
                minValue = kvp.Value;
            }
        }

        Console.WriteLine($"Most common: ({maxKey}, {maxValue}), least common: ({minKey}, {minValue})");
        return maxValue - minValue;
    }
}
