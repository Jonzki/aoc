using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Utils;

/// <summary>
/// Represents a 2D point in space.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class Point2D
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point2D(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point2D((int, int) point)
    {
        X = (int)point.Item1;
        Y = (int)point.Item2;
    }

    public Point2D Clone() => new Point2D(X, Y);

    public static Point2D Parse(string input)
    {
        var parts = input.Split(',', ';');
        if (parts.Length != 2) throw new ArgumentException("Input string must contain exactly 2 coordinates.");
        return new Point2D(int.Parse(parts[0]), int.Parse(parts[1]));
    }

    public static implicit operator Point2D((int, int) point) => new Point2D(point.Item1, point.Item2);

    public static implicit operator (int, int)(Point2D point) => (point.X, point.Y);

    public static bool operator ==(Point2D a, Point2D b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Point2D a, Point2D b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if (obj is Point2D point)
        {
            return point.X == X && point.Y == Y;
        }
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return 10_000 * X + Y;
    }

    public override string ToString() => $"({X},{Y})";

    private string DebuggerDisplay => ToString();
}
