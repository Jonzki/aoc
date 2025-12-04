namespace Aoc.Utils;

/// <summary>
/// Represents a 2D point in space.
/// This is useful in many problems where we operate on a 2D map.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public struct Point2D
{
    public int X;
    public int Y;

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

    public Point2D Clone() => new(X, Y);

    public static Point2D Zero { get; } = new(0, 0);

    public static Point2D Parse(string input)
    {
        var parts = input.Split(',', ';');
        if (parts.Length != 2) throw new ArgumentException("Input string must contain exactly 2 coordinates.");
        return new Point2D(int.Parse(parts[0]), int.Parse(parts[1]));
    }

    public static implicit operator Point2D((int, int) point) => new Point2D(point.Item1, point.Item2);

    public static implicit operator (int, int)(Point2D point) => (point.X, point.Y);

    public static Point2D operator +(Point2D a, Point2D b) => new Point2D(a.X + b.X, a.Y + b.Y);

    public static Point2D operator -(Point2D a, Point2D b) => new Point2D(a.X - b.X, a.Y - b.Y);

    public static Point2D operator *(Point2D a, int multiplier) => new Point2D(a.X * multiplier, a.Y * multiplier);

    #region Unit Vectors

    public static Point2D UnitRight { get; } = new(1, 0);

    public static Point2D UnitLeft { get; } = new(-1, 0);

    public static Point2D UnitUp { get; } = new(0, -1);

    public static Point2D UnitDown { get; } = new(0, 1);

    #endregion Unit Vectors

    #region Directional builders

    /// <summary>
    /// Returns a point one step "left" of the current point.
    /// </summary>
    /// <returns></returns>
    public Point2D Left() => this + UnitLeft;

    /// <summary>
    /// Returns a point one step "right" of the current point.
    /// </summary>
    /// <returns></returns>
    public Point2D Right() => this + UnitRight;

    /// <summary>
    /// Returns a point one step "up" of the current point.
    /// </summary>
    /// <returns></returns>
    public Point2D Up() => this + UnitUp;

    /// <summary>
    /// Returns a point one step "down" of the current point.
    /// </summary>
    /// <returns></returns>
    public Point2D Down() => this + UnitDown;

    #endregion Directional builders

    /// <summary>
    /// Returns the Points surrounding the input Point, optionally including diagonals.
    /// </summary>
    /// <param name="includeDiagonals"></param>
    /// <returns></returns>
    public Point2D[] GetSurroundingPoints(bool includeDiagonals = false)
    {
        var points = new Point2D[includeDiagonals ? 8 : 4];

        // Add the cardinal positions first.
        points[0] = this.Left();
        points[1] = this.Right();
        points[2] = this.Up();
        points[3] = this.Down();

        // Include diagonals if needed.
        if (includeDiagonals)
        {
            points[4] = this.Left().Up();
            points[5] = this.Left().Down();
            points[6] = this.Right().Up();
            points[7] = this.Right().Down();
        }

        return points;
    }

    /// <summary>
    /// Considering the input Point2D as a directional vector, rotates the vector 90 degrees to the right (clockwise).
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public Point2D RotateRight90()
    {
        // https://stackoverflow.com/a/4780141
        // Rotating a vector 90 degrees is particularly simple.
        // (x, y) rotated 90 degrees around (0, 0) is (-y, x).
        //
        // If Y points down as on computer screens, then clockwise and counterclockwise are reversed.
        // (-y, x) is clockwise and (y, -x) is counterclockwise.
        return new Point2D(-Y, X);
    }

    /// <summary>
    /// Considering the input Point2D as a directional vector, rotates the vector 90 degrees to the right (counter-clockwise).
    /// </summary>
    /// <returns></returns>
    public Point2D RotateLeft90()
    {
        // https://stackoverflow.com/a/4780141
        // Rotating a vector 90 degrees is particularly simple.
        // (x, y) rotated 90 degrees around (0, 0) is (-y, x).
        //
        // If Y points down as on computer screens, then clockwise and counterclockwise are reversed.
        // (-y, x) is clockwise and (y, -x) is counterclockwise.
        return new Point2D(Y, -X);
    }

    /// <summary>
    /// Returns true if the point is in given boundaries (0..Width, 0..Height), false otherwise.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public bool IsInBounds(int width, int height)
    {
        return X >= 0 && X < width && Y >= 0 && Y < height;
    }

    /// <summary>
    /// Returns true if the point is within a rectangle described by the two opposing corners.
    /// </summary>
    /// <param name="corner1"></param>
    /// <param name="corner2"></param>
    /// <returns></returns>
    public bool IsWithin(Point2D corner1, Point2D corner2)
    {
        return X.BetweenInclusive(corner1.X, corner2.X)
            && Y.BetweenInclusive(corner1.Y, corner2.Y);
    }

    public bool PositionEquals(Point2D other)
    {
        return PositionEquals(other.X, other.Y);
    }

    public bool PositionEquals(int x, int y)
    {
        return X == x && Y == y;
    }

    public bool Equals(Point2D other)
    {
        return PositionEquals(other);
    }

    public bool Equals(int x, int y)
    {
        return PositionEquals(x, y);
    }

    public bool Equals((int X, int Y) other)
    {
        return PositionEquals(other.X, other.Y);
    }

    /// <summary>
    /// Returns a HashCode for the Point.
    /// The Hash Code will be unique when using values under 10 000.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return 10_000 * X + Y;
    }

    public override string ToString() => $"({X},{Y})";

    private string DebuggerDisplay => ToString();
}
