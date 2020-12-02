namespace Aoc.Problems.Aoc19
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Problem7 : IProblem
    {
        public object Solve1(string input)
        {
            // Generate the phase arrays.
            // A phase array has numbers [0-4], with none of them occurring twice.
            var phases = new HashSet<string>();
            for (var i1 = 0; i1 < 5; ++i1)
                for (var i2 = 0; i2 < 5; ++i2)
                    for (var i3 = 0; i3 < 5; ++i3)
                        for (var i4 = 0; i4 < 5; ++i4)
                            for (var i5 = 0; i5 < 5; ++i5)
                            {
                                var phase = string.Join(',', i1, i2, i3, i4, i5);
                                if (phase.Distinct().Count() == 6) // 5 number plus comma.
                                {
                                    phases.Add(phase);
                                }
                            }

            var maxSignal = phases.Select(phase => GetThrusterSignal(input, phase)).Max();
            return maxSignal;
        }

        public object Solve2(string input)
        {
            // Generate the phase arrays.
            // A phase array has numbers [0-4], with none of them occurring twice.
            var phases = new HashSet<string>();
            for (var i1 = 5; i1 < 10; ++i1)
                for (var i2 = 5; i2 < 10; ++i2)
                    for (var i3 = 5; i3 < 10; ++i3)
                        for (var i4 = 5; i4 < 10; ++i4)
                            for (var i5 = 5; i5 < 10; ++i5)
                            {
                                var phase = string.Join(',', i1, i2, i3, i4, i5);
                                if (phase.Distinct().Count() == 6) // 5 number plus comma.
                                {
                                    phases.Add(phase);
                                }
                            }

            Console.WriteLine($"Locating max signal from a set of {phases.Count}..");

            var signalBag = new ConcurrentBag<long>();
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 8
            };

            Parallel.ForEach(phases, parallelOptions, phase =>
            {
                signalBag.Add(GetThrusterSignal2(input, phase));
            });

            return signalBag.Max();
        }

        public static long GetThrusterSignal(string program, string phases) => GetThrusterSignal(program, phases.Split(',').Select(long.Parse).ToArray());

        public static long GetThrusterSignal(string program, long[] phases)
        {
            // Run through the process.
            long output = 0;
            for (var i = 0; i < phases.Length; ++i)
            {
                var computer = new Utils.IntCodeComputer(program);

                computer.Execute(phases[i], output);

                output = computer.GetLastOutput() ?? throw new InvalidOperationException("Program produced no output!");
            }

            return output;
        }

        public static long GetThrusterSignal2(string program, string phaseInput)
        {
            long[] phases = phaseInput.Split(',').Select(long.Parse).ToArray();

            // Set up computers for each phase.
            var computers = new Utils.IntCodeComputer[phases.Length];
            for (var i = 0; i < phases.Length; ++i)
            {
                computers[i] = new Utils.IntCodeComputer(program);
                computers[i].Execute(phases[i]);
            }

            const long maxIterations = 1_000_000;

            // Start with an input of 0.
            long output = 0;
            for (var i = 0; i < maxIterations; ++i)
            {
                // Run through each computer.
                for (var c = 0; c < computers.Length; ++c)
                {
                    var computer = computers[c];

                    var computerState = computer.Execute(output);

                    if (computer.Output.Count > 0)
                    {
                        output = computer.Output.Last();
                    }

                    if (computerState == Utils.IntCodeComputer.ExecutionState.WaitingForInput)
                    {
                        continue;
                    }
                    if (computerState == Utils.IntCodeComputer.ExecutionState.Halted && i == phases.Length - 1)
                    {
                        // Last computer halted - break out.
                        break;
                    }
                }
            }

            return output;
        }
    }
}