using System.Text.RegularExpressions;

namespace Aoc.Problems.Aoc24;

/// <summary>
/// https://adventofcode.com/2024/day/3
/// </summary>
public class Problem03 : IProblem
{
    // In (at least) part 1 we are looking for a precise mul(X,Y) instruction.
    // The lazy (easy) way here is just Regex:
    public static Regex MulPattern = new Regex(@"(mul\(\d+,\d+\))", RegexOptions.Compiled);

    // In part 2 we also process the do() / don't() commands.
    // Extend the pattern.
    public static Regex MulDoDontPattern = new Regex(@"(do\(\))|(don't\(\))|(mul\(\d+,\d+\))", RegexOptions.Compiled);

    public object Solve1(string input)
    {
        var matches = MulPattern.Matches(input);

        int sum = 0;

        foreach (Match match in matches)
        {
            sum += CalculateMul(match);
        }

        return sum;
    }

    public object Solve2(string input)
    {
        // Run a similar Regex match, but this time include the control commands.
        // Regex.Match() returns the matches in the order they occur.
        var matches = MulDoDontPattern.Matches(input);

        // Track the enabled state:
        bool enabled = true;
        int sum = 0;

        foreach (Match match in matches)
        {
            // Add in the do/don't command handling
            if (match.Value == "do()")
            {
                enabled = true;
            }
            else if (match.Value == "don't()")
            {
                enabled = false;
            }
            else
            {
                // Otherwise add to the sum like before.
                if (enabled)
                {
                    sum += CalculateMul(match);
                }
            }
        }

        return sum;
    }

    /// <summary>
    /// Parses a mul(A,B) match and returns the multiplication result.
    /// </summary>
    /// <param name="match"></param>
    /// <returns></returns>
    private static int CalculateMul(Match match)
    {
        // Simple parsing from here on:
        var numbers = match
            // Substring: skip 4 first chars "mul(" and the last char ")"
            .Value[4..^1]
            // Then split with comma
            .Split(',')
            // And parse numbers.
            .Select(int.Parse)
            .ToArray();

        // Then simply multiply.
        return numbers[0] * numbers[1];
    }
}
