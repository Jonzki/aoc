namespace Aoc.Problems.Aoc20
{
    public class Problem6 : IProblem
    {
        public object Solve1(string input)
        {
            // Parse the groups.
            var groups = input
                .Replace("\r", "")
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(ParseGroup)
                .ToArray();

            // For each group, count the number of questions to which anyone answered "yes".
            var yesCounts = groups.Select(g => g.People.SelectMany(p => p).Distinct().Count());

            // What is the sum of those counts?
            return yesCounts.Sum();
        }

        public object Solve2(string input)
        {
            // Parse the groups.
            var groups = input
                .Replace("\r", "")
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(ParseGroup)
                .ToArray();

            // For each group, count the number of questions to which everyone answered "yes".
            var yesCount = 0;
            foreach (var group in groups)
            {
                // Run through all characters.
                for (var c = 'a'; c <= 'z'; c++)
                {
                    // Check if each person in the group has answered yes = character included in string.
                    if (group.People.All(p => p.Contains(c))) ++yesCount;
                }
            }

            // What is the sum of those counts?
            return yesCount;
        }

        public static Group ParseGroup(string input) => new Group { People = input.Split('\n') };

        public record Group
        {
            /// <summary>
            /// People with the questions they answered "yes" to - each character is a question.
            /// </summary>
            public string[] People { get; set; }
        }
    }
}