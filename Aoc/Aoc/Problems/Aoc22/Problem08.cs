using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Aoc.Utils;

namespace Aoc.Problems.Aoc22;

/// <summary>
/// https://adventofcode.com/2022/day/8
/// </summary>
public class Problem08 : IProblem
{
    public object Solve1(string input)
    {
        // Start by parsing the map.
        var map = new Map(input);

        // Count visible points.
        // Corners are always visible.
        int visibleCount = 0;
        for (var y = 0; y < map.Height; ++y)
        {
            for (var x = 0; x < map.Width; ++x)
            {
                if (map.IsVisible((x, y)))
                {
                    visibleCount++;
                }
            }
        }

        return visibleCount;
    }

    public object Solve2(string input)
    {
        // Start by parsing the map.
        var map = new Map(input);

        // Track highest scenic score.
        var maxScore = -1;
        for (var y = 0; y < map.Height; ++y)
        {
            for (var x = 0; x < map.Width; ++x)
            {
                var score = map.ScenicScore((x, y));
                if (score > maxScore)
                {
                    maxScore = score;
                }
            }
        }

        return maxScore;
    }

    public class Map
    {
        public Map(string input)
        {
            var lines = input.SplitLines();
            Height = lines.Length;
            Width = lines[0].Length;

            Points = new int[Width * Height];

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    Points[ArrayUtils.GetIndex(x, y, Width)] = lines[y][x] - '0';
                }
            }
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public int[] Points { get; set; }

        /// <summary>
        /// Returns the height value at the given location, or -1 if the point is not within bounds.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public int Get(Point2D point)
        {
            if (!point.IsInBounds(Width, Height))
            {
                return -1;
            };
            return Points[ArrayUtils.GetIndex(point.X, point.Y, Width)];
        }

        /// <summary>
        /// Checks if the point is visible from any direction on the Map.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsVisible(Point2D point)
        {
            // Edges always visible.
            if (point.X == 0 || point.X == Width - 1 || point.Y == 0 || point.Y == Height - 1)
            {
                return true;
            }

            var pointValue = Get(point);

            // Assume visible.
            var visible = true;

            // Up check
            for (var p = point.Up(); p.Y >= 0; p = p.Up())
            {
                if (Get(p) >= pointValue)
                {
                    visible = false; break;
                }
            }

            if (visible) return true;
            visible = true;

            // Down
            for (var p = point.Down(); p.Y < Height; p = p.Down())
            {
                if (Get(p) >= pointValue)
                {
                    visible = false; break;
                }
            }

            if (visible) return true;
            visible = true;

            // Left
            for (var p = point.Left(); p.X >= 0; p = p.Left())
            {
                if (Get(p) >= pointValue)
                {
                    visible = false; break;
                }
            }

            if (visible) return true;
            visible = true;

            // Right
            for (var p = point.Right(); p.X < Width; p = p.Right())
            {
                if (Get(p) >= pointValue)
                {
                    visible = false; break;
                }
            }

            return visible;
        }

        /// <summary>
        /// Calculates the part 2 "scenic score".
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public int ScenicScore(Point2D point)
        {
            var pointValue = Get(point);

            // Scores need to be multiplied together.
            var totalScore = 1;

            int score = 0;
            int value;

            // Up check
            for (var p = point.Up(); p.Y >= 0; p = p.Up())
            {
                value = Get(p);
                score++;
                if (value >= pointValue) break;
            }
            totalScore *= score;
            score = 0;

            // Down
            for (var p = point.Down(); p.Y < Height; p = p.Down())
            {
                value = Get(p);
                score++;
                if (value >= pointValue) break;
            }
            totalScore *= score;
            score = 0;

            // Left
            for (var p = point.Left(); p.X >= 0; p = p.Left())
            {
                value = Get(p);
                score++;
                if (value >= pointValue) break;
            }
            totalScore *= score;
            score = 0;

            // Right
            for (var p = point.Right(); p.X < Width; p = p.Right())
            {
                value = Get(p);
                score++;
                if (value >= pointValue) break;
            }
            totalScore *= score;

            return totalScore;
        }

    }

}
