﻿using System;
using System.Diagnostics;

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

    #region Directional builders 

    public Point2D Left() => new(X - 1, Y);
    public Point2D Right() => new(X + 1, Y);
    public Point2D Up() => new(X, Y - 1);
    public Point2D Down() => new(X, Y + 1);

    #endregion

    /// <summary>
    /// Returns true if the point is in given boundaries, false otherwise.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public bool IsInBounds(int width, int height)
    {
        return X >= 0 && X < width && Y >= 0 && Y < height;
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
