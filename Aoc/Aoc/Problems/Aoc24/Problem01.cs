namespace Aoc.Problems.Aoc24;

/// <summary>
/// https://adventofcode.com/2024/day/1
/// </summary>
public class Problem01 : IProblem
{
    //public string? ReadInput() => InputReader.ReadInput(2024, 1, "small");

    public object Solve1(string input)
    {
        // "Two lists side by side"
        var (first, second) = ParseLists(input);

        // Pair up the smallest number in each list = sort lists.
        first.Sort();
        second.Sort();

        int totalDiff = 0;
        for (var i = 0; i < first.Count; ++i)
        {
            // Within each pair, figure out how far apart the two numbers are;
            // you'll need to add up all of those distances.
            totalDiff += Math.Abs(first[i] - second[i]);
        }

        return totalDiff;
    }

    public object Solve2(string input)
    {
        // "Two lists side by side"
        var (first, second) = ParseLists(input);

        // This time we want to multiply the counts on both sides. Form a dictionary of both.
        var counts = new Dictionary<int, (int Left, int Right)>();

        foreach (var x in first)
        {
            // Make sure the dictionary has a key.
            counts.TryAdd(x, (0, 0));

            // Then bump up the count.
            counts[x] = (counts[x].Left + 1, counts[x].Right);
        }

        foreach (var x in second)
        {
            // Make sure the dictionary has a key.
            counts.TryAdd(x, (0, 0));

            // Then bump up the count.
            counts[x] = (counts[x].Left, counts[x].Right + 1);
        }

        // Finally, simply sum all these together while multiplying.
        long sum = 0;
        foreach (var kvp in counts)
        {
            // The second number in the left list is 4.
            // It appears in the right list once,
            // so the similarity score increases by 4 * 1 = 4.
            // This is done for each left-side key = multiply by left side count.
            sum += (kvp.Key * kvp.Value.Left * kvp.Value.Right);
        }
        return sum;
    }

    /// <summary>
    /// Parses the input into two separate lists.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private (List<int> First, List<int> Second) ParseLists(string input)
    {
        List<int> first = [], second = [];

        foreach (var line in input.SplitLines())
        {
            var numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            first.Add(numbers[0]);
            second.Add(numbers[1]);
        }

        return (first, second);
    }
}
