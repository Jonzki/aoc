namespace Aoc.Problems.Aoc25;

public class Problem04 : IProblem
{
    public object Solve1(string input)
    {
        // Part 1: start by finding all paper roll positions.
        var map = ParseMap(input);

        // From here, find accessible ones.
        return FindAccessibleRolls(map).Count;
    }

    public object Solve2(string input)
    {
        var map = ParseMap(input);

        int removeCount = 0;

        // Run until we have no more points to remove.
        while (true)
        {
            List<Point2D> rollsToRemove = FindAccessibleRolls(map);

            // Update the remove count and actually remove the rolls.
            removeCount += rollsToRemove.Count;
            foreach (var roll in rollsToRemove)
            {
                map.Data[roll.X, roll.Y] = '.';
            }

            if (rollsToRemove.Count == 0)
            {
                break;
            }
        }

        return removeCount;
    }

    public static List<Point2D> FindAccessibleRolls((int Width, int Height, char[,] Data) map)
    {
        List<Point2D> accessible = new();

        for (var y = 0; y < map.Height; ++y)
        {
            for (var x = 0; x < map.Width; ++x)
            {
                if (map.Data[x, y] != '@')
                {
                    continue;
                }

                var roll = new Point2D(x, y);

                var surroundingRolls = roll
                    // Find surrounding positions that:
                    .GetSurroundingPoints(includeDiagonals: true)
                    .Count(p =>
                        // are within the map
                        p.IsInBounds(map.Width, map.Height)
                        // and that contain a roll.
                        && map.Data[p.X, p.Y] == '@'
                    );

                // If there are less than 4 surrounding rolls, we can remove this one.
                if (surroundingRolls < 4)
                {
                    accessible.Add(roll);
                }
            }
        }

        return accessible;
    }

    /// <summary>
    /// Parses the input into a 2D map.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static (int Width, int Height, char[,] Data) ParseMap(string input)
    {
        var lines = input.SplitLines();
        var height = lines.Length;

        var lineLengths = lines.Select(l => l.Length).Distinct().ToArray();
        if (lineLengths.Length != 1)
        {
            throw new ArgumentException("Inconsistent line lengths - cannot parse a 2D map.");
        }

        var width = lineLengths[0];

        var map = new char[width, height];

        for (var y = 0; y < height; ++y)
        {
            for (var x = 0; x < width; ++x)
            {
                map[x, y] = lines[y][x];
            }
        }

        return (width, height, map);
    }
}
