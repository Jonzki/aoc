namespace Aoc.Utils;

using System;
using System.Linq;

public static class MathUtils
{
    /// <summary>
    /// Calculates the Manhattan distance between two points.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static int ManhattanDistance((int x, int y) a, (int x, int y) b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);

    public static decimal Average(params int[] numbers)
    {
        if (numbers.Length == 0) throw new ArgumentException("Must have at least one number in input.");
        return numbers.Sum() / (decimal)numbers.Length;
    }
}
