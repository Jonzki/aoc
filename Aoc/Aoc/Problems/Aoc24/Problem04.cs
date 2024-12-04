namespace Aoc.Problems.Aoc24;

public class Problem04 : IProblem
{
    public object Solve1(string input)
    {
        var lines = input.SplitLines();

        // Need a 2D array.
        var map = ArrayUtils.To2D(input.RemoveAll('\r').RemoveAll('\n').ToCharArray(), lines[0].Length, lines.Length);

        int width = map.GetLength(0),
            height = map.GetLength(1);

        // From each position we scan into 8 directions.
        (int X, int Y)[] directions =
        [
            (1, 0), // R
            (1, 1), // DR
            (0, 1), // D
            (-1, 1),// DL
            (-1, 0), // L
            (-1, -1), // LU
            (0, -1), // U
            (1, -1), // RU
        ];

        int wordCount = 0;
        for (var x = 0; x < width; ++x)
        {
            for (var y = 0; y < height; ++y)
            {
                foreach (var dir in directions)
                {
                    if (ScanWord(map, x, y, dir.X, dir.Y))
                    {
                        ++wordCount;
                    }
                }
            }
        }

        return wordCount;
    }

    public object Solve2(string input)
    {
        var lines = input.SplitLines();

        // Need a 2D array.
        var map = ArrayUtils.To2D(input.RemoveAll('\r').RemoveAll('\n').ToCharArray(), lines[0].Length, lines.Length);

        int width = map.GetLength(0),
            height = map.GetLength(1);

        int crossCount = 0;
        for (var x = 0; x < width; ++x)
        {
            for (var y = 0; y < height; ++y)
            {
                // Scan for crosses.
                if (ScanCross(map, x, y))
                {
                    ++crossCount;
                }
            }
        }

        return crossCount;
    }

    /// <summary>
    /// Scans for the word XMAS from the given location towards the given direction.
    /// Returns true if XMAS is spelled from this location, false otherwise.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="startX"></param>
    /// <param name="startY"></param>
    /// <param name="dirX"></param>
    /// <param name="dirY"></param>
    /// <param name="word">Word to search for.</param>
    /// <returns></returns>
    public static bool ScanWord(char[,] map, int startX, int startY, int dirX, int dirY, string word = "XMAS")
    {
        for (int i = 0; i < word.Length; ++i)
        {
            if (!map.TryGet(startX + i * dirX, startY + i * dirY, out var c) || c != word[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Searches for an X-MAS cross: two intersecting diagonal MAS texts
    /// </summary>
    /// <param name="map"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static bool ScanCross(char[,] map, int x, int y)
    {
        // Sample:
        // M.M
        // .A.
        // S.S

        // Check two diagonals in either direction.
        bool downLeft = ScanWord(map, x - 1, y - 1, 1, 1, "MAS")
                        || ScanWord(map, x - 1, y - 1, 1, 1, "SAM");

        bool upRight = ScanWord(map, x - 1, y + 1, 1, -1, "MAS")
                       || ScanWord(map, x - 1, y + 1, 1, -1, "SAM");

        // We have a cross if both diagonals have a MAS in either direction.
        return downLeft && upRight;
    }
}
