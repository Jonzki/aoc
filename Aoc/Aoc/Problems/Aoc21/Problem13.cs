namespace Aoc.Problems.Aoc21;

public class Problem13 : IProblem
{
    public object Solve1(string input)
    {
        var (points, folds) = ParseInput(input);

        points = ApplyFolds(points, folds, 1);

        return points.Count;
    }

    public object Solve2(string input)
    {
        var (points, folds) = ParseInput(input);

        points = ApplyFolds(points, folds);

        Print(points);

        return "Read answer above.";
    }

    private List<Point2D> ApplyFolds(List<Point2D> points, List<Point2D> folds, int limit = int.MaxValue)
    {
        for (var i = 0; i < folds.Count && i < limit; ++i)
        {
            points = Fold(points, folds[i]);
        }
        return points;
    }

    private List<Point2D> Fold(List<Point2D> points, Point2D fold)
    {
        // Move all points past the fold line.
        foreach (var point in points)
        {
            if (fold.X >= 0 && point.X > fold.X)
            {
                // Move the point left by given amount. 
                point.X -= (2 * Math.Abs(point.X - fold.X));
            }
            else if (fold.Y >= 0 && point.Y > fold.Y)
            {
                point.Y -= (2 * Math.Abs(point.Y - fold.Y));
            }
        }

        // Remove duplicate points.
        return points.DistinctBy(p => (p.X, p.Y)).ToList();
    }

    public (List<Point2D> Points, List<Point2D> Folds) ParseInput(string input)
    {
        var lines = input.SplitLines();

        var points = new List<Point2D>();
        var folds = new List<Point2D>();
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            if (line.StartsWith("fold along "))
            {
                // Handle fold instructions.
                var parts = line.Substring("fold along ".Length).Split('=');
                if (parts[0] == "x")
                {
                    folds.Add(new(int.Parse(parts[1]), -1));
                }
                else if (parts[0] == "y")
                {
                    folds.Add(new(-1, int.Parse(parts[1])));
                }
                else
                {
                    throw new InvalidOperationException($"Bad fold instruction: '{line}'");
                }
            }
            else
            {
                // Parse other line types as coordinates.
                var parts = line.Split(",");
                points.Add(new(int.Parse(parts[0]), int.Parse(parts[1])));
            }
        }

        return (points, folds);
    }

    private void Print(List<Point2D> points)
    {
        int maxX = -1, maxY = -1;
        foreach (var point in points)
        {
            if (point.X > maxX) maxX = point.X;
            if (point.Y > maxY) maxY = point.Y;
        }

        for (var y = 0; y <= maxY; y++)
        {
            for (var x = 0; x <= maxX; x++)
            {
                if (points.Contains(new(x, y)))
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine("");
        }
    }
}
