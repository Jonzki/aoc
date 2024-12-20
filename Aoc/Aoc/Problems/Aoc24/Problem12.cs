using System.Reflection.PortableExecutable;

namespace Aoc.Problems.Aoc24;

public class Problem12 : IProblem
{
    public object Solve1(string input)
    {
        var map = ParseMap(input);

        var regions = ParseRegions(map);

        var totalPrice = regions.Sum(r => r.Price());

        return totalPrice;
    }

    public object Solve2(string input)
    {
        var map = ParseMap(input);

        var regions = ParseRegions(map);

        var totalPrice = regions.Sum(r => r.Price2());

        return totalPrice;
    }

    public static char[,] ParseMap(string input)
    {
        var lines = input.SplitLines();

        var map = new char[lines.Length, lines.First().Length];

        for (var y = 0; y < lines.Length; ++y)
        {
            for (var x = 0; x < lines[y].Length; ++x)
            {
                map.Set(x, y, lines[y][x]);
            }
        }

        return map;
    }

    /// <summary>
    /// Parses all Regions from the input Map.
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    public static List<Region> ParseRegions(char[,] map)
    {
        var regions = new List<Region>();

        // Iterate each position in the map.
        var visited = new HashSet<Point2D>();
        map.Iterate((position, c) =>
        {
            if (visited.Contains(position))
            {
                return;
            }

            // Unseen position - create a new Region and floodfill.
            var region = new Region
            {
                Character = c
            };
            region.FloodFill(map, position);

            regions.Add(region);
            visited.AddRange(region.Points);
        });

        return regions;
    }

    public class Region
    {
        public char Character { get; init; }

        public HashSet<Point2D> Points { get; } = new();

        /// <summary>
        /// Flood-fills the
        /// </summary>
        /// <param name="map"></param>
        /// <param name="startPosition"></param>
        public void FloodFill(char[,] map, Point2D startPosition)
        {
            Points.Clear();

            var queue = new Queue<Point2D>();
            queue.Enqueue(startPosition);

            while (queue.TryDequeue(out var point))
            {
                if (Points.Contains(point))
                {
                    continue;
                }

                if (map.TryGet(point, out var c) && c == Character)
                {
                    Points.Add(point);

                    // Add surrounding points to the queue.
                    queue.Enqueue(point.Left());
                    queue.Enqueue(point.Right());
                    queue.Enqueue(point.Up());
                    queue.Enqueue(point.Down());
                }
            }
        }

        /// <summary>
        /// Calculates the area of the region.
        /// </summary>
        /// <returns></returns>
        public int Area()
        {
            return Points.Count;
        }

        /// <summary>
        /// Calculates the perimeter of the region.
        /// </summary>
        /// <returns></returns>
        public int Perimeter()
        {
            int perimeter = 0;

            foreach (var point in Points)
            {
                Point2D[] potential = [point.Left(), point.Right(), point.Up(), point.Down()];
                int temp = 0;

                foreach (var p in potential)
                {
                    if (!Points.Contains(p))
                    {
                        temp++;
                    }
                }

                perimeter += temp;
            }

            return perimeter;
        }

        /// <summary>
        /// Calculates the amount of sides of the region.
        /// </summary>
        /// <returns></returns>
        public int Sides()
        {
            // The amount of Sides is equal to the amount of corners in the shape.
            // Corners should be easier to calculate.
            // We will look for points with 1 or 2 empty neighbors.
            // This would be a problem for 1-wide areas, so first scale the shape up by 2.

            var scaledPoints = new HashSet<Point2D>();
            foreach (var point in Points)
            {
                // Scale by 2.
                var s = new Point2D(2 * point.X, 2 * point.Y);
                scaledPoints.Add(s);

                // Generate a 2x2 point set from each scaled point:
                scaledPoints.Add(s.Down());
                scaledPoints.Add(s.Right());
                scaledPoints.Add(s.Right().Down());
            }

            // From this new area we can calculate the amount of corners.
            var corners = 0;
            foreach (var point in scaledPoints)
            {
                var emptyNeighbors =
                    new[] { point.Left(), point.Right(), point.Up(), point.Down() }
                        .Except(scaledPoints)
                        .ToArray();

                // Outer corner: two empty spots around, which are not on the same line.
                if (emptyNeighbors.Length == 2)
                {
                    if (emptyNeighbors[0].X != emptyNeighbors[1].X && emptyNeighbors[0].Y != emptyNeighbors[1].Y)
                    {
                        ++corners;
                    }
                }

                // Inner corner: zero empty spots around, but exactly one in diagonals.
                if (emptyNeighbors.Length == 0)
                {
                    var emptyDiagonals = new[]
                    {
                        point.Left().Up(), //
                        point.Left().Down(),
                        point.Right().Up(),
                        point.Right().Down(),
                    }.Count(p => !scaledPoints.Contains(p));
                    if (emptyDiagonals == 1)
                    {
                        ++corners;
                    }
                }
            }

            return corners;
        }

        public int Price() => Area() * Perimeter();

        public int Price2() => Area() * Sides();
    }
}
