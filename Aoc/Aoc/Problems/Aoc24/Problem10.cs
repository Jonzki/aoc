namespace Aoc.Problems.Aoc24;

/// <summary>
/// https://adventofcode.com/2024/day/10
/// </summary>
public class Problem10 : IProblem
{
    private const int Trailhead = 0;
    private const int Trailpeak = 9;

    private static readonly Point2D[] Directions =
    [
        Point2D.Zero.Up(),
        Point2D.Zero.Down(),
        Point2D.Zero.Left(),
        Point2D.Zero.Right()
    ];

    public object Solve1(string input)
    {
        var map = ParseMap(input);

        // Find all trailheads (value 0)
        var trailheads = FindAll(map, Trailhead);

        // And peaks (value 9)
        var peaks = FindAll(map, Trailpeak);

        // Count the scores for each trailhead
        // (how many peaks can be reached from each trailhead).
        int sum = 0;
        foreach (var trailhead in trailheads)
        {
            int score = 0;
            foreach (var peak in peaks)
            {
                if (CanReach(map, trailhead, peak))
                {
                    ++score;
                }
            }

            sum += score;
        }

        return sum;
    }

    public object Solve2(string input)
    {
        var map = ParseMap(input);

        // Find all trailheads (value 0)
        var trailheads = FindAll(map, Trailhead);
        var peaks = FindAll(map, Trailpeak);

        // Count the rating for each trailhead.
        // (how many peaks can be reached from each trailhead).
        int sum = 0;
        foreach (var trailhead in trailheads)
        {
            int score = 0;
            foreach (var peak in peaks)
            {
                var startPath = new Path(trailhead);

                // Count the amount of possible paths.
                var paths = FindPaths(map, peak, startPath);
                score += paths.Count;
            }

            sum += score;
        }

        return sum;
    }

    public static int[,] ParseMap(string input)
    {
        var lines = input.SplitLines();
        var map = new int[lines[0].Length, lines.Length];

        for (var y = 0; y < lines.Length; ++y)
        {
            for (var x = 0; x < lines[0].Length; ++x)
            {
                if (lines[y][x] == '.')
                {
                    map.Set(x, y, -1);
                }
                else
                {
                    map.Set(x, y, lines[y][x] - '0');
                }
            }
        }

        return map;
    }

    public List<Point2D> FindAll(int[,] map, int value)
    {
        var points = new List<Point2D>();

        ArrayUtils.Iterate(map, (pos, mapValue) =>
        {
            if (mapValue == value)
            {
                points.Add(pos);
            }
        });

        return points;
    }

    /// <summary>
    /// Returns true if there is a trail between the given points.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static bool CanReach(int[,] map, Point2D start, Point2D end)
    {
        // Recursion end condition.
        if (start.PositionEquals(end))
        {
            return true;
        }

        // We can navigate in 4 directions.
        foreach (var dir in Directions)
        {
            var target = start + dir;

            if (!IsValidStep(map, start, target))
            {
                continue;
            }

            if (CanReach(map, target, end))
            {
                return true;
            }
        }
        return false;
    }

    public static List<Path> FindPaths(int[,] map, Point2D target, Path currentPath)
    {
        // Recursion end condition.
        if (currentPath.Position.Equals(target))
        {
            return [currentPath];
        }

        // We can navigate in 4 directions.
        // Try each and return the possible paths.
        var resultPaths = new List<Path>();
        foreach (var dir in Directions)
        {
            var nextTarget = currentPath.Position + dir;
            if (IsValidStep(map, currentPath.Position, nextTarget))
            {
                // The step is valid, try navigating that way.
                var path = new Path([.. currentPath.Points, nextTarget]);
                resultPaths.AddRange(FindPaths(map, target, path));
            }
        }
        return resultPaths;
    }

    public static bool IsValidStep(int[,] map, Point2D start, Point2D end)
    {
        // Check if start & target are in bounds.
        if (!map.TryGet(start, out var startValue) || !map.TryGet(end, out var endValue))
        {
            return false;
        }

        // Check if the target step is reachable (difference of 1).
        if (startValue + 1 != endValue)
        {
            return false;
        }

        return true;
    }

    public class Path
    {
        public Path(List<Point2D> points)
        {
            Points = points;
        }

        public Path(Point2D point) : this([point])
        {
        }

        public List<Point2D> Points { get; }

        public Point2D Position => Points.Last();
    }
}
