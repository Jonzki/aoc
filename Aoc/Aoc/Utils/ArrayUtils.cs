using System;

namespace Aoc.Utils
{
    public static class ArrayUtils
    {
        /// <summary>
        /// Converts an array to a two-dimensional matrix.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static T[,] To2D<T>(T[] input, int width, int height)
        {
            if (input.Length != width * height) throw new ArgumentException($"Input array of {input.Length} items cannot be converted to a {width}*{height} matrix.");

            var output = new T[height, width];

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    output[y, x] = input[GetIndex(x, y, width)];
                }
            }

            return output;
        }

        /// <summary>
        /// Converts a two-dimensional matrix into an array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T[] To1D<T>(T[,] input)
        {
            var output = new T[input.Length];

            var height = input.GetLength(0);
            var width = input.GetLength(1);

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    output[GetIndex(x, y, width)] = input[y, x];
                }
            }

            return output;
        }

        /// <summary>
        /// Returns a single-dimensional index for two-dimensional coordinates, given the width of the matrix.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static int GetIndex(int x, int y, int width)
        {
            return y * width + x;
        }

    }
}