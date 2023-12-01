namespace Aoc.Problems.Aoc22;

/// <summary>
/// https://adventofcode.com/2022/day/4
/// </summary>
public class Problem04 : IProblem
{
    public object Solve1(string input)
    {
        // Split the input into lines.
        var lines = input.SplitLines();

        // Looking for fully contained groups.
        var containCount = 0;

        foreach (var line in lines)
        {
            // Check if either group encompasses the other.
            var groups = Group.Parse(line);
            if (groups[0].Contains(groups[1]) || groups[1].Contains(groups[0]))
            {
                containCount++;
            }
        }

        return containCount;
    }

    public object Solve2(string input)
    {
        // Split the input into lines.
        var lines = input.SplitLines();

        // Looking for fully contained groups.
        var overlapCount = 0;

        foreach (var line in lines)
        {
            // Check if either group encompasses the other.
            var groups = Group.Parse(line);
            if (groups[0].Overlaps(groups[1]))
            {
                overlapCount++;
            }
        }

        return overlapCount;
    }

    struct Group
    {
        public int Lower, Upper;

        public Group(int lower, int upper)
        {
            Lower = lower;
            Upper = upper;
        }

        public static Group[] Parse(string input)
        {
            // Split the input by commas and dashes.
            var parts = input.Split(',', '-').Select(int.Parse).ToArray();
            return new[] { new Group(parts[0], parts[1]), new Group(parts[2], parts[3]) };
        }

        /// <summary>
        /// Checks if this group fully encompasses the input group.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Contains(Group other)
        {
            return this.Lower <= other.Lower && this.Upper >= other.Upper;
        }

        /// <summary>
        /// Checks if this group overlaps the other group at all.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Overlaps(Group other)
        {
            // Check if we are completely outside either end.
            if (this.Lower > other.Upper) return false;
            if (this.Upper < other.Lower) return false;

            // If not, there must be some overlap.
            return true;
        }
    }
}
