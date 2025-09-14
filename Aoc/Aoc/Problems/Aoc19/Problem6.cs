namespace Aoc.Problems.Aoc19;

public class Problem6 : IProblem
{
    public object Solve1(string input)
    {
        var array = input.Split(Environment.NewLine);
        var orbitCount = CountOrbits(array);
        return orbitCount;
    }

    public object Solve2(string input)
    {
        var array = input.Split(Environment.NewLine);
        var orbitCount = GetMinimumTransfer(array, "YOU", "SAN");
        return orbitCount;
    }

    /// <summary>
    /// Counts the amount of direct and indirect orbits in the input array.
    /// </summary>
    /// <param name="inputArray"></param>
    /// <returns></returns>
    public static int CountOrbits(string[] inputArray) => GetOrbits(inputArray).Sum(x => x.Count - 1);

    /// <summary>
    /// Returns the minimum number of orbit transfers to make nodeA orbit the same parent as nodeB
    /// </summary>
    /// <param name="inputArray"></param>
    /// <param name="node"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static int GetMinimumTransfer(string[] inputArray, string node, string target)
    {
        // Start by finding all Orbits.
        var orbits = GetOrbits(inputArray);

        // Assume both target nodes are not parents of any other node.
        var nodeOrbit = orbits.FirstOrDefault(o => o.First() == node) ?? throw new InvalidOperationException($"Could not find full orbit for '{node}.'");
        var targetOrbit = orbits.FirstOrDefault(o => o.First() == target) ?? throw new InvalidOperationException($"Could not find full orbit for '{target}.'");

        // Reverse both orbits, remove all common parents.
        nodeOrbit.Reverse();
        targetOrbit.Reverse();
        string? lastParent = null;
        while (nodeOrbit.Count > 0 && targetOrbit.Count > 0)
        {
            if (nodeOrbit.First() == targetOrbit.First())
            {
                lastParent = nodeOrbit.First();
                nodeOrbit.RemoveAt(0);
                targetOrbit.RemoveAt(0);
            }
            else
            {
                // Different parent reached - break.
                break;
            }
        }

        // Join the two paths back together.
        var fullPath = new List<string>();
        nodeOrbit.Reverse();
        fullPath.AddRange(nodeOrbit);
        if (lastParent != null)
        {
            fullPath.Add(lastParent);
        }
        fullPath.AddRange(targetOrbit);

        // Transfer count is the length of the full path minus 3 (2 for nodes itself, 1 to remove the start orbit).
        return fullPath.Count - 3;
    }

    /// <summary>
    /// Parses full orbit paths from the input Map.
    /// </summary>
    /// <param name="inputArray"></param>
    /// <returns></returns>
    public static List<string>[] GetOrbits(string[] inputArray)
    {
        // Construct a lookup of direct orbiters.
        // Id -> Parent
        var parentLookup = new Dictionary<string, string>();
        foreach (var i in inputArray)
        {
            var split = i.Split(')');
            parentLookup.TryAdd(split[1], split[0]);
        }

        // Unwind the parent structure for each orbit.
        // These arrays start from the orbiter and end with COM.
        var outputArray = new List<string>[inputArray.Length];
        for (var i = 0; i < outputArray.Length; ++i)
        {
            var startNode = inputArray[i].Split(')')[1];
            var fullOrbit = ParseOrbit(parentLookup, startNode);
            outputArray[i] = fullOrbit;
        }

        // Amount of orbits is the amount of nodes in the list, minus the orbiter itself.
        return outputArray;
    }

    public static List<string> ParseOrbit(Dictionary<string, string> parentLookup, string node)
    {
        var fullOrbit = new List<string> { node };
        // Find parent ids and add them to the orbit in order until we find the COM.
        while (fullOrbit.Last() != "COM")
        {
            if (parentLookup.TryGetValue(fullOrbit.Last(), out var parent))
            {
                fullOrbit.Add(parent);
            }
            else
            {
                throw new InvalidOperationException($"Failed to get parent for '{fullOrbit.Last()}'.");
            }
        }
        return fullOrbit;
    }
}
