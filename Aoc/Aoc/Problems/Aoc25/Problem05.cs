namespace Aoc.Problems.Aoc25;

public class Problem05 : IProblem
{
    public object Solve1(string input)
    {
        var (ranges, ingredients) = ParseInput(input);

        // In part 1, an ingredient is fresh if it is in any range.
        var freshCount = 0;

        foreach (var i in ingredients)
        {
            if (CountInRange(i, ranges) > 0)
            {
                ++freshCount;
            }
        }

        return freshCount;
    }

    public object Solve2(string input)
    {
        var (ranges, _) = ParseInput(input);

        // In part 2 we want to count all possible IDs that would be fresh based on the input.

        // Pool the ID range points together.
        var rangePoints = new List<(long Position, bool IsStart)>();

        foreach (var r in ranges)
        {
            rangePoints.Add((r.Start, true));
            rangePoints.Add((r.End, false));
        }

        // Sort the range points.
        rangePoints = rangePoints
            .OrderBy(x => x.Position)
            .ThenByDescending(r => r.IsStart ? 1 : 0)
            .ToList();

        // First item must always be a Start position.
        if (!rangePoints.First().IsStart)
        {
            throw new InvalidOperationException("First RangePoint is not a start point??");
        }

        int level = 0;
        long startPoint = -1;
        long sum = 0;

        // Now we can "travel" the range points. Track the depth level so that we know where to update the total.
        foreach (var r in rangePoints)
        {
            if (r.IsStart)
            {
                // Mark the starting point if we are at zero level.
                if (level == 0)
                {
                    startPoint = r.Position;
                }
                ++level;
            }
            else // End point
            {
                --level;

                // If we dropped back to zero, update the sum.
                if (level == 0)
                {
                    sum += (r.Position - startPoint) + 1;
                }
            }
        }

        return sum;
    }

    public static (List<IdRange> Ranges, List<long> Ingredients) ParseInput(string input)
    {
        var ranges = new List<IdRange>();
        var ingredients = new List<long>();

        foreach (var line in input.SplitLines())
        {
            if (line.Contains('-'))
            {
                // Parse line as a Range.
                var s = line.Split('-');
                if (s.Length != 2)
                {
                    throw new ArgumentException($"Line '{line}' is not a valid ID range.");
                }

                ranges.Add(new IdRange(long.Parse(s[0]), long.Parse(s[1])));
            }
            else if (long.TryParse(line, out var temp))
            {
                ingredients.Add(temp);
            }
        }

        // We do a tactical sorting here for part 2.
        return (ranges.OrderBy(r => r.Start).ToList(), ingredients);
    }

    public static int CountInRange(long input, List<IdRange> ranges)
    {
        return ranges.Count(r => r.Start <= input && input <= r.End);
    }

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct IdRange(long start, long end)
    {
        public long Start = start;
        public long End = end;

        private string DebuggerDisplay => Start + "-" + End;
    };
}
