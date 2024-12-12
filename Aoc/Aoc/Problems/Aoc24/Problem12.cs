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
        throw new NotImplementedException();
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
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;

            // Find the bounds for possible sides.
            foreach (var point in Points)
            {
                if (point.X < minX)
                {
                    minX = point.X;
                }
                if (point.X > maxX)
                {
                    maxX = point.X;
                }

                if (point.Y < minY)
                {
                    minY = point.Y;
                }
                if (point.Y > maxY)
                {
                    maxY = point.Y;
                }
            }

            // Scan horizontal.
            HashSet<int> sidesX = new();
            for (var x = minX - 1; x < maxX + 1; ++x)
            {
                foreach (var point in Points.Where(p => p.X == x))
                {
                    if (!Points.Contains(point.Left()))
                    {
                        sidesX.Add(x);
                    }
                    if (!Points.Contains(point.Right()))
                    {
                        sidesX.Add(x);
                    }
                }
            }
            // Scan vertical
            HashSet<int> sidesY = new();
            for (var y = minY - 1; y < maxY + 1; ++y)
            {
                foreach (var point in Points.Where(p => p.Y == y))
                {
                    if (!Points.Contains(point.Up()))
                    {
                        sidesY.Add(y);
                    }
                    if (!Points.Contains(point.Down()))
                    {
                        sidesY.Add(y);
                    }
                }
            }

            return sidesX.Count + sidesY.Count;
        }

        public int Price() => Area() * Perimeter();

        public int Price2() => Area() * Sides();
    }
}
