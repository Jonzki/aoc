namespace Aoc.Problems.Aoc24;

/// <summary>
/// https://adventofcode.com/2024/day/2
/// </summary>
public class Problem02 : IProblem
{
    public object Solve1(string input)
    {
        // Split the input into lines (reports),
        // Then parse each number in the line.
        var reports = input
            .SplitLines()
            .Select(line => line
                .Split(' ')
                .Select(int.Parse)
                .ToArray()
            )
            .ToArray();

        return reports.Count(IsSafe1);
    }

    public object Solve2(string input)
    {
        // Split the input into lines (reports),
        // Then parse each number in the line.
        var reports = input
            .SplitLines()
            .Select(line => line
                .Split(' ')
                .Select(int.Parse)
                .ToArray()
            )
            .ToArray();

        // In part 2, we allow a single issue for each report.
        return reports.Count(IsSafe2);
    }

    /// <summary>
    /// Checks if the input array is "safe"
    /// </summary>
    /// <param name="report"></param>
    /// <returns></returns>
    public static bool IsSafe1(int[] report)
    {
        if (report.Length < 2)
        {
            // 0 or 1 item arrays are safe, we assume.
            return true;
        }

        // Report counts as safe if:
        // The levels are all increasing or all decreasing.
        bool isAscending = report[0] < report[1];

        // Any two adjacent levels differ by at least one and at most three.
        const int minDiff = 1, maxDiff = 3;

        for (var i = 1; i < report.Length; ++i)
        {
            var diff = report[i] - report[i - 1];

            if (isAscending && diff < 0)
            {
                // Not safe, ascending mode and got a decrease.
                return false;
            }
            if (!isAscending && diff > 0)
            {
                // Not safe, descending mode and got an increase.
                return false;
            }

            if (!Math.Abs(diff).BetweenInclusive(minDiff, maxDiff))
            {
                // Not safe
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Tests if input array is "safe".
    /// Part 2: also test by removing a single item.
    /// </summary>
    /// <param name="report"></param>
    /// <returns></returns>
    public static bool IsSafe2(int[] report)
    {
        // Test with the whole array first.
        if (IsSafe1(report))
        {
            return true;
        }

        for (var removeIndex = 0; removeIndex < report.Length; ++removeIndex)
        {
            var removed = RemoveAt(report, removeIndex);
            if (IsSafe1(removed))
            {
                return true;
            }
        }

        // None of the variants succeed.
        return false;
    }

    /// <summary>
    /// Returns a version of the report with the given index removed.
    /// </summary>
    /// <param name="report"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static int[] RemoveAt(int[] report, int index)
    {
        int[] output = new int[report.Length - 1];
        var oi = 0;
        for (var i = 0; i < report.Length; ++i)
        {
            if (i != index)
            {
                output[oi++] = report[i];
            }
        }
        return output;
    }
}
