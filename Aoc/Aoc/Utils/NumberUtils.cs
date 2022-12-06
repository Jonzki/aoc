using System;
using System.Linq;

namespace Aoc.Utils;

public static class NumberUtils
{
    /// <summary>
    /// Returns a roll-over index for the input index and array size.
    /// Values larger than array size are rotated to the start of the array.
    /// Values less than zero are rotated back of the array.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static int GetRolloverIndex(int index, int length)
    {
        if (index < 0)
        {
            // Rotate sub-zero values to the end of the "array".
            return (index % length) + length;
        }
        else
        {
            return index % length;
        }
    }

    public static bool Between(this int value, int from, int to) => from < value && value < to;

    public static bool BetweenInclusive(this int value, int from, int to) => from <= value && value <= to;

    public static bool Between(this long value, long from, long to) => from < value && value < to;

    public static bool BetweenInclusive(this long value, long from, long to) => from <= value && value <= to;

    public static int ManhattanDistance(int x1, int y1, int x2, int y2) => Math.Abs(x2 - x1) + Math.Abs(y2 - y1);

    /// <summary>
    /// Calculates the Manhattan distance between two points.
    /// Manhattan distance = abs(x2-x1) + abs(y2-y1)
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static int ManhattanDistance((int x, int y) a, (int x, int y) b) => ManhattanDistance(a.x, a.y, b.x, b.y);

    public static decimal Average(params int[] numbers)
    {
        if (numbers.Length == 0) throw new ArgumentException("Must have at least one number in input.");
        return numbers.Sum() / (decimal)numbers.Length;
    }
}
