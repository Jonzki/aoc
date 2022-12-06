namespace Aoc.Problems.Aoc19
{
    using Aoc.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Problem3 : IProblem
    {
        public object Solve1(string input)
        {
            var paths = input.Split(Environment.NewLine);
            return GetDistance(paths[0], paths[1]);
        }

        public object Solve2(string input)
        {
            var paths = input.Split(Environment.NewLine);
            return GetLeastSteps(paths[0], paths[1]);
        }

        /// <summary>
        /// Finds the distance of the closest intersection between the two paths.
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static int GetDistance(string path1, string path2)
        {
            // Find crossings between the two paths. Ignore origin point.
            var crossings = GetCrossings(GetPath(path1), GetPath(path2)).Where(c => c != (0, 0));

            var closestDistance = crossings.Select(c => NumberUtils.ManhattanDistance(c, (0, 0))).OrderBy(d => d).FirstOrDefault();

            return closestDistance;
        }

        /// <summary>
        /// Parses the input raw path into a list of visited coordinates.
        /// </summary>
        /// <param name="rawPath"></param>
        /// <returns></returns>
        public static List<(int x, int y)> GetPath(string rawPath)
        {
            var path = new List<(int x, int y)>();
            (int x, int y) pos = (0, 0);
            path.Add(pos);
            foreach (var instruction in rawPath.Split(','))
            {
                var dir = instruction[0];
                var amount = int.Parse(instruction.Substring(1));

                for (int i = 0; i < amount; ++i)
                {
                    pos = dir switch
                    {
                        'U' => (pos.x, pos.y + 1),
                        'D' => (pos.x, pos.y - 1),
                        'L' => (pos.x - 1, pos.y),
                        'R' => (pos.x + 1, pos.y),
                        _ => throw new ArgumentException($"Invalid direction parameter '{dir}'.")
                    };
                    path.Add(pos);
                }
            }
            return path;
        }

        /// <summary>
        /// Finds all crossing points between the two paths.
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static IEnumerable<(int x, int y)> GetCrossings(List<(int x, int y)> path1, List<(int x, int y)> path2)
            => path1.Intersect(path2);

        /// <summary>
        /// Calculates the "fewest combined steps" to reach an intersection between the two paths.
        /// </summary>
        /// <param name="rawPath1"></param>
        /// <param name="rawPath2"></param>
        /// <returns></returns>
        public static int GetLeastSteps(string rawPath1, string rawPath2)
        {
            var path1 = GetPath(rawPath1);
            var path2 = GetPath(rawPath2);

            // Find crossings between the two paths. Ignore origin point.
            var crossings = GetCrossings(path1, path2).Where(c => c != (0, 0));

            var leastSteps = int.MaxValue;
            foreach (var crossing in crossings)
            {
                // Calculate the combined steps.
                var steps = path1.IndexOf(crossing) + path2.IndexOf(crossing);
                if (steps < leastSteps) leastSteps = steps;
            }

            return leastSteps;
        }
    }
}
