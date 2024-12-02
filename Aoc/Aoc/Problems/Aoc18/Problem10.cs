namespace Aoc.Problems.Aoc18;

/// <summary>
/// https://adventofcode.com/2018/day/10
/// </summary>
public class Problem10 : IProblem
{
    private const string SmallInput = @"position=< 9,  1> velocity=< 0,  2>
position=< 7,  0> velocity=<-1,  0>
position=< 3, -2> velocity=<-1,  1>
position=< 6, 10> velocity=<-2, -1>
position=< 2, -4> velocity=< 2,  2>
position=<-6, 10> velocity=< 2, -2>
position=< 1,  8> velocity=< 1, -1>
position=< 1,  7> velocity=< 1,  0>
position=<-3, 11> velocity=< 1, -2>
position=< 7,  6> velocity=<-1, -1>
position=<-2,  3> velocity=< 1,  0>
position=<-4,  3> velocity=< 2,  0>
position=<10, -3> velocity=<-1,  1>
position=< 5, 11> velocity=< 1, -2>
position=< 4,  7> velocity=< 0, -1>
position=< 8, -2> velocity=< 0,  1>
position=<15,  0> velocity=<-2,  0>
position=< 1,  6> velocity=< 1,  0>
position=< 8,  9> velocity=< 0, -1>
position=< 3,  3> velocity=<-1,  1>
position=< 0,  5> velocity=< 0, -1>
position=<-2,  2> velocity=< 2,  0>
position=< 5, -2> velocity=< 1,  2>
position=< 1,  4> velocity=< 2,  1>
position=<-2,  7> velocity=< 2, -2>
position=< 3,  6> velocity=<-1, -1>
position=< 5,  0> velocity=< 1,  0>
position=<-6,  0> velocity=< 2,  0>
position=< 5,  9> velocity=< 1, -2>
position=<14,  7> velocity=<-2,  0>
position=<-3,  6> velocity=< 2, -1>";

    public object Solve1(string input)
    {
        var parsed = Input10.Parse(input);
        Simulate(parsed);

        // The solution for this problem is read from the Console - no simple return value.
        return 0;
    }

    public object Solve2(string input)
    {
        // Part 1 also shows the second value for part 2.
        return 0;
    }

    private void Simulate(Input10 input)
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
        for (var i = 0; i < 10; ++i)
        {
            Print(points, second);
            Update(points);
            ++second;
        }
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

    public class Input10
    {
        public static Input10 Parse(string input)
        {
            var points = new List<Point>();
            foreach (var line in input.SplitLines())
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
                points.Add(point);
            }

            return new Input10 { Points = points };
        }

        public required List<Point> Points { get; init; }
    }

    public class Point
    {
        public int PositionX { get; set; }

        public int PositionY { get; set; }

        public int VelocityX { get; set; }

        public int VelocityY { get; set; }
    }
}
