namespace Aoc.Problems.Aoc25;

public class Problem02 : IProblem
{
    public object Solve1(string input)
    {
        var ranges = ParseRanges(input);

        var invalidIds = ranges.SelectMany(r => r.FindInvalidIds(mode: 1));

        return invalidIds.Sum();
    }

    public object Solve2(string input)
    {
        var ranges = ParseRanges(input);

        var invalidIds = ranges.SelectMany(r => r.FindInvalidIds(mode: 2));

        return invalidIds.Sum();
    }

    internal static List<IdRange> ParseRanges(string input)
    {
        var ranges = input.Split(',').Select(IdRange.Parse).ToList();

        return ranges;
    }

    internal static bool IsInvalidId(long id, int mode = 1)
    {
        // Always parse the output.
        var str = id.ToString();

        // Pick the correct sequence comparer.
        Func<string, string, bool> sequenceCompare = mode switch
        {
            1 => SequenceCompare1,
            2 => SequenceCompare2,
            _ => throw new ArgumentOutOfRangeException($"Unsupported mode: {mode}.")
        };

        // Check if the ID consists of repeating sequences.
        for (int l = 1; l <= str.Length / 2; ++l)
        {
            // If the ID length is not evenly divisible by sequence length, it cannot be a repeating sequence.
            if (str.Length % l != 0)
            {
                continue;
            }

            // Grab a substring from the start and repeat this however many times our input is.
            var seq = str.Substring(0, l);

            if (sequenceCompare(str, seq))
            {
                return true;
            }
        }
        return false;
    }

    // Part 1: check if the ID contains the test sequence exactly twice.
    internal static bool SequenceCompare1(string idStr, string seq)
    {
        return idStr == seq + seq;
    }

    // Part 2: check if the ID contains the test sequence AT LEAST two times.
    internal static bool SequenceCompare2(string idStr, string seq)
    {
        // ID length must be at least the length of two test sequences.
        if (idStr.Length < seq.Length * 2)
        {
            return false;
        }

        // Removing all test sequences must result in an empty string.
        return idStr.Replace(seq, string.Empty).Length == 0;
    }

    public class IdRange(long start, long end)
    {
        public long Start { get; init; } = start;
        public long End { get; init; } = end;

        public static IdRange Parse(string input)
        {
            var split = input.Split('-');
            if (split.Length != 2)
            {
                throw new ArgumentException($"Invalid range: '{input}'");
            }

            if (!long.TryParse(split[0], out var start))
            {
                throw new ArgumentException($"Invalid start value: '{split[0]}'");
            }
            if (!long.TryParse(split[1], out var end))
            {
                throw new ArgumentException($"Invalid end value: '{split[1]}'");
            }

            return new IdRange(start, end);
        }

        /// <summary>
        /// Finds invalid IDs in the range.
        /// </summary>
        /// <param name="mode">Controls whether the comparison is made with logic for part 1 or 2.</param>
        /// <returns></returns>
        public List<long> FindInvalidIds(int mode = 1)
        {
            List<long> output = new();

            for (var id = Start; id <= End; ++id)
            {
                if (IsInvalidId(id, mode))
                {
                    output.Add(id);
                }
            }

            return output;
        }
    }
}
