using System.Collections;

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
            output += temp;
        }

        return output;
    }

    public static int ParseNumber(string input)
    {
        var values = input.Split(new[] { "|", " " }, StringSplitOptions.RemoveEmptyEntries);
        var sorted = values.Select(x => new string(x.ToCharArray().OrderBy(c => c).ToArray())).ToArray();

        var n = new string[10];

        //  00
        // 1  2
        // 1  2
        //  33
        // 4  5
        // 4  5
        //  66
        var digits = new char[7] { '_', '_', '_', '_', '_', '_', '_' };

        int ResolveNumber(string input, char[] codec)
        {
            // Calculate a bitmask of sorts.
            var bitArray = new BitArray(7);
            for (var i = 0; i < 7; i++)
            {
                bitArray[i] = input.Contains(codec[i]);
            }

            return Array.IndexOf(BitArrayLookup, bitArray.ToInteger());
        }

        // Process the entire input with each permutation, look for one that produces valid numbers.
        foreach (var perm in GetPermutations())
        {
            var numbers = values.Select(x => ResolveNumber(x, perm)).ToArray();
            // Check if permutation is valid.
            if (numbers.Contains(-1)) continue;

            // Permutation was valid - build the output number.
            var output = 0;
            for (var i = 0; i < 4; ++i)
            {
                output += numbers[numbers.Length - 1 - i] * (int)Math.Pow(10, i);
            }
            return output;
        }

        return -1;
    }

    // Construct a bitArray lookup for each digit.
    private static int[] BitArrayLookup { get; } = new[]
        {
            "1110111",
            "0010010",
            "1011101",
            "1011011",
            "0111010",
            "1101011",
            "1101111",
            "1010010",
            "1111111",
            "1111011"
        }.Select(x => BitArrayUtils.Parse(x).ToInteger()).ToArray();

    private static List<char[]>? _permutations = null;

    private static List<char[]> GetPermutations()
    {
        if (_permutations == null)
        {
            _permutations = BuildPermutations(Array.Empty<char>(), "abcdefg".ToCharArray());
        }
        return _permutations;
    }

    /// <summary>
    /// Returns all 7-digit permutations possible.
    /// </summary>
    /// <param name="current"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    private static List<char[]> BuildPermutations(IEnumerable<char> current, IEnumerable<char> input)
    {
        var perms = new List<char[]>();
        if (!input.Any()) { return new List<char[]> { current.ToArray() }; }
        foreach (var c in "abcdefg")
        {
            if (current.Contains(c)) continue;
            perms.AddRange(BuildPermutations(current.Append(c), input.Except([c])));
        }
        return perms;
    }
}
