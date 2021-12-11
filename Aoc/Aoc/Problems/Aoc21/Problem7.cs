using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aoc.Utils;

namespace Aoc.Problems.Aoc21;

public class Problem7 : IProblem
{
    public object Solve1(string input)
    {
        var crabs = ProblemInput.ParseNumberList(input, ",");

        // Find minimum & maximum.
        int min = int.MaxValue, max = int.MinValue;
        foreach (var c in crabs)
        {
            if (c < min) { min = c; }
            if (c > max) { max = c; }
        }

        // Try each position.
        var minMoves = int.MaxValue;
        for (var pos = min; pos <= max; pos++)
        {
            var moves = 0;
            foreach (var crab in crabs)
            {
                moves += Math.Abs(crab - pos);
            }
            if (moves < minMoves)
            {
                minMoves = moves;
            }
        }

        return minMoves;
    }

    public object Solve2(string input)
    {
        var crabs = ProblemInput.ParseNumberList(input, ",");

        // Find minimum & maximum.
        int min = int.MaxValue, max = int.MinValue;
        foreach (var c in crabs)
        {
            if (c < min) { min = c; }
            if (c > max) { max = c; }
        }

        // Try each position. Brute force ends up a little slow but not too much so.
        var minMoves = int.MaxValue;
        for (var pos = min; pos <= max; pos++)
        {
            var moves = 0;
            foreach (var crab in crabs)
            {
                var m = Math.Abs(crab - pos);
                if (m > 0)
                {
                    // Fuel cost will be 1+2+3+... per move.
                    for (var i = 1; i <= m; i++) { moves += i; }
                }
            }
            if (moves < minMoves)
            {
                minMoves = moves;
            }
        }

        return minMoves;
    }


}
