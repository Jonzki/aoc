using Spectre.Console;

namespace Aoc.Problems.Aoc24;

public class Problem16 : IProblem
{
    private const string SmallInput1 = """
                                     ###############
                                     #.......#....E#
                                     #.#.###.#.###.#
                                     #.....#.#...#.#
                                     #.###.#####.#.#
                                     #.#.#.......#.#
                                     #.#.#####.###.#
                                     #...........#.#
                                     ###.#.#####.#.#
                                     #...#.....#.#.#
                                     #.#.#.###.#.#.#
                                     #.....#...#.#.#
                                     #.###.#.#.#.#.#
                                     #S..#.....#...#
                                     ###############
                                     """;

    public object Solve1(string input)
    {
        var map = ParseMap(input);

        map.Draw();

        map.Scores.Add(map.StartPoint.ToString(), 0);

        var paths = ResolvePaths(map, false);

        var v = paths.First().CalculateVisited(map);
        map.Draw([.. v]);
        //Console.WriteLine(paths.FirstOrDefault()?.Score() ?? -1);

        return map.Scores.GetValueOrDefault(map.EndPoint.ToString(), 0);
    }

    public object Solve2(string input)
    {
        var map = ParseMap(input);

        map.Draw();

        map.Scores.Add(map.StartPoint.ToString(), 0);

        // Find the best score like before.
        ResolvePaths(map, true);

        // However, this time we want to find all paths resulting in this score
        // and return the points visited by these.
        var score = map.Scores.GetValueOrDefault(map.EndPoint.ToString(), 0);

        var paths = ResolvePaths(map, false, score);
        Console.WriteLine($"{paths.Count} best paths");

        var visited = new HashSet<Point2D>();
        foreach (var path in paths)
        {
            visited.AddRange(path.CalculateVisited(map));
        }

        map.Draw(visited);

        return visited.Count;
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

        return map;
    }

    public class Map
    {
        public int Width { get; init; }

        public int Height { get; init; }

        public HashSet<Point2D> Positions { get; } = new();

        /// <summary>
        /// Lowest known score for each position.
        /// </summary>
        public Dictionary<string, int> Scores { get; } = new();

        public Point2D StartPoint { get; set; }
        public Point2D EndPoint { get; set; }

        //public Dictionary<string, Node>

        public void Draw(HashSet<Point2D>? visitedPoints = null)
        {
            AnsiConsole.WriteLine("Map:");

            var canvas = new Canvas(Width, Height);

            for (var y = 0; y < Height; ++y)
            {
                for (var x = 0; x < Width; ++x)
                {
                    if (visitedPoints != null && visitedPoints.Contains(new Point2D(x, y)))
                    {
                        canvas.SetPixel(x, y, Color.Green);
                    }
                    else if (StartPoint.PositionEquals(x, y))
                    {
                        canvas.SetPixel(x, y, Color.Green1);
                    }
                    else if (EndPoint.PositionEquals(x, y))
                    {
                        canvas.SetPixel(x, y, Color.Green3);
                    }
                    else if (Positions.Contains((x, y)))
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

    public static List<MapPath> ResolvePaths(Map map, bool useCache, int targetScore = -1)
    {
        // In each position we should have 3 possible options:
        // - Move forward
        // - Turn left and move forward
        // - Turn right and move forward

        List<MapPath> paths = new();

        // Seed the paths set with the initial position.
        paths.Add(new MapPath
        {
            Position = map.StartPoint
        });

        while (paths.Count > 0)
        {
            if (paths.All(p => p.Finished))
            {
                break;
            }

            var newPaths = new List<MapPath>();

            var pathCount = paths.Count;
            for (var i = 0; i < pathCount; ++i)
            {
                var forward = paths[i].Move(map, MapStep.Forward, useCache, targetScore);
                if (forward != null)
                {
                    newPaths.Add(forward);
                }

                var right = paths[i].Move(map, MapStep.TurnRight, useCache, targetScore);
                if (right != null)
                {
                    newPaths.Add(right);
                }

                var left = paths[i].Move(map, MapStep.TurnLeft, useCache, targetScore);
                if (left != null)
                {
                    newPaths.Add(left);
                }
            }

            paths = newPaths;
        }

        return paths;
    }

    public class MapPath
    {
        public bool Finished { get; set; } = false;

        public Point2D Position { get; set; }

        // Start facing east.
        public Point2D Direction { get; set; } = new(1, 0);

        public List<MapStep> Steps { get; init; } = new();

        public int Score()
        {
            int sum = 0;
            foreach (var step in Steps)
            {
                sum += step switch
                {
                    MapStep.Forward => 1,
                    MapStep.TurnLeft => 1000,
                    MapStep.TurnRight => 1000,
                    _ => 0
                };
            }

            return sum;
        }

        public MapPath? Move(Map map, MapStep step, bool useCache, int targetScore = -1)
        {
            // Abort processing if we're over the best score.
            if (targetScore >= 0 && Score() > targetScore)
            {
                return null;
            }

            var newDirection = step switch
            {
                MapStep.Forward => Direction,
                MapStep.TurnLeft => Direction.RotateLeft90(),
                MapStep.TurnRight => Direction.RotateRight90(),
                _ => throw new InvalidOperationException()
            };
            var target = Position + newDirection;

            // If the target position is impossible, abort.
            if (!map.Positions.Contains(target))
            {
                return null;
            }

            var newPath = Clone();

            newPath.Position = target;
            newPath.Direction = newDirection;

            newPath.Finished = newPath.Position.PositionEquals(map.EndPoint);

            newPath.Steps.Add(step);
            if (step is MapStep.TurnLeft or MapStep.TurnRight)
            {
                newPath.Steps.Add(MapStep.Forward);
            }

            var score = newPath.Score();

            var key = newPath.Position.ToString();

            map.Scores.TryAdd(key, score);

            if (useCache)
            {
                if (map.Scores[key] < score)
                {
                    // We have already found a more optimal route to the position - abort this path.
                    return null;
                }
                else
                {
                    map.Scores[key] = score;
                }
            }

            return newPath;
        }

        public MapPath Clone()
        {
            return new MapPath
            {
                Position = Position,
                Direction = Direction,
                Steps = [.. Steps]
            };
        }

        /// <summary>
        /// Calculates the positions that this path visited.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Point2D> CalculateVisited(Map map)
        {
            var path = new MapPath { Position = map.StartPoint };
            var points = new HashSet<Point2D>();

            points.Add(path.Position);

            // The path resolver injects a forward step into the stream
            // (the turn operation also moves forward).
            // Need to compensate here.
            for (var i = 0; i < Steps.Count; ++i)
            {
                if (i > 0 && Steps[i - 1] is MapStep.TurnLeft or MapStep.TurnRight && Steps[i] is MapStep.Forward)
                {
                    continue;
                }

                path = path.Move(map, Steps[i], false)!;
                points.Add(path.Position);
            }

            return points;
        }
    }

    public enum MapStep
    {
        /// <summary>
        /// Cost: 1
        /// </summary>
        Forward,

        /// <summary>
        /// Cost: 1000
        /// </summary>
        TurnLeft,

        /// <summary>
        /// Cost: 1000
        /// </summary>
        TurnRight
    }
}
