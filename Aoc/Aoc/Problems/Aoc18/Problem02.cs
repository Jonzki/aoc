using System.Text;

namespace Aoc.Problems.Aoc18;

public class Problem02 : IProblem
{
    public object Solve1(string input)
    {
        int twos = 0, threes = 0;

        foreach (var line in input.SplitLines())
        {
            // Count each letter.
            var groups = line
                .GroupBy(c => c, c => 1);

            bool hasTwo = false, hasThree = false;
            foreach (var group in groups)
            {
                if (group.Count() == 2)
                {
                    hasTwo = true;
                }
                if (group.Count() == 3)
                {
                    hasThree = true;
                }
            }
            if (hasTwo) twos++;
            if (hasThree) threes++;
        }

        return twos * threes;
    }

    public object Solve2(string input)
    {
        var lines = input.SplitLines();

        for (var i = 0; i < lines.Length; ++i)
        {
            for (var j = 0; j < i; ++j)
            {
                var diff = CalculateDifference(lines[i], lines[j]);
                if (diff == 1)
                {
                    // Construct the return value.
                    return GetResultString(lines[i], lines[j]);
                }
            }
        }
        return "ERROR";
    }

    private int CalculateDifference(string boxA, string boxB)
    {
        // Count each letter of each input.
        var diff = 0;
        for (var i = 0; i < boxA.Length; ++i)
        {
            if (boxA[i] != boxB[i]) ++diff;
        }
        return diff;
    }

    private string GetResultString(string a, string b)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < a.Length; ++i)
        {
            if (a[i] == b[i])
            {
                sb.Append(a[i]);
            }
        }
        return sb.ToString();
    }

}
