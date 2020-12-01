namespace Aoc.Utils
{
    using System;

    public static class MathUtils
    {
        /// <summary>
        /// Calculates the Manhattan distance between two points.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int ManhattanDistance((int x, int y) a, (int x, int y) b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }
}