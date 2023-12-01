namespace Aoc.Problems.Aoc22;

/// <summary>
/// https://adventofcode.com/2022/day/3
/// </summary>
public class Problem03 : IProblem
{
    public object Solve1(string input)
    {
        // Split the input into lines.
        var lines = input.SplitLines();

        var prioritySum = 0;

        foreach (var line in lines)
        {
            // Split the line into compartments (half and half).
            var part1 = line.Take(line.Length / 2);
            var part2 = line.Skip(line.Length / 2);

            // Find items that exist in both parts.
            var commonItems = part1.Intersect(part2).Distinct();

            // Calculate the priority for each item type and add to the sum.
            foreach (var item in commonItems)
            {
                prioritySum += GetCharacterPriority(item);
            }
        }

        return prioritySum;
    }

    public object Solve2(string input)
    {
        // For part 2, divide the input into groups of 3 lines.
        var groups = input.SplitLines().Chunk(3);

        // We'll be summing up the priorities again.
        var prioritySum = 0;

        // Within each group, find the common character.
        foreach (var group in groups)
        {
            // Checking for 3 items is safe if we assume the input is always divisible by 3.
            if (group.Length != 3)
            {
                throw new InvalidOperationException("Input group must have exactly 3 items.");
            }

            var commonItems = group[0].Intersect(group[1]).Intersect(group[2]).ToList();
            if (commonItems.Count != 1)
            {
                throw new InvalidOperationException($"Group has {commonItems.Count} common items, was expecting 1.");
            }

            prioritySum += GetCharacterPriority(commonItems[0]);
        }

        return prioritySum;
    }

    /// <summary>
    /// Calculates the priority value for the input character.
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public static int GetCharacterPriority(char c)
    {
        if (char.IsLower(c))
        {
            // Lowercase item types a through z have priorities 1 through 26.
            return (c - 'a') + 1;
        }
        else
        {
            // Uppercase item types A through Z have priorities 27 through 52.
            return (c - 'A') + 27;
        }
    }
}
