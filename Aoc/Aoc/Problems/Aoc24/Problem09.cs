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
        Console.WriteLine($"Disk length at start: {disk.Length}.");

        // <10 seconds for full input.
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
        // Locate the files. Reverse order by position (process last file first).
        var files = ParseFiles(disk).OrderByDescending(f => f.Position);
        foreach (var file in files)
        {
            // Try locating an empty position for the file.
            for (var i = 0; i < file.Position; ++i)
            {
                // Check if we have a free block anywhere.
                bool isFree = true;
                for (var j = 0; j < file.Size; ++j)
                {
                    isFree = isFree && disk[i + j] == EmptyBlock;
                }

                if (isFree)
                {
                    // Write the file data to the new position, and clear the old one.
                    for (var j = 0; j < file.Size; ++j)
                    {
                        disk[i + j] = disk[file.Position + j];
                        disk[file.Position + j] = EmptyBlock;
                    }

                    // Stop processing this file.
                    break;
                }
            }
        }
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

    public static List<File> ParseFiles(int[] disk)
    {
        // Instead of operating with blocks, form a set of Files with the starting position, length and ID.
        var files = new List<File>();

        File f = new File { Id = EmptyBlock };
        for (int i = 0; i < disk.Length; ++i)
        {
            if (disk[i] != f.Id)
            {
                // Found a new ID.
                // If our file was not the empty file, push to output.
                if (f.Id != EmptyBlock)
                {
                    // Save file size and add to output.
                    f.Size = i - f.Position;
                    files.Add(f);
                }

                // Reset the file.
                f = new File
                {
                    Id = disk[i],
                    Position = i,
                    Size = 0 // Temporary size.
                };
            }
        }

        // If we have a file in the buffer, remember to add it.
        if (f.Id != EmptyBlock)
        {
            f.Size = disk.Length - f.Position;
            files.Add(f);
        }

        return files;
    }

    /// <summary>
    /// "Writes" the Files into the disk.
    /// This completely overwrites the disk.
    /// </summary>
    /// <param name="disk"></param>
    /// <param name="files"></param>
    public static void FilesToDisk(int[] disk, List<File> files)
    {
        var emptyFile = new File { Id = EmptyBlock };

        // Clear the disk first.
        Array.Fill(disk, EmptyBlock);

        // Write each file.
        foreach (var file in files)
        {
            FileToDisk(disk, file);
        }
    }

    public static void FileToDisk(int[] disk, File file)
    {
        for (var i = 0; i < file.Size; ++i)
        {
            disk[file.Position + i] = file.Id;
        }
    }

    private const int EmptyBlock = -1;

    public struct File
    {
        public int Id { get; init; }
        public int Position { get; set; }
        public int Size { get; set; }
    }
}
