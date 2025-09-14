namespace Aoc.Problems.Aoc20;

public class Problem2 : IProblem
{
    public object Solve1(string input)
    {
        var validPasswords = input
            .Split(Environment.NewLine)
            .Select(ParsePasswordRow)
            .Count(IsPasswordValid1);

        return validPasswords;
    }

    public object Solve2(string input)
    {
        var validPasswords = input
            .Split(Environment.NewLine)
            .Select(ParsePasswordRow)
            .Count(IsPasswordValid2);

        return validPasswords;
    }

    /// <summary>
    /// Checks if the input Password is valid, variant 1.
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    public static bool IsPasswordValid1(PasswordRow row)
    {
        var count = 0;
        for (var i = 0; i < row.Password.Length; ++i)
        {
            if (row.Password[i] == row.RequiredChar)
            {
                ++count;
            }
            if (count > row.MaxCharCount) break;
        }
        return row.MinCharCount <= count && count <= row.MaxCharCount;
    }

    /// <summary>
    /// Checks if the input Password is valid, variant 2.
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    public static bool IsPasswordValid2(PasswordRow row)
    {
        var occurs = 0;
        if (row.Password[row.MinCharCount - 1] == row.RequiredChar) ++occurs;
        if (row.Password[row.MaxCharCount - 1] == row.RequiredChar) ++occurs;
        return occurs == 1;
    }

    /// <summary>
    /// Parses input line into a password row.
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public static PasswordRow ParsePasswordRow(string line)
    {
        // Split by all kinds of separators.
        var parts = line.Split(['-', ' ', ':'], StringSplitOptions.RemoveEmptyEntries);
        return new PasswordRow
        {
            MinCharCount = int.Parse(parts[0]),
            MaxCharCount = int.Parse(parts[1]),
            RequiredChar = parts[2][0],
            Password = parts[3]
        };
    }

    public class PasswordRow
    {
        public char RequiredChar { get; init; }
        public int MinCharCount { get; init; }
        public int MaxCharCount { get; init; }

        public required string Password { get; init; }
    }
}
