namespace Aoc.Problems.Aoc23;

public class Problem01 : IProblem
{
    public object Solve1(string input)
    {
        return input.SplitLines().Select(LineValue1).Sum();
    }

    public object Solve2(string input)
    {
        return input.SplitLines().Select(LineValue2).Sum();
    }

    internal static int LineValue1(string line)
    {
        var firstIndex = line.IndexOfAny(CharUtils.Digits);
        var lastIndex = line.LastIndexOfAny(CharUtils.Digits);

        if (firstIndex == -1)
        {
            return 0;
        }
        return (line[firstIndex] - '0') * 10 + (line[lastIndex] - '0');
    }

    /// <summary>
    /// Looks for the first and last occurrence of a digit or a "number word",
    /// then joins occurrences together for an output value.
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    internal static int LineValue2(string line)
    {
        var searchValues = new (char c, string word)[] {
            ('1', "one"),
            ('2', "two"),
            ('3', "three"),
            ('4', "four"),
            ('5', "five"),
            ('6', "six"),
            ('7', "seven"),
            ('8', "eight"),
            ('9', "nine")
        };

        int firstIndex = 1000,
            firstValue = 0,
            lastIndex = -1000,
            lastValue = 0;

        foreach (var (c, w) in searchValues)
        {
            var ci = line.IndexOf(c);
            var wi = line.IndexOf(w);

            if (ci >= 0 && ci < firstIndex)
            {
                firstIndex = ci;
                firstValue = c - '0';
            }
            if (wi >= 0 && wi < firstIndex)
            {
                firstIndex = wi;
                firstValue = c - '0';
            }

            ci = line.LastIndexOf(c);
            wi = line.LastIndexOf(w);

            if (ci >= 0 && ci > lastIndex)
            {
                lastIndex = ci;
                lastValue = c - '0';
            }
            if (wi >= 0 && wi > lastIndex)
            {
                lastIndex = wi;
                lastValue = c - '0';
            }
        }

        return firstValue * 10 + lastValue;
    }
}
