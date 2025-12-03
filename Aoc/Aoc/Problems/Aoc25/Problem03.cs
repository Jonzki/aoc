namespace Aoc.Problems.Aoc25;

public class Problem03 : IProblem
{
    public object Solve1(string input)
    {
        return input
            .SplitLines()
            .Select(b => FindLargestNumber(b, 2))
            .Sum();
    }

    public object Solve2(string input)
    {
        return input
            .SplitLines()
            .Select(b => FindLargestNumber(b, 12))
            .Sum();
    }

    /// <summary>
    /// Finds the largest possible number within the input of the given length.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static long FindLargestNumber(string input, int length)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException("Need an actual input.");
        }
        if (length > 16)
        {
            throw new ArgumentOutOfRangeException("Max supported length is 16.");
        }
        if (input.Length < length)
        {
            throw new ArgumentException($"Cannot find a {length} digit number from a {input.Length} digit input.");
        }

        long outputNumber = 0;

        int start = 0;

        for (var n = length; n > 0; --n)
        {
            char largest = input[start];
            int largestPosition = start;

            // Scan for the largest available number while keeping space at the end.
            for (var i = start + 1; i < input.Length - n + 1; ++i)
            {
                if (input[i] > largest)
                {
                    largestPosition = i;
                    largest = input[i];
                }
            }

            // Append the number we found. Shift existing value first.
            if (outputNumber != 0)
            {
                outputNumber *= 10;
            }
            outputNumber += largest.NumberValue();

            // Set the next start position to the next available.
            start = largestPosition + 1;
        }

        return outputNumber;
    }
}
