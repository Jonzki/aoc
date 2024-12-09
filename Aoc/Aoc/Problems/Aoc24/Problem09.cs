namespace Aoc.Problems.Aoc24;

/// <summary>
/// https://adventofcode.com/2024/day/9
/// </summary>
public class Problem09 : IProblem
{
    public object Solve1(string input)
    {
        var disk = ParseInput(input);
        Console.WriteLine($"Disk length at start: {disk.Length}");

        CompressPart1(disk);

        var checksum = Checksum(disk);

        return checksum;
    }

    public object Solve2(string input)
    {
        var disk = ParseInput(input);
        Console.WriteLine($"Disk length at start: {disk.Length}");

        CompressPart2(disk);

        var checksum = Checksum(disk);

        return checksum;
    }

    /// <summary>
    /// Produces a "disk" array from the input string.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static int[] ParseInput(string input)
    {
        var output = new List<int>();

        int id = 0;

        // Input alternates between indicating
        // the length of a file and the length of free space.
        for (var i = 0; i < input.Length; ++i)
        {
            var num = input[i] - '0';

            // Even = file, odd = empty space.
            if (i % 2 == 0)
            {
                for (var n = 0; n < num; ++n)
                {
                    output.Add(id);
                }
            }
            else
            {
                // When encountering empty space, increase the ID.
                if (output.Count > 0 && output.Last() >= 0)
                {
                    id++;
                }
                // -1 = empty.
                for (var n = 0; n < num; ++n)
                {
                    output.Add(-1);
                }
            }
        }

        return output.ToArray();
    }

    /// <summary>
    /// Outputs a string format of the input Disk.
    /// Useful for both tests and visualization.
    /// </summary>
    /// <param name="disk"></param>
    /// <returns></returns>
    public static string DiskToString(int[] disk)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < disk.Length; ++i)
        {
            if (disk[i] >= 0)
            {
                sb.Append(disk[i]);
            }
            else
            {
                sb.Append('.');
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Compresses the input Disk according to Part 1 rules.
    /// </summary>
    /// <param name="disk"></param>
    public static void CompressPart1(int[] disk)
    {
        // Scan from rear and front to relocate blocks.
        int front = 0;
        int rear = disk.Length - 1;

        while (front < rear)
        {
            // Scan the front index until we find an empty space.
            for (; front < disk.Length; ++front)
            {
                if (disk[front] == EmptyBlock) { break; }
            }
            // Scan the rear index (backwards) until we find a block.
            for (; rear >= 0; --rear)
            {
                if (disk[rear] >= 0) { break; }
            }

            if (rear > front)
            {
                // Move the block from rear to front.
                disk[front] = disk[rear];
                disk[rear] = EmptyBlock;
            }
        }
    }

    public static void CompressPart2(int[] disk)
    {
    }

    public static long Checksum(int[] disk)
    {
        long output = 0;

        for (var i = 0; i < disk.Length; ++i)
        {
            if (disk[i] != EmptyBlock)
            {
                output += i * disk[i];
            }
        }

        return output;
    }

    private const int EmptyBlock = -1;
}
