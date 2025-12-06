namespace Aoc.Problems.Aoc25;

public class Problem06 : IProblem
{
    public object Solve1(string input)
    {
        var problems = ParseProblems1(input);

        return problems.Sum(p => p.Calculate());
    }

    public object Solve2(string input)
    {
        var problems = ParseProblems2(input);

        return problems.Sum(p => p.Calculate());
    }

    public static List<Problem> ParseProblems1(string input)
    {
        // Input is a two-dimensional array.
        // Parse as such.
        var lines = input
            .SplitLines()
            .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .ToArray();

        // Sanity check: all lines should have the same number of items.
        for (var i = 1; i < lines.Length; ++i)
        {
            if (lines[i].Length != lines[i - 1].Length)
            {
                throw new ArgumentException("Parsing error: lines don't have the same amount of inputs.");
            }
        }

        // From here we can parse the problems.
        var problemCount = lines[0].Length;
        var problems = new List<Problem>();

        for (var p = 0; p < problemCount; ++p)
        {
            var problem = new Problem();

            for (var l = 0; l < lines.Length - 1; ++l)
            {
                problem.Numbers.Add(long.Parse(lines[l][p]));
            }

            // Final line is the operand. The item should have a length of 1, or we have a problem.
            var operandStr = lines.Last()[p];
            if (operandStr.Length != 1)
            {
                throw new ArgumentException($"Invalid operand '{operandStr}'");
            }

            problem.Operand = operandStr[0];

            problems.Add(problem);
        }

        return problems;
    }

    public static List<Problem> ParseProblems2(string input)
    {
        // Read the input as a table of characters - the spaces end up mattering a lot here.
        var height = input.Count(c => c == '\n') + 1;

        var chars = input
            .Replace('\r', '\n')
            .RemoveAll('\n')
            .ToCharArray();
        if (chars.Length % height != 0)
        {
            throw new ArgumentException("Input string is not a proper 2D array.");
        }

        var width = chars.Length / height;
        var charMap = chars.To2D(width, height);

        // We will transpose the input into a new string.
        var sb = new StringBuilder();

        // Parse from the right.
        for (var x = width - 1; x >= 0; --x)
        {
            // Read the numbers from top down.
            for (var y = 0; y < height; ++y)
            {
                var c = charMap[y, x];
                sb.Append(c);
            }

            sb.AppendLine();
        }

        var transposedInput = sb.ToString();

        // From here we can parse the problems quite easily.
        var lines = transposedInput.SplitLines();
        var problems = new List<Problem>();
        var currentProblem = new Problem();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            // Check if the line contains an operand.
            if (line.EndsWith('*') || line.EndsWith('+'))
            {
                // Push the operand into the operand slot,
                // And parse the number before it.
                currentProblem.Operand = line.Last();
                currentProblem.Numbers.Add(long.Parse(line.Substring(0, line.Length - 1)));

                problems.Add(currentProblem);
                currentProblem = new();
            }
            else
            {
                // Otherwise the line should just be a number.
                currentProblem.Numbers.Add(long.Parse(line));
            }
        }

        return problems;
    }

    public class Problem
    {
        public List<long> Numbers { get; set; } = new();

        public char Operand { get; set; }

        /// <summary>
        /// Calculates the result of this Problem for Part 1.
        /// </summary>
        /// <returns></returns>
        public long Calculate()
        {
            if (Operand == '+')
            {
                return Numbers.Sum();
            }
            if (Operand == '*')
            {
                return Numbers.Aggregate(1L, (current, total) => current * total);
            }
            throw new NotImplementedException($"Unsupported operand '{Operand}'.");
        }
    }
}
