namespace Aoc.Problems.Aoc19
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class Problem2 : IProblem
    {
        public object Solve1(string input)
        {
            var array = input.Split(',').Select(long.Parse).ToArray();

            // before running the program, 
            // replace position 1 with the value 12 
            // and replace position 2 with the value 2.
            array[1] = 12;
            array[2] = 2;

            var output = Process1(array);

            // What value is left at position 0 after the program halts?
            return output[0];
        }

        public object Solve2(string input)
        {
            var array = input.Split(',').Select(long.Parse).ToArray();

            // Each of the two input values will be between 0 and 99, inclusive.
            // Fill up all possible pairs.
            Console.WriteLine("Filling pairs.");
            var pairs = new ConcurrentBag<(int a, int b)>();
            for (var i = 0; i < 100; ++i)
            {
                for (var j = 0; j < 100; ++j)
                {
                    pairs.Add((i, j));
                }
            }

            // Storage for results.
            var results = new ConcurrentBag<(int a, int b)>();
            var targetZero = 19690720;
            var targetFound = 0;

            // Run each pair in parallel.
            Parallel.ForEach(pairs, pair =>
            {
                if (targetFound > 0) return;

                // Clone the source array.
                var threadArray = new long[array.Length];
                Array.Copy(array, 0, threadArray, 0, threadArray.Length);

                // Switch the inputs.
                threadArray[1] = pair.a;
                threadArray[2] = pair.b;

                var output = Process1(threadArray);

                if (output[0] == targetZero)
                {
                    // Add the current pair into results.
                    results.Add(pair);
                    // Mark result as found - other threads can stop.
                    Interlocked.Increment(ref targetFound);
                }
            });

            if (results.Count == 0) return "RESULT NOT FOUND";
            if (results.Count > 1) return "MULTIPLE RESULTS: " + string.Join(';', results);

            var resultPair = results.First();
            return 100 * resultPair.a + resultPair.b;
        }

        public static long[] Process1(long[] input)
        {
            var program = new Utils.IntCodeComputer(input);
            program.Execute();
            return program.GetExecutionMemory();
        }
    }
}
