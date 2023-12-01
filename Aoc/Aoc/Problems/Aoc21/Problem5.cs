using System.Diagnostics;

namespace Aoc.Problems.Aoc21;

public class Problem5 : IProblem
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Line
    {
        public Line(Point2D start, Point2D end)
        {
            Points = new List<Point2D>();

            // Calculate the direction to move towards the end.
            var dir = new Point2D(Math.Sign(end.X - start.X), Math.Sign(end.Y - start.Y));

            for (var i = 0; i < 1000; ++i)
            {
                Points.Add((start.X + i * dir.X, start.Y + i * dir.Y));

                if (Points.Last().Equals(end)) break;
            }
        }

        public List<Point2D> Points { get; set; }

        private string DebuggerDisplay => $"{Points.First()} -> {Points.Last()}";
    }

    public object Solve1(string input)
    {
        // Start by parsing all Lines. Skip diagonals.
        var lines = ParseLines(input, false);

        if (Debugger.IsAttached) DrawLines(lines);

        // "you need to determine the number of points where at least two lines overlap."
        var points = lines.SelectMany(line => line.Points).ToArray(); // Select every point from all lines.
        var groups = points.GroupBy(p => p.ToString()).ToDictionary(g => g.Key, g => g.Count()); // Group same points together.
        var overlaps = groups.Count(x => x.Value > 1); // Count overlaps (multiples of same point).

        return overlaps;
    }

    public object Solve2(string input)
    {
        // Start by parsing all Lines. Allow diagonals.
        var lines = ParseLines(input, true);

        if (Debugger.IsAttached) DrawLines(lines);

        // "you need to determine the number of points where at least two lines overlap."
        var points = lines.SelectMany(line => line.Points).ToArray(); // Select every point from all lines.
        var groups = points.GroupBy(p => p.ToString()).ToDictionary(g => g.Key, g => g.Count()); // Group same points together.
        var overlaps = groups.Count(x => x.Value > 1); // Count overlaps (multiples of same point).

        return overlaps;
    }

    public static List<Line> ParseLines(string input, bool allowDiagonals)
    {
        var lines = new List<Line>();
        var rows = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var row in rows)
        {
            // Split by separator and comma. Should become 4 items.
            var parts = row.Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var start = Point2D.Parse(parts[0]);
            var end = Point2D.Parse(parts[1]);

            // Allow skipping diagonals.
            var isDiagonal = start.X != end.X && start.Y != end.Y;
            if (isDiagonal && !allowDiagonals) continue;

            lines.Add(new Line(start, end));
        }
        return lines;
    }

    private void DrawLines(List<Line> lines)
    {
        int maxX = 0, maxY = 0;
        var counts = new Dictionary<(int X, int Y), int>();
        foreach (var line in lines)
        {
            foreach (var point in line.Points)
            {
                if (!counts.ContainsKey(point)) { counts[point] = 0; };
                counts[point]++;

                if (point.X > maxX) maxX = point.X;
                if (point.Y > maxY) maxY = point.Y;
            }
        }

        if (maxX > 50 || maxY > 50)
        {
            Console.WriteLine("Map is too large to draw.");
            return;
        }

        for (var y = 0; y <= maxY; y++)
        {
            for (var x = 0; x <= maxX; x++)
            {
                if (counts.TryGetValue((x, y), out var count))
                {
                    Console.Write(count);
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine("");
        }
    }
}
