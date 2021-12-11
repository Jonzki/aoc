using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aoc.Utils;

namespace Aoc.Problems.Aoc21;

public class Problem8 : IProblem
{
    public object Solve1(string input)
    {
        var lines = input.SplitLines();

        var count = 0;
        foreach (var line in lines)
        {
            // In the output values,
            var outputValues = line.Split(" | ")[1].Split(' ').ToArray();

            // How many times do digits 1, 4, 7, or 8 appear?
            // Because the digits 1, 4, 7, and 8 each use a unique number of segments,
            // you should be able to tell which combinations of signals correspond to those digits.
            foreach (var outputValue in outputValues)
            {
                if (new[] { 2, 3, 4, 7 }.Contains(outputValue.Length))
                {
                    count++;
                }
            }
        }

        return count;
    }

    public object Solve2(string input)
    {
        var lines = input.SplitLines();

        var output = 0;
        foreach (var line in lines)
        {
            var temp = ParseNumber(line);
            Console.WriteLine(temp);
            output += temp;
        }

        return output;
    }

    public static int ParseNumber(string input)
    {
        var values = input.Split(new[] { "|", " " }, StringSplitOptions.RemoveEmptyEntries);
        var sorted = values.Select(x => new string(x.ToCharArray().OrderBy(c => c).ToArray())).ToArray();

        for (var i = 0; i < sorted.Length; i++)
        {
            Console.WriteLine($"{values[i]} - {sorted[i]}");
        }

        var n = new string[10];

        //  00 
        // 1  2
        // 1  2
        //  33 
        // 4  5
        // 4  5
        //  66
        var digits = new char[7] { '_', '_', '_', '_', '_', '_', '_' };

        bool HasDigits(string input, params int[] digitPositions)
        {
            return digitPositions.All(d => input.Contains(digits[d]));
        }

        bool HasChars(string input, string digits)
        {
            return digits.All(d => input.Contains(d));
        }

        int ResolveNumber(string input)
        {
            // Easy ones.
            if (input.Length == 7) return 8;
            if (input.Length == 3) return 7;
            if (input.Length == 2) return 1;
            if (input.Length == 4) return 4;

            // Use digits for the rest.
            if (input.Length == 5) // 2,3,5
            {
                // Only 5 has digit 1.
                if (HasDigits(input, 1)) return 5;

                // 2 has digit 4.
                if (HasDigits(input, 4)) return 2;

                return 3;
            }

            if (input.Length == 6) // 0, 6, 9
            {
                // 0 does not have digit 3.
                if (!HasDigits(input, 3)) return 0;

                // Only 9 has digit 2.
                if (HasDigits(input, 2)) return 9;

                // This leaves 6.
                return 6;
            }

            return -1;
        }

        // Find the easy numbers: 1,4,7,8.
        n[1] = sorted.First(x => x.Length == 2);
        n[4] = sorted.First(x => x.Length == 4);
        n[7] = sorted.First(x => x.Length == 3);
        n[8] = sorted.First(x => x.Length == 7);

        // Use 1 & 7 for the first digit.
        digits[0] = n[7].Replace(n[1], "")[0];

        // 3 has 5 segments and contains 1.
        n[3] = sorted.First(x => x.Length == 5 && HasChars(x, n[1]));

        // From 3, remove 7 and 4 to get the bottom digit.
        digits[6] = n[3].Except(n[7]).Except(n[4]).First();

        // Same trick to resolve the middle segment. Remove 7 and the bottom digit.
        digits[3] = n[3].Except(n[7]).Except(digits).First();

        // Reduce 4 to get the 1-digit.
        digits[1] = n[4].Except(n[1]).Except(digits).First();

        // We have all but 1 digit for 5.
        n[5] = sorted.First(x => x.Length == 5 && HasDigits(x, 0, 1, 3, 6));

        // We can also get digit 5 now, we know all others.
        digits[5] = n[5].Except(digits).First();

        // Digit 2 can be resolved now, using 1.
        digits[2] = n[1].Except(digits).First();

        // This leaves only digit 4.
        digits[4] = "abcdefg".Except(digits).First();


        // Calculate the output.
        int output = 0;
        for (int i = 0; i < 4; ++i)
        {
            var num = ResolveNumber(values[values.Length - 1 - i]);
            output += (int)Math.Pow(10, i) * num;
        }

        return output;
    }





}
