namespace Aoc.Utils;

internal static class CharUtils
{
    /// <summary>
    /// Lowercase alphabet (a-z).
    /// </summary>
    public static readonly char[] Alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

    /// <summary>
    /// Array of digit characters (0-9).
    /// </summary>
    public static readonly char[] Digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    public static Comparison<char> SortLowercase = (a, b) => b - a;
}
