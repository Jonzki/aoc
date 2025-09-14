using Spectre.Console;

namespace Aoc.Problems.Aoc24;

public class Problem16 : IProblem
{
    public object Solve1(string input)
    {
        var map = ParseMap(input);

        map.Draw();

        var paths2 = ResolvePaths(map);
        Console.WriteLine($"Found {paths2.Count} paths to finish.");

        // Calculate the score for each path.
        var scores = paths2.ToDictionary(p => p.Id, p => CalculateScore(p));

        // For part 1, we return the lowest possible score.
        var lowestScore = scores.MinBy(x => x.Value);

        map.Draw(paths2.First(p => p.Id == lowestScore.Key).VisitedPoints.ToHashSet(), $"Path {lowestScore.Key} has lowest score of {lowestScore.Value}.");

        return lowestScore.Value;
    }

    public object Solve2(string input)
    {
        var map = ParseMap(input);

        map.Draw();

        // In part 2, we are interested in all visited points of the best scoring paths.
        // Find the best scoring path first.
        var paths2 = ResolvePaths(map);
        Console.WriteLine($"Found {paths2.Count} paths to finish.");

        // Calculate the score for each path.
        var scores = paths2.ToDictionary(p => p.Id, p => CalculateScore(p));

        // Find the best score.
        var lowestScore = scores.MinBy(x => x.Value);

        var bestPath = paths2.First(p => p.Id == lowestScore.Key);
        map.Draw(bestPath.VisitedPoints.ToHashSet(), $"Path {lowestScore.Key} has lowest score of {lowestScore.Value}.");

        Console.WriteLine($"Finding paths with a score of exactly: {lowestScore.Value}..");

        // Calculate the paths again, this time using the target score.
        scores.Clear();
        paths2 = ResolvePaths(map, useScoreCache: true, targetScore: lowestScore.Value, maxSteps: bestPath.VisitedPoints.Count);

        // Then find all paths with this score:
        var pathsWithBestScore = paths2.Where(p => CalculateScore(p) == lowestScore.Value);

        // Collect all visited points together.
        var visitedPoints = pathsWithBestScore.SelectMany(p => p.VisitedPoints).Distinct().ToList();

        var title =
            $"Found {pathsWithBestScore.Count()} paths with the best score of {lowestScore.Value}. {visitedPoints.Count} positions visited in total.";
        map.Draw(visitedPoints.ToHashSet(), title);

        return visitedPoints.Count;
    }

    public static Map ParseMap(string input)
    {
        var lines = input.SplitLines();

        var height = lines.Length;
        var width = lines[0].Length;

        var map = new Map
        {
            Width = width,
            Height = height
        };

        for (var y = 0; y < height; ++y)
        {
            for (var x = 0; x < width; ++x)
            {
                switch (lines[y][x])
                {
                    case '.':
                        map.Positions.Add((x, y));
                        break;

                    case 'S':
                        map.Positions.Add((x, y));
                        map.StartPoint = (x, y);
                        break;

                    case 'E':
                        map.Positions.Add((x, y));
                        map.EndPoint = (x, y);
                        break;
                }
            }
        }

        CalculateDeadEnds(map);

        return map;
    }

    public class Map
    {
        public int Width { get; init; }

        public int Height { get; init; }

        /// <summary>
        /// All movable positions on the Map.
        /// </summary>
        public HashSet<Point2D> Positions { get; } = new();

        /// <summary>
        /// All dead end points on the Map.
        /// </summary>
        public HashSet<Point2D> DeadPoints { get; } = new();

        /// <summary>
        /// Lowest known score for each position.
        /// </summary>
        public Dictionary<string, int> Scores { get; } = new();

        public Point2D StartPoint { get; set; }

        public Point2D EndPoint { get; set; }

        public void Draw(HashSet<Point2D>? visitedPoints = null, string? overrideTitle = null)
        {
            AnsiConsole.WriteLine(overrideTitle ?? "Map:");

            var canvas = new Canvas(Width, Height);

            for (var y = 0; y < Height; ++y)
            {
                for (var x = 0; x < Width; ++x)
                {
                    var pos = new Point2D(x, y);

                    if (visitedPoints?.Contains(pos) == true)
                    {
                        canvas.SetPixel(x, y, Color.Green);
                    }
                    else if (DeadPoints.Contains(pos))
                    {
                        canvas.SetPixel(x, y, Color.OrangeRed1);
                    }
                    else if (StartPoint.PositionEquals(pos))
                    {
                        canvas.SetPixel(x, y, Color.Green1);
                    }
                    else if (EndPoint.PositionEquals(pos))
                    {
                        canvas.SetPixel(x, y, Color.Blue);
                    }
                    else if (Positions.Contains(pos))
                    {
                        canvas.SetPixel(x, y, Color.Black);
                    }
                    else
                    {
                        canvas.SetPixel(x, y, Color.Red);
                    }
                }
            }

            AnsiConsole.Write(canvas);
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Finds all possible Paths from the map Start point to the map End point.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="useScoreCache">Whether to use the score lookup to optimize path finding.</param>
    /// <param name="targetScore">Target score to kill suboptimal paths with. Requires actually calculating the best path first.</param>
    /// <param name="maxSteps">Maximum steps to allow for a Path. A longer path must have more turns than the optimal one.</param>
    /// <returns></returns>
    public static List<MapPath> ResolvePaths(Map map, bool useScoreCache = true, int? targetScore = null, int? maxSteps = null)
    {
        // Seed the path list with the starting point.
        List<MapPath> paths = [
            new MapPath([map.StartPoint])
        ];

        // Track the lowest score for each position, as well as the paths that have reached this position with that score.
        var scoresByPosition = new Dictionary<string, (int LowestScore, HashSet<int> PathIds)>();
        foreach (var position in map.Positions)
        {
            scoresByPosition.Add(ScoreKey(position, Point2D.UnitLeft), (int.MaxValue, []));
            scoresByPosition.Add(ScoreKey(position, Point2D.UnitRight), (int.MaxValue, []));
            scoresByPosition.Add(ScoreKey(position, Point2D.UnitUp), (int.MaxValue, []));
            scoresByPosition.Add(ScoreKey(position, Point2D.UnitDown), (int.MaxValue, []));
        }

        // Advance each Path.
        // Maximum theoretical length for a Path is the amount of positions on the Map - no need to iterate further.
        for (var c = 0; c < map.Positions.Count; ++c)
        {
            bool anyPathMoved = false;

            List<MapPath> newPaths = new();

            // Occasionally trim out dead paths.
            if (c % 50 == 0)
            {
                var countBefore = paths.Count;
                paths = paths.Where(p => p.IsActive || p.ReachedEndPoint).ToList();
                Console.WriteLine($"Trimmed dead paths from {countBefore} to {paths.Count}.");
            }

            // Debug: find longest active path to debug.
            var debugId = paths.OrderByDescending(p => p.VisitedPoints.Count).FirstOrDefault(p => p.IsActive || p.ReachedEndPoint)?.Id;

            HashSet<int> pathsToKill = new();

            foreach (var path in paths)
            {
                // Check if the Path is finished.
                if (!path.IsActive)
                {
                    continue;
                }

                if (maxSteps.HasValue && path.VisitedPoints.Count > maxSteps.Value)
                {
                    Console.WriteLine($"Path {path.Id} over steps limit - killing it.");
                    path.IsActive = false;
                    continue;
                }

                var pos = path.CurrentPosition;
                var pathScore = CalculateScore(path);

                if (targetScore.HasValue && pathScore > targetScore.Value)
                {
                    Console.WriteLine($"Path {path.Id} over score limit - killing it.");
                    path.IsActive = false;
                    continue;
                }

                if (path.CurrentPosition.PositionEquals(map.EndPoint))
                {
                    map.Draw(path.VisitedPoints.ToHashSet(), $"Path {path.Id} reached the finish!");

                    path.IsActive = false;
                    path.ReachedEndPoint = true;
                    continue;
                }

                if (useScoreCache)
                {
                    var scoreKey = ScoreKey(path.CurrentPosition, path.CurrentDirection);

                    var positionScore = scoresByPosition[scoreKey];
                    if (pathScore < positionScore.LowestScore)
                    {
                        // Found a more optimal path to this position. Need to kill all other paths.
                        pathsToKill.AddRange(positionScore.PathIds);

                        // Replace the lowest score with the current path.
                        scoresByPosition[scoreKey] = (pathScore, [path.Id]);
                    }
                    else if (pathScore == positionScore.LowestScore)
                    {
                        // If our score is equal, add it to the list of IDs.
                        scoresByPosition[scoreKey].PathIds.Add(path.Id);
                    }
                    else
                    {
                        // If our score is higher, it is us who should be eliminated.
                        pathsToKill.Add(path.Id);
                    }
                }

                var nextPositions = GetPossibleNextPoints(map, path.CurrentPosition)
                    //.Except(deadPoints)
                    .Except(path.VisitedPoints)
                    .ToList();

                if (nextPositions.Count == 0)
                {
                    path.IsActive = false;
                    continue;
                }

                anyPathMoved = true;

                for (var i = 0; i < nextPositions.Count; ++i)
                {
                    // Reuse the current path if we had multiple potential paths.
                    if (i == nextPositions.Count - 1)
                    {
                        path.VisitedPoints.Add(nextPositions[i]);
                    }
                    else
                    {
                        newPaths.Add(new MapPath([.. path.VisitedPoints, nextPositions[i]]));
                    }
                }
            }

            // Deactivate any suboptimal paths.
            foreach (var path in paths)
            {
                if (path.IsActive && pathsToKill.Contains(path.Id))
                {
                    path.IsActive = false;
                }
            }

            if (!anyPathMoved)
            {
                Console.WriteLine("No paths moved - nothing more to calculate.");
                break;
            }

            paths.AddRange(newPaths);

            if (c % 100 == 0)
            {
                // Print the current debug path occasionally.
                MapPath? pathToDraw = debugId.HasValue ? paths.First(p => p.Id == debugId) : null;
                if (pathToDraw != null)
                {
                    map.Draw(pathToDraw.VisitedPoints.ToHashSet(), $"Path {pathToDraw.Id} (active={pathToDraw.IsActive}, at finish={pathToDraw.ReachedEndPoint})");
                }
            }
        }

        // Finally, return only the routes that reached the finish.
        return paths.Where(p => p.ReachedEndPoint).ToList();
    }

    /// <summary>
    /// Returns possible next positions on the Map for the input position.
    /// Note: does NOT check if the position has been visited already.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static List<Point2D> GetPossibleNextPoints(Map map, Point2D position)
    {
        // We can move up, down, left and right.
        var points = new List<Point2D>();

        if (map.Positions.Contains(position.Up()))
        {
            points.Add(position.Up());
        }
        if (map.Positions.Contains(position.Down()))
        {
            points.Add(position.Down());
        }
        if (map.Positions.Contains(position.Left()))
        {
            points.Add(position.Left());
        }
        if (map.Positions.Contains(position.Right()))
        {
            points.Add(position.Right());
        }

        return points;
    }

    /// <summary>
    /// Calculates the score of the Path.
    /// Unfinished path gets a negative score.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="withTurns">Whether to calculate turns or not.</param>
    /// <returns></returns>
    public static int CalculateScore(MapPath path, bool withTurns = true)
    {
        // 1 point per step forward == positions visited minus starting position.
        // 1000 points per turn.

        var turns = 0;

        if (withTurns)
        {
            // We always start facing east.
            var currentDirection = Point2D.UnitRight;
            for (var i = 0; i < path.VisitedPoints.Count - 1; ++i)
            {
                // Calculate the direction the next point is at.
                var dir = path.VisitedPoints[i + 1] - path.VisitedPoints[i];

                // If this is not the direction we are facing, bump the turn count and then rotate that way.
                if (!dir.PositionEquals(currentDirection))
                {
                    ++turns;
                    currentDirection = dir;
                }
            }
        }

        // Our score calculation is now simple.
        return (path.VisitedPoints.Count - 1) + 1000 * turns;
    }

    /// <summary>
    /// Flood fills all dead ends on the Map.
    /// </summary>
    /// <param name="map"></param>
    public static void CalculateDeadEnds(Map map, int maxIterations = 50)
    {
        Console.WriteLine("Calculating dead ends on the map..");
        var i = 0;
        for (; i < maxIterations; ++i)
        {
            // Run through all positions on the Map.
            var deadPoints = new List<Point2D>();
            foreach (var position in map.Positions)
            {
                if (position.PositionEquals(map.EndPoint) || position.PositionEquals(map.StartPoint))
                {
                    continue;
                }

                // If the position has just one possible spot to move to, it's a dead end.
                var nextPoints = GetPossibleNextPoints(map, position).Except(deadPoints);

                if (nextPoints.Count() == 1)
                {
                    deadPoints.Add(position);
                }
            }

            if (deadPoints.Count > 0)
            {
                // Remove all dead point from the valid positions,
                // and add to the dead points list (for drawing).
                map.Positions.RemoveRange(deadPoints);
                map.DeadPoints.AddRange(deadPoints);
            }
            else
            {
                Console.WriteLine("Done, no dead ends found.");
                break;
            }
        }

        Console.WriteLine($"{map.DeadPoints.Count} dead points marked - ran {i}/{maxIterations} iterations.");
    }

    public static string ScoreKey(Point2D position, Point2D direction) => position + "|" + direction;

    public class MapPath
    {
        private static int _idCounter = 1;

        public MapPath(IEnumerable<Point2D> points)
        {
            this.VisitedPoints = [.. points];
            this.Id = _idCounter++;
        }

        public int Id { get; }

        public List<Point2D> VisitedPoints { get; init; }

        public Point2D CurrentPosition => VisitedPoints.Last();

        /// <summary>
        /// Returns the current facing direction of the Path.
        /// </summary>
        public Point2D CurrentDirection => VisitedPoints.Count switch
        {
            0 => throw new InvalidOperationException("Cannot calculate direction with zero points"),
            1 => Point2D.UnitRight, // Paths start facing right.
            _ => VisitedPoints[^1] - VisitedPoints[^2]
        };

        /// <summary>
        /// Indicates whether the Path is being processed.
        /// </summary>
        public bool IsActive { get; set; } = true;

        public bool ReachedEndPoint { get; set; } = false;
    }
}
