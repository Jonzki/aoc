using System.Diagnostics;

namespace Aoc.Problems.Aoc22;

/// <summary>
/// https://adventofcode.com/2022/day/7
/// </summary>
public class Problem07 : IProblem
{
    public object Solve1(string input)
    {
        var rootDirectory = ParseInput(input);

        var sumOfSizes = rootDirectory
            .GetDirectorySizes()
            .Where(s => s <= 100_000)
            .Sum();

        return sumOfSizes;
    }

    public object Solve2(string input)
    {
        var rootDirectory = ParseInput(input);

        // Total space: 70_000_000
        // Need unused: 30_000_000

        // Find the smallest directory that, if deleted,
        // would free up enough space on the filesystem to run the update.
        var totalSpace = 70_000_000;
        var needUnused = 30_000_000;

        // Sort the sizes smallest to largest.
        var sumOfSizes = rootDirectory
            .GetDirectorySizes()
            .OrderBy(s => s)
            .ToList();

        // Take the last item (root directory)
        var currentlyFree = totalSpace - sumOfSizes.Last();
        var needToFree = needUnused - currentlyFree;

        var smallestToDelete = sumOfSizes.First(s => s >= needToFree);

        return smallestToDelete;
    }

    /// <summary>
    /// Parses the input string into a directory structure.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private Directory ParseInput(string input)
    {
        var lines = input.SplitLines();

        // Set up the root directory.
        var rootDirectory = new Directory("/", null);

        // Track the current directory.
        var currentDirectory = rootDirectory;

        // Process each line one by one.
        foreach (var line in lines)
        {
            if (line.StartsWith("$ cd "))
            {
                var dir = line.Substring("$ cd ".Length);
                if (dir == "/")
                {
                    // We started with the root directory - do nothing.
                }
                else if (dir == "..")
                {
                    // Back out one step.
                    currentDirectory = currentDirectory.ParentDirectory;
                }
                else
                {
                    // Create a subdirectory, then navigate to it.
                    currentDirectory.Subdirectories.Add(new Directory(dir, currentDirectory));
                    currentDirectory = currentDirectory.Subdirectories.Last();
                }
            }
            else if (line.StartsWith("$ ls"))
            {
                // No need to do anything with this line - it just indicates that we are about to receive data.
            }
            else if (line.StartsWith("dir"))
            {
                // Do nothing here - we will add an empty directory if necessary.
            }
            else
            {
                // Must be a file. The first "word" is the file size, the rest is the file name.
                var parts = line.Split(' ');
                currentDirectory.Files.Add(new File
                {
                    FileName = parts[1],
                    Size = long.Parse(parts[0])
                });
            }
        }

        return rootDirectory;
    }

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    class Directory
    {
        public Directory(string name, Directory parent)
        {
            Name = name;
            Subdirectories = new List<Directory>();
            Files = new List<File>();
            ParentDirectory = parent;
        }

        public string Name { get; set; }

        public Directory ParentDirectory { get; set; }

        public List<Directory> Subdirectories { get; set; }

        public List<File> Files { get; set; }

        public long GetSize()
        {
            // Recurse into subdirectories.
            var subDirs = Subdirectories.Sum(d => d.GetSize());
            // Return subdirectory size + direct file sizes.
            return subDirs + Files.Sum(f => f.Size);
        }

        /// <summary>
        /// Returns the full parent directory name.
        /// </summary>
        /// <returns></returns>
        public string GetFullName()
        {
            if (ParentDirectory == null)
            {
                return "/";
            }
            return ParentDirectory.GetFullName() + "/" + Name;
        }

        /// <summary>
        /// Recursively calculates directory sizes.
        /// </summary>
        /// <returns></returns>
        public List<long> GetDirectorySizes()
        {
            var output = new List<long>() { GetSize() };
            foreach (var dir in Subdirectories)
            {
                output.AddRange(dir.GetDirectorySizes());
            }
            return output;
        }

        private string DebuggerDisplay => GetFullName();
    }

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    class File
    {
        public string FileName { get; set; }
        public long Size { get; set; }

        private string DebuggerDisplay => ToString();

        public override string ToString() => $"{FileName} {Size}";
    }


}
