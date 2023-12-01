namespace Aoc.Problems.Aoc22;

public class Problem06 : IProblem
{
    public object Solve1(string input) => FindMarker(input, 4);

    public object Solve2(string input) => FindMarker(input, 14);

    /// <summary>
    /// Finds a marker position (first substring with distinct characters) in the input.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="markerLength"></param>
    /// <returns></returns>
    private static int? FindMarker(string input, int markerLength)
    {
        // Find a marker - a string where all characters are unique.
        for (var i = 0; i <= input.Length - markerLength; ++i)
        {
            var block = input.Substring(i, markerLength);
            var isMarker = block.Distinct().Count() == markerLength;

            // Return the end of the current position.
            if (isMarker) return i + markerLength;
        }
        return null;
    }
}
