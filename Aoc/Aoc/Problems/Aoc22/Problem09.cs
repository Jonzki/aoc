using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Aoc.Utils;

namespace Aoc.Problems.Aoc22;

/// <summary>
/// https://adventofcode.com/2022/day/9
/// </summary>
public class Problem09 : IProblem
{
    public object Solve1(string input)
    {
        var instructions = ParseInstructions(input);

        var rope = new Rope(2);

        foreach (var instruction in instructions)
        {
            rope.MoveHead(instruction.Dir, instruction.Count);
        }

        return rope.TailVisited.Count;
    }

    public object Solve2(string input)
    {
        var instructions = ParseInstructions(input);

        // For part 2, use a 10 segment rope.
        var rope = new Rope(10);

        foreach (var instruction in instructions)
        {
            rope.MoveHead(instruction.Dir, instruction.Count);
        }

        return rope.TailVisited.Count;
    }

    /// <summary>
    /// Parses movement instructions from the input.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private List<(char Dir, int Count)> ParseInstructions(string input)
    {
        var instructions = new List<(char Dir, int Count)>();

        foreach (var line in input.SplitLines())
        {
            // Instructions look like "D <count>".
            var dir = line[0];
            var count = int.Parse(line.Substring(2));
            instructions.Add((dir, count));
        }

        return instructions;
    }

    public class Rope
    {
        /// <summary>
        /// Initializes a new Rope.
        /// Assume the head and the tail both start at the same position, overlapping.
        /// </summary>
        public Rope(int segments)
        {
            if (segments < 2)
            {
                throw new ArgumentException("Must have at least 2 segments.");
            }

            Segments = new Point2D[segments];
            for (var i = 0; i < segments; i++)
            {
                Segments[i] = (0, 0);
            }
            TailVisited = new HashSet<Point2D>() { (0, 0) };
        }

        private Point2D[] Segments { get; set; }

        public HashSet<Point2D> TailVisited { get; set; }

        /// <summary>
        /// Moves the head and subsequently, the tail.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="count"></param>
        public void MoveHead(char dir, int count)
        {
            for (var i = 0; i < count; i++)
            {
                // Move the head first.
                Segments[0] = dir switch
                {
                    'U' => Segments[0].Up(),
                    'D' => Segments[0].Down(),
                    'L' => Segments[0].Left(),
                    'R' => Segments[0].Right(),
                    _ => throw new ArgumentException($"Invalid direction '{dir}'")
                };

                // Then move each segment.
                for (var s = 1; s < Segments.Length; s++)
                {
                    MoveSegment(s);

                    // Add the tail position to visited list.
                    TailVisited.Add(Segments.Last());
                }
            }
        }

        private void MoveSegment(int index)
        {
            var head = Segments[index - 1];
            var tail = Segments[index];

            // Moves the Tail one step towards the Head, if necessary.
            var dX = Math.Abs(head.X - tail.X);
            var dY = Math.Abs(head.Y - tail.Y);

            if (dX >= 2 || dY >= 2)
            {
                // Head has moved too far - move the tail towards it.
                var direction = new Point2D(Math.Sign(head.X - tail.X), Math.Sign(head.Y - tail.Y));
                Segments[index] += direction;
            }
        }
    }
}
