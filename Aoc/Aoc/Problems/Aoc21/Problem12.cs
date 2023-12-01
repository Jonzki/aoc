namespace Aoc.Problems.Aoc21;

public class Problem12 : IProblem
{
    public object Solve1(string input)
    {
        var map = ParseMap(input);

        var paths = FindPaths(map, new[] { "start" });

        return paths.Count;
    }

    public object Solve2(string input)
    {
        var map = ParseMap(input);

        var paths = FindPaths2(map, new[] { "start" });

        return paths.Count;
    }

    private Dictionary<string, HashSet<string>> ParseMap(string input)
    {
        var map = new Dictionary<string, HashSet<string>>();

        foreach (var pair in input.SplitLines())
        {
            var parts = pair.Split('-');

            // Add routes both ways.
            map.TryAdd(parts[0], new HashSet<string>());
            map.TryAdd(parts[1], new HashSet<string>());

            if (parts[1] != "start") map[parts[0]].Add(parts[1]);
            if (parts[0] != "start") map[parts[1]].Add(parts[0]);
        }
        return map;
    }

    private List<string> FindPaths(Dictionary<string, HashSet<string>> map, string[] visited)
    {
        var output = new List<string>();

        // Find all possible targets.
        var possibleTargets = map[visited.Last()];
        foreach (var target in possibleTargets)
        {
            // Recursion end.
            if (target == "end")
            {
                output.Add(string.Join('-', visited) + '-' + "end");
            }
            else
            {
                if (visited.Contains(target) && target.ToLower() == target)
                {
                    // Lowercase locations cannot be visited twice.
                    continue;
                }
                output.AddRange(FindPaths(map, visited.Append(target).ToArray()));
            }
        }

        return output;
    }

    private List<string> FindPaths2(Dictionary<string, HashSet<string>> map, string[] visited, string revisit = null)
    {
        var output = new List<string>();

        // Find all possible targets.
        var possibleTargets = map[visited.Last()];
        foreach (var target in possibleTargets)
        {
            // Recursion end.
            if (target == "end")
            {
                output.Add(string.Join('-', visited) + '-' + "end");
            }
            else
            {
                if (visited.Contains(target) && target.ToLower() == target)
                {
                    // If revisit key has not been set yet, simulate with it.
                    if (revisit == null)
                    {
                        output.AddRange(FindPaths2(map, visited.Append(target).ToArray(), target));
                    }
                    // Lowercase locations cannot be visited twice.
                    continue;
                }
                output.AddRange(FindPaths2(map, visited.Append(target).ToArray(), revisit));
            }
        }

        return output;
    }
}
