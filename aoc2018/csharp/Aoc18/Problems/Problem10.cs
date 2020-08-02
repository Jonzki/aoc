using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aoc18.Problems
{
    public class Input10
    {
        public Input10(string file)
        {
            var lines = File.ReadAllLines(file);
            Points = new List<Point>();
            foreach (var line in lines)
            {
                // position=< 4,  7> velocity=< 0, -1>
                var temp = line
                    .Replace("velocity", ",")
                    .Replace("position", "")
                    .Replace("<", "")
                    .Replace(">", "")
                    .Replace("=", "")
                    .Replace(" ", "")
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray();
                var point = new Point
                {
                    PositionX = temp[0],
                    PositionY = temp[1],
                    VelocityX = temp[2],
                    VelocityY = temp[3]
                };
                Points.Add(point);
            }
        }

        public List<Point> Points { get; }
    }

    public class Point
    {
        public int PositionX { get; set; }

        public int PositionY { get; set; }

        public int VelocityX { get; set; }

        public int VelocityY { get; set; }
    }

    public class Problem10 : Problem<Input10>
    {
        protected override object Part1(Input10 input)
        {
            var points = new List<Point>(input.Points);

            // Run the simulation until points start to get further away.
            var distX = long.MaxValue - 1;
            var distY = long.MaxValue - 1;
            var prevDistX = long.MaxValue;
            var prevDistY = long.MaxValue;

            int pivotPoint = -1;
            var second = 0;
            while (true)
            {
                //Console.WriteLine($"Second {second}:");
                Update(points);

                var b = GetBoundingBox(points);

                prevDistX = distX;
                prevDistY = distY;
                distX = b.maxX - b.minX;
                distY = b.maxY - b.minY;

                if (distX > prevDistX && distY > prevDistY || (distX < 100 && distY < 40))
                {
                    Console.WriteLine($"Found pivot point at second {second}.");
                    pivotPoint = second;
                    break;
                }
                ++second;
            }

            // Print around the pivot point.
            Console.WriteLine("Printing around pivot.");
            // Roll back for 5 steps.
            for (var i = 0; i < 5; ++i)
            {
                if (second - 1 >= 0)
                {
                    Update(points, -1);
                    --second;
                }
            }
            // Print next 10 steps.
            for(var i = 0; i < 10; ++i)
            {
                Print(points, second);
                Update(points);
                ++second;
            }




            return null;
        }

        private void Update(List<Point> points, int dir = 1)
        {
            foreach (var p in points)
            {
                p.PositionX += dir * p.VelocityX;
                p.PositionY += dir * p.VelocityY;
            }
        }

        private (int minX, int maxX, int minY, int maxY) GetBoundingBox(List<Point> points)
        {
            int minX = int.MaxValue,
                minY = int.MaxValue,
                maxX = int.MinValue,
                maxY = int.MinValue;

            foreach (var p in points)
            {
                if (p.PositionX < minX) minX = p.PositionX;
                if (p.PositionY < minY) minY = p.PositionY;
                if (p.PositionX > maxX) maxX = p.PositionX;
                if (p.PositionY > maxY) maxY = p.PositionY;
            }

            return (minX, maxX, minY, maxY);
        }

        private void Print(List<Point> points, int? second = null)
        {
            (var minX, var maxX, var minY, var maxY) = GetBoundingBox(points);

            var coords = new HashSet<(int x, int y)>();
            foreach (var p in points)
            {
                coords.Add((p.PositionX, p.PositionY));
            }

            if (second.HasValue)
            {
                Console.WriteLine($"> Map at second {second}:");
            }
            for (var y = minY; y <= maxY; ++y)
            {
                for (var x = minX; x <= maxX; ++x)
                {
                    if (coords.Contains((x, y)))
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }

        }
    }
}
