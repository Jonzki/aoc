namespace Aoc.Problems.Aoc24;

public class Problem07 : IProblem
{
    public object Solve1(string input)
    {
        long sum = 0;

        foreach (var line in input.SplitLines())
        {
            var output = ProcessEquation(line);
            if (output.PossibleCount > 0)
            {
                sum += output.Result;
            }
        }

        return sum;
    }

    public object Solve2(string input)
    {
        long sum = 0;

        List<long> results = new();

        long previous = 0;

        foreach (var line in input.SplitLines())
        {
            previous = sum;

            var output = ProcessEquation(line, true);
            if (output.PossibleCount > 0)
            {
                results.Add(output.Result);
                sum += output.Result;

                if (sum < previous)
                {
                    throw new InvalidOperationException("Oops, overflow.");
                }
            }
        }
        
        return sum;
    }

    /// <summary>
    /// Processes the input equation and returns the amount of possible configurations to complete the equation.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="withConcat">Allow using the concatenation operator.</param>
    /// <returns></returns>
    public static (long Result, int PossibleCount) ProcessEquation(string input, bool withConcat = false)
    {
        // Grab the output and the input values:
        var parts = input.Split(':');

        long result = long.Parse(parts[0]);
        int[] inputs = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        // Prepare all variations of the inputs.
        var configs = OperationPermutations(inputs.Length - 1, new List<char>(), withConcat);

        var possibleCount = 0;

        foreach (var config in configs)
        {
            long configResult = inputs[0];

            for (var i = 0; i < config.Length; i++)
            {
                if (config[i] == '+')
                {
                    configResult += inputs[i + 1];
                }
                else if (config[i] == '*')
                {
                    configResult *= inputs[i + 1];
                }
                else if (config[i] == '|' && withConcat)
                {
                    // Concatenate numbers.
                    var temp = "" + configResult + inputs[i+1];
                    configResult = long.Parse(temp);
                }
                else
                {
                    throw new InvalidOperationException($"Unexpected operator '{config[i]}' at position {i}");
                }

                if (configResult > result)
                {
                    // Result has already gone over - break out.
                    break;
                }
            }

            if (result == configResult)
            {
                ++possibleCount;
            }
        }

        return (result, possibleCount);
    }

    /// <summary>
    /// Returns all possible permutations of plus/multiply operations of the given length.
    /// </summary>
    /// <param name="length"></param>
    /// <param name="buffer"></param>
    /// <param name="withConcat"></param>
    /// <returns></returns>
    public static List<char[]> OperationPermutations(int length, IEnumerable<char> buffer, bool withConcat = false)
    {
        // If we've reached the full length, return the array.
        if (buffer.Count() == length)
        {
            return new List<char[]> { buffer.ToArray() };
        }

        // Otherwise, return two variants of the buffer:
        var output = new List<char[]>();
        output.AddRange(OperationPermutations(length, buffer.Append('*'), withConcat));
        output.AddRange(OperationPermutations(length, buffer.Append('+'), withConcat));
        if (withConcat)
        {
            output.AddRange(OperationPermutations(length, buffer.Append('|'), withConcat));
        }
        return output;
    }
}
