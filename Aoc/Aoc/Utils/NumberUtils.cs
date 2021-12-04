using System;
using System.Collections;

namespace Aoc.Utils
{
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
            var i = index + 0;
            // Negative index should roll to the back 
            while (i < 0) i = length + i;
            while (i >= length) i = i - length;
            return i;
        }

        public static bool Between(this int value, int from, int to) => from < value && value < to;

        public static bool BetweenInclusive(this int value, int from, int to) => from <= value && value <= to;

        public static bool Between(this long value, long from, long to) => from < value && value < to;

        public static bool BetweenInclusive(this long value, long from, long to) => from <= value && value <= to;

        public static int ManhattanDistance(int x1, int y1, int x2, int y2) => Math.Abs(x2 - x1) + Math.Abs(y2 - y1);

        public static int ManhattanDistance((int x, int y) a, (int x, int y) b) => ManhattanDistance(a.x, a.y, b.x, b.y);
    }
}
