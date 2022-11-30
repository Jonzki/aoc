using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aoc.Utils;

namespace Aoc.Problems.Aoc21;

public class Problem6 : IProblem
{
    public object Solve1(string input)
    {
        return Simulate(input, 80);
    }

    public object Solve2(string input)
    {
        return Simulate(input, 256);
    }

    private object Simulate(string input, int daysToRun)
    {
        // Ages for the fish range from 0 to 8. Track the amounts with a simple array.
        var fish = new long[9];

        // Fill in the initial state.
        foreach (var f in InputReader.ParseNumberList(input, ","))
        {
            fish[f]++;
        }

        for (var d = 0; d < daysToRun; d++)
        {
            // Each day each fish ticks down by 1.
            // This means we can rotate the array values left.
            var zeros = fish[0];
            for (int i = 0; i < 8; i++)
            {
                // Move all fish to the left (age)
                fish[i] = fish[i + 1];
            }

            // The fish that would go negative should reset to 6.
            fish[6] += zeros;
            // These fish should also create a new fish at 8.
            // We can assign since earlier we moved all 8 fishes down.
            fish[8] = zeros;
        }

        // Finally after the simulation, we can simply count the total.
        return fish.Sum();
    }
}
