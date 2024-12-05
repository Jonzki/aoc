namespace Aoc.Utils;

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

    /// <summary>
    /// Returns two-dimensional coordinates for the input index on a map.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static (int X, int Y) GetCoordinates(int width, int height, int index)
    {
        var x = index % width;
        var y = (index - x) / height;
        return (x, y);
    }

    /// <summary>
    /// Returns the "height" of the input two-dimensional array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static int Height<T>(this T[,] array) => array.GetLength(0);

    /// <summary>
    /// Returns the "Width" of the input two-dimensional array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static int Width<T>(this T[,] array) => array.GetLength(1);

    /// <summary>
    /// Returns a value from the 2D array based on input coordinates.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static T Get<T>(this T[,] array, int x, int y) => array[y, x];

    /// <summary>
    /// Returns a value from the 2D array based on input coordinates.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public static T Get<T>(this T[,] array, Point2D point) => array[point.Y, point.X];

    public static bool TryGet<T>(this T[,] array, int x, int y, out T? value)
    {
        value = default;

        if (x < 0 || x >= array.Width()) return false;
        if (y < 0 || y >= array.Height()) return false;

        value = array[y, x];
        return true;
    }

    public static int IndexOf<T>(this T[] array, T value) where T : IEquatable<T>
    {
        for (var i = 0; i < array.Length; ++i)
        {
            if (array[i].Equals(value))
            {
                return i;
            }
        }

        return -1;
    }
}
