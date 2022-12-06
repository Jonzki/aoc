using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Aoc.Utils;

namespace Aoc.Problems.Aoc18;

public class Problem05 : IProblem
{
    public object Solve1(string input)
    {
        var output = React(input);

        return output.Count;
    }

    public object Solve2(string input)
    {
        // In part 2, we need to run multiple potential reaction chains.
        var alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

        var outputs = new ConcurrentBag<int>();

        // Lazy solution: run each simulation in parallel.
        Parallel.ForEach(alphabet, new ParallelOptions { MaxDegreeOfParallelism = 12 }, c =>
        {
            var sw = Stopwatch.StartNew();

            var modifiedInput = input.RemoveStrings(c.ToString(), c.ToString().ToUpper());

            var modifiedOutput = React(modifiedInput).Count;
            outputs.Add(modifiedOutput);

            sw.Stop();
            Console.WriteLine($"Finished variant {char.ToUpper(c)} in {sw.Elapsed}");
        });

        return outputs.Min();
    }

    private List<char> React(IEnumerable<char> input)
    {
        var output = new List<char>(input);

        var loopCount = 0;
        while (true)
        {
            var removedAnything = false;
            loopCount++;
            for (var i = 0; i < output.Count - 1; ++i)
            {
                if (output[i] == '-') continue;

                var j = i + 1;

                for (; j < output.Count; ++j)
                {
                    if (output[j] != '-') break;
                }

                if (j >= output.Count)
                {
                    // Reached the end of the list.
                    continue;
                }

                var cA = output[i];
                var cB = output[j];

                if (cA != cB && char.ToLower(cA) == char.ToLower(cB))
                {
                    //Debug.WriteLine($"Remove {cA}{cB}");

                    // Both indices should be destroyed.
                    output[i] = '-';
                    output[j] = '-';

                    // Wind the i index to the end point.
                    i = j;

                    removedAnything = true;
                }
            }

            if (!removedAnything) break;

            // Clean up the output every once in a while.
            if (loopCount % 100 == 0)
            {
                output = output.Where(c => c != '-').ToList();
            }
        }

        // Final cleanup before return.
        return output.Where(c => c != '-').ToList();
    }
}
