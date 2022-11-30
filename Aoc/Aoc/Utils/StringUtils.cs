namespace Aoc.Utils
{
    public static class StringUtils
    {
        public static string[] SplitLines(this string input) => input.Replace("\r", "").Split("\n");
    }
}
