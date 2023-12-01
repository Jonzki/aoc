namespace Aoc.Problems.Aoc18;

public class Problem01 : IProblem
{
    public object Solve1(string input)
    {
        // Remove the plus sign to allow int.Parse, then simply sum all the rows together.
        return ParseNumbers(input).Sum();
    }

    public object Solve2(string input)
    {
        var numbers = ParseNumbers(input);

        // Add the initial frequency of zero.
        var visited = new HashSet<int> { 0 };

        var resultingFrequency = 0;
        var run = true;
        while (run)
        {
            for (var i = 0; run && i < numbers.Count; ++i)
            {
                resultingFrequency += numbers[i];

                if (visited.Contains(resultingFrequency))
                {
                    return resultingFrequency;
                }

                visited.Add(resultingFrequency);
            }
        }
        return resultingFrequency;
    }

    private List<int> ParseNumbers(string input) => input
        .RemoveString("+")
        .Split(',', '\n')
        .Select(int.Parse)
        .ToList();
}
