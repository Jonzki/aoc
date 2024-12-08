namespace Aoc.Problems.Aoc24;

/// <summary>
/// https://adventofcode.com/2024/day/8
/// </summary>
public class Problem08 : IProblem
{
    public object Solve1(string input)
    {
        var map = ParseMap(input);

        // Calculate all pairs.
        var pairs = BuildPairs(map.Points);

        var pointLookup = map.Points.ToDictionary(p => p.Id);

        var signalLocations = new HashSet<Point2D>();

        foreach (var pair in pairs)
        {
            var pointA = pointLookup[pair.A];
            var pointB = pointLookup[pair.B];

            var posA = pointA.Position;
            var posB = pointB.Position;

            // Calculate signal positions as vectors between A and B.
            // Two directions per pair.
            signalLocations.Add(posB + (posB - posA));
            signalLocations.Add(posA + (posA - posB));
        }

        //Draw(map.Points, map.Width, map.Height, signalLocations);

        return signalLocations.Count(p => p.IsInBounds(map.Width, map.Height));
    }

    public object Solve2(string input)
    {
        var map = ParseMap(input);

        // Calculate all pairs.
        var pairs = BuildPairs(map.Points);

        var pointLookup = map.Points.ToDictionary(p => p.Id);

        var signalLocations = new HashSet<Point2D>();

        // In part 2, we calculate a line of points in each direction
        // (until we go out of bounds).
        foreach (var pair in pairs)
        {
            var pointA = pointLookup[pair.A];
            var pointB = pointLookup[pair.B];

            var posA = pointA.Position;
            var posB = pointB.Position;

            // The two starting positions themselves can be added as is.
            signalLocations.Add(posA);
            signalLocations.Add(posB);

            // Run in either direction.
            // Our map dimensions works as a nice upper bound here.
            Point2D temp = posA;
            for (var i = 0; i < Math.Max(map.Width, map.Height); ++i)
            {
                temp += (posB - posA);
                if (temp.IsInBounds(map.Width, map.Height))
                {
                    signalLocations.Add(temp);
                }
                else
                {
                    break;
                }
            }

            temp = posB;
            for (var i = 0; i < Math.Max(map.Width, map.Height); ++i)
            {
                temp += (posA - posB);
                if (temp.IsInBounds(map.Width, map.Height))
                {
                    signalLocations.Add(temp);
                }
                else
                {
                    break;
                }
            }
        }

        //Draw(map.Points, map.Width, map.Height, signalLocations);

        return signalLocations.Count(p => p.IsInBounds(map.Width, map.Height));
    }

    private (int Width, int Height, List<MapPoint> Points) ParseMap(string input)
    {
        var lines = input.SplitLines();
        var height = lines.Length;
        var width = lines[0].Length;

        var mapPoints = new List<MapPoint>();

        // Start ID from 1 to leave room for default(int).
        int id = 1;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (lines[y][x] != '.')
                {
                    mapPoints.Add(new MapPoint()
                    {
                        Id = id++,
                        Char = lines[y][x],
                        Position = new Point2D(x, y)
                    });
                }
            }
        }

        return (width, height, mapPoints);
    }

    public struct MapPoint
    {
        public int Id { get; init; }

        public char Char { get; init; }

        public Point2D Position { get; init; }
    }

    /// <summary>
    /// Draws the map for visual inspection / diagnostics.
    /// </summary>
    /// <param name="points"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="signalLocations"></param>
    private void Draw(List<MapPoint> points, int width, int height, HashSet<Point2D> signalLocations)
    {
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var point = points.FirstOrDefault(p => p.Position.Equals(x, y));
                if (point.Id > 0)
                {
                    Console.Write(point.Char);
                }
                else if (signalLocations.Contains(new Point2D(x, y)))
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Builds all possible pairs from the input MapPoints.
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    public static List<(int A, int B)> BuildPairs(List<MapPoint> points)
    {
        var pairs = new List<(int A, int B)>();

        // Process each group (character) separately.
        foreach (var group in points.GroupBy(p => p.Char))
        {
            // Build pairs within each group.
            var count = group.Count();
            var groupPoints = group.ToList();
            for (var i = 0; i < count; ++i)
            {
                for (var j = 0; j < i; ++j)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    pairs.Add((groupPoints[i].Id, groupPoints[j].Id));
                }
            }
        }

        return pairs;
    }
}
