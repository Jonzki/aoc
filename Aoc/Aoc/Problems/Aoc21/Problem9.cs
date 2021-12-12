using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aoc.Utils;

namespace Aoc.Problems.Aoc21;

public class Problem9 : IProblem
{
    public object Solve1(string input)
    {
        var map = ParseInput(input);

        var riskValues = new List<int>();
        for (var y = 0; y < map.Height(); y++)
        {
            for (var x = 0; x < map.Width(); x++)
            {
                //Console.Write(map[y, x]);
                if (IsLowPoint(map, x, y))
                {
                    var val = map.Get(x, y);
                    // The risk level of a low point is 1 plus its height.
                    riskValues.Add(val + 1);
                }
            }
            //Console.WriteLine("");
        }

        return riskValues.Sum();
    }

    public object Solve2(string input)
    {
        var map = ParseInput(input);

        // Find low points first.
        var lowPoints = new List<(int x, int y)>();
        for (var y = 0; y < map.Height(); y++)
        {
            for (var x = 0; x < map.Width(); x++)
            {
                if (IsLowPoint(map, x, y))
                {
                    lowPoints.Add((x, y));
                }
            }
        }

        // Then, calculate the size of each area.
        var sizes = new List<int>();
        foreach (var lowPoint in lowPoints)
        {
            var visited = new HashSet<(int, int)>();
            TrackBasin(map, lowPoint.x, lowPoint.y, visited);
            sizes.Add(visited.Count);
        }

        // What do you get if you multiply together the sizes of the three largest basins?
        var max = sizes.OrderByDescending(x => x).Take(3).ToArray();

        return max[0] * max[1] * max[2];
    }

    private int[,] ParseInput(string input)
    {
        var lines = input.SplitLines();
        var w = lines[0].Length;
        var numbers = lines.SelectMany(l => l.Select(c => c - '0')).ToArray();
        return ArrayUtils.To2D(numbers, w, numbers.Length / w);
    }

    private bool IsLowPoint(int[,] map, int x, int y)
    {
        var val = map.Get(x, y);

        return GetSurroundings(map, x, y).All(x => x > val);
    }

    private List<int> GetSurroundings(int[,] map, int x, int y)
    {
        var output = new List<int>();

        // Left, Right, Top, Bottom
        if (x > 0) output.Add(map.Get(x - 1, y));
        if (x < map.Width() - 1) output.Add(map.Get(x + 1, y));
        if (y > 0) output.Add(map.Get(x, y - 1));
        if (y < map.Height() - 1) output.Add(map.Get(x, y + 1));

        return output;
    }

    /// <summary>
    /// Flood-fills the basin from the starting position.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="visitedPoints"></param>
    private void TrackBasin(int[,] map, int x, int y, HashSet<(int x, int y)> visitedPoints)
    {
        // Recursion end condition.
        if (visitedPoints.Contains((x, y))) return;

        if (map.Get(x, y) == 9) { return; }

        visitedPoints.Add((x, y));

        // Left, Right, Top, Bottom
        if (x > 0) TrackBasin(map, x - 1, y, visitedPoints);
        if (x < map.Width() - 1) TrackBasin(map, x + 1, y, visitedPoints);
        if (y > 0) TrackBasin(map, x, y - 1, visitedPoints);
        if (y < map.Height() - 1) TrackBasin(map, x, y + 1, visitedPoints);
    }

}
