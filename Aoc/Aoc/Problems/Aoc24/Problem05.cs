namespace Aoc.Problems.Aoc24;

/// <summary>
/// https://adventofcode.com/2024/day/5
/// </summary>
public class Problem05 : IProblem
{
    public object Solve1(string input)
    {
        var (rules, inputs) = ParseInput(input);

        // start by identifying which updates are already in the right order.
        var correctlyOrdered = inputs.Where(i => IsCorrectlyOrdered(i, rules));

        // From these, sum together the middle values.
        var sum = 0;
        foreach (var list in correctlyOrdered)
        {
            sum += list[list.Length / 2];
        }

        return sum;
    }

    public object Solve2(string input)
    {
        var (rules, inputs) = ParseInput(input);

        // In part 2, pick only the incorrectly ordered lists to process further.
        var incorrectlyOrdered = inputs.Where(i => !IsCorrectlyOrdered(i, rules));

        var sum = 0;

        foreach (var list in incorrectlyOrdered)
        {
            // Reorder each list and add the middle value to the sum.
            // The list length remains the same so we can use list.Length here.
            sum += Reorder(list, rules)[list.Length / 2];
        }

        return sum;
    }

    /// <summary>
    /// Returns true if the input array is sorted correctly according to the input rules.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="rules"></param>
    /// <returns></returns>
    public static bool IsCorrectlyOrdered(int[] input, List<(int A, int B)> rules)
    {
        // Check each Rule against the input.
        foreach (var rule in rules)
        {
            // A should appear before B.
            var indexA = input.IndexOf(rule.A);
            if (indexA == -1) continue;

            var indexB = input.IndexOf(rule.B);
            if (indexB == -1) continue;

            if (indexB < indexA)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Returns a version of the input array that is sorted correctly according to the input rules.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="rules"></param>
    /// <returns></returns>
    public static int[] Reorder(int[] input, List<(int A, int B)> rules)
    {
        // Create a Comparer.
        var comparer = new RuleComparer(rules);

        // Use it to sort the list.
        return input.OrderBy(x => x, comparer).ToArray();
    }

    /// <summary>
    /// Parses the raw input into a set of sorting rules, and the inputs themselves.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static (List<(int A, int B)> SortRules, List<int[]> Inputs) ParseInput(string input)
    {
        // Split with a double linebreak first.
        var parts = input.Split("\r\n\r\n");

        // Parse the sorting rules.
        var rules = new List<(int A, int B)>();
        foreach (var line in parts[0].SplitLines())
        {
            var numbers = line.Split('|').Select(int.Parse).ToArray();
            rules.Add((numbers[0], numbers[1]));
        }

        // Parse the inputs.
        var inputs = new List<int[]>();
        foreach (var line in parts[1].SplitLines())
        {
            inputs.Add(line.Split(',').Select(int.Parse).ToArray());
        }

        return (rules, inputs);
    }

    private class RuleComparer : IComparer<int>
    {
        private readonly List<(int A, int B)> _rules;

        public RuleComparer(List<(int A, int B)> rules)
        {
            _rules = rules;
        }

        public int Compare(int x, int y)
        {
            // Check each rule for a match.
            if (_rules.Any(r => r.A == x && r.B == y))
            {
                // x should be first.
                return -1;
            }

            if (_rules.Any(r => r.A == y && r.B == x))
            {
                // y should be first.
                return 1;
            }

            // No rule for this pair - ignore.
            return 0;
        }
    }
}
