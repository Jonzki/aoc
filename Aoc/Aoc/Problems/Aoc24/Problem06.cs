namespace Aoc.Problems.Aoc24;

public class Problem06 : IProblem
{
    public object Solve1(string input)
    {
        var map = ParseMap(input);

        return Navigate(map).VisitCount;
    }

    public object Solve2(string input)
    {
        var map = ParseMap(input);

        // Do an initial navigation to resolve the potential points to inject obstacles.
        var (_, navData) = Navigate(map);

        // Our possible loop-inducing points are around the navigated points - collect those.
        var possibleLoopPoints = navData.Keys.SelectMany<Point2D, Point2D>((Point2D p) =>
        [
            p.Left(),
            p.Right(),
            p.Up(),
            p.Down()
        ])
        .Where(p => map.IsInBounds(p) && map.Get(p) == '.')
        .Distinct()
        .ToArray();

        Console.WriteLine($"{possibleLoopPoints.Length} possible loop points.");

        // On a single thread this processing ends up in the 30s region.
        // Interestingly, multithreading to 8 or 16 Tasks does not affect this basically at all.

        int loopCount = 0;

        foreach (var loopPoint in possibleLoopPoints)
        {
            // Run a navigation loop for this position.
            var (visits, _) = Navigate(map, loopPoint);
            if (visits == -1)
            {
                ++loopCount;
            }
        }

        // Count the amount of navigations that ended in a loop.
        return loopCount;
    }

    /// <summary>
    /// Navigates the input Map. Returns the amount of tiles visited before exiting the map,
    /// or -1 if a loop is detected.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="extraObstacle">Part 2 optional extra obstacle.</param>
    /// <returns></returns>
    private (int VisitCount, Dictionary<Point2D, HashSet<Point2D>> VisitData) Navigate(char[,] map, Point2D? extraObstacle = null)
    {
        // Locate the Guard first.
        var guard = LocateGuard(map);

        // Track the visited locations and the directions we've encountered in each position.
        var visited = new Dictionary<Point2D, HashSet<Point2D>>();

        // Guard keeps walking around until they are off the map.
        while (map.IsInBounds(guard.Position.X, guard.Position.Y))
        {
            // Mark the current position as visited.
            visited.TryAdd(guard.Position, new HashSet<Point2D>());
            if (!visited[guard.Position].Add(guard.Direction))
            {
                // This position has been reached before in the current direction - we are in a loop.
                return (-1, visited);
            }

            var newPos = guard.Position + guard.Direction;
            if (map.IsInBounds(newPos.X, newPos.Y) && (map[newPos.Y, newPos.X] == '#' || extraObstacle?.PositionEquals(newPos) == true))
            {
                // Hit an obstacle - turn right.
                guard.Direction = guard.Direction.RotateRight90();
            }
            else
            {
                // Did not hit an obstacle - move along.
                guard.Position += guard.Direction;
            }
        }

        return (visited.Keys.Count, visited);
    }

    private char[,] ParseMap(string input)
    {
        var lines = input.SplitLines();

        var w = lines[0].Length;
        var h = lines.Length;

        return ArrayUtils.To2D(input.RemoveString("\r\n").ToCharArray(), w, h);
    }

    /// <summary>
    /// Finds the guard position and current location on the input Map.
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    private (Point2D Position, Point2D Direction) LocateGuard(char[,] map)
    {
        // Scan the entire map.
        for (var x = 0; x < map.Width(); ++x)
        {
            for (var y = 0; y < map.Height(); ++y)
            {
                if (map.Get(x, y) == '^')
                {
                    // Guard is currently facing up.
                    return (new Point2D(x, y), new Point2D(0, -1));
                }
            }
        }

        throw new InvalidOperationException("Could not locate guard on map.");
    }

    /// <summary>
    /// Prints the map on the console.
    /// </summary>
    private void Print(char[,] map, Point2D guardPosition, Point2D guardDirection, Dictionary<Point2D, HashSet<Point2D>> visitData, bool separator = true)
    {
        var w = map.Width();
        var h = map.Height();

        if (separator)
        {
            Console.WriteLine(new string('-', w));
        }

        for (var y = 0; y < h; ++y)
        {
            for (var x = 0; x < w; ++x)
            {
                if (guardPosition.Equals(x, y))
                {
                    // Print the correct guard direction.
                    if (guardDirection.Equals(1, 0))
                    {
                        Console.Write(">");
                    }
                    else if (guardDirection.Equals(0, 1))
                    {
                        Console.Write("V");
                    }
                    else if (guardDirection.Equals(-1, 0))
                    {
                        Console.Write("V");
                    }
                    else if (guardDirection.Equals(0, -1))
                    {
                        Console.Write("^");
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unexpected guard direction: {guardDirection}");
                    }
                }
                else
                {
                    if (visitData.ContainsKey(new(x, y)))
                    {
                        Console.Write('X');
                    }
                    else
                    {
                        Console.Write(map.Get(x, y));
                    }
                }
            }
            Console.WriteLine();
        }
    }
}
