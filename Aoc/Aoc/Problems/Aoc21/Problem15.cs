using System;
using System.Collections.Generic;
using System.Linq;
using Aoc.Utils;

namespace Aoc.Problems.Aoc21;

public class Problem15 : IProblem
{
    public const string MiniInput = @"12
11";

    public const string MazeInput = @"19111
19191
11191";

    public const string SmallInput = @"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581";


    public object Solve1(string input)
    {
        // Parse the map.
        var map = Map.Parse(input);

        //map.Print();

        Console.WriteLine(new string('-', map.Width));

        // Start at 0,0 with risk 0.
        var path = new Path
        {
            Points = new List<Point2D>() { new(0, 0) },
            Risk = 0
        };

        FindBestPath(map, path);

        Console.WriteLine($"Best path found with a risk of {map.BestPath.Risk}:");
        map.Print(map.BestPath);

        return map.BestPath?.Risk ?? -1;
    }

    public object Solve2(string input)
    {
        throw new NotImplementedException();
    }

    public class Map
    {
        public long[,] Points { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Path BestPath { get; set; }

        /// <summary>
        /// Parses a map from the problem input.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Map Parse(string input)
        {
            var lines = input.SplitLines();
            var width = lines[0].Length;
            var height = lines.Length;

            var temp = lines.SelectMany(l => l.Select(c => (long)(c - '0'))).ToArray();

            var points = ArrayUtils.To2D(temp, width, height);

            return new Map
            {
                Points = points,
                Width = width,
                Height = height
            };
        }

        public void Print(Path path = null)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (path != null && path.Points.Contains(new(x, y)))
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(Points.Get(x, y));
                    }
                }
                Console.WriteLine();
            }
        }
    }


    /// <summary>
    /// Finds the best path (lowest risk) from 0,0 to (w,h).
    /// </summary>
    /// <param name="map"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    long FindBestPath(Map map, Path path)
    {
        // Find all possible next steps.
        var pos = path.Points.Last();

        if (pos.X == map.Width - 1 && pos.Y == map.Height - 1)
        {
            //Console.WriteLine($"Found a path to finish with a risk of {path.Risk}");
            //map.Print(path);
            if (map.BestPath == null || path.Risk < map.BestPath.Risk)
            {
                //Console.WriteLine("This path improves on previous best.");
                map.BestPath = path;
            }
            return path.Risk;
        }

        var steps = new[]
        {
            pos.Left(), pos.Right(), pos.Up(), pos.Down()
        }.Where(p => p.IsInBounds(map.Width, map.Height) && !path.Points.Contains(p));

        foreach (var p in steps)
        {
            // Test each step.
            var potentialPath = path.Step(p, map.Points.Get(p));
            if (map.BestPath != null && map.BestPath.Risk < potentialPath.Risk)
            {
                // This path is already more risky than the best we've found - abort.
                //Console.WriteLine($"Abort worse path, risk reached {potentialPath.Risk}.");
            }
            else
            {
                // This path still has potential to be the best - keep searching.
                FindBestPath(map, potentialPath);
            }
        }

        return path.Risk;
    }

    public class Path
    {
        public List<Point2D> Points { get; set; } = new();
        public long Risk { get; set; } = 0;

        public Path Step(Point2D point, long risk)
        {
            return new Path
            {
                Points = this.Points.Select(p => new Point2D(p)).Append(point).ToList(),
                Risk = Risk + risk
            };
        }

        internal void Print()
        {
            Console.WriteLine($"Risk {Risk}: " + string.Join(",", Points));
        }
    }


}
