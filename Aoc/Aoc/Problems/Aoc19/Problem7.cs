namespace Aoc.Problems.Aoc19
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

            var maxSignal = phases.Select(phase => GetThrusterSignal2(input, phase)).Max();
            return maxSignal;
        }

        public static long GetThrusterSignal(string program, string phases) => GetThrusterSignal(program, phases.Split(',').Select(long.Parse).ToArray());

        public static long GetThrusterSignal(string program, long[] phases)
        {
            // Run through the process.
            long output = 0;
            for (var i = 0; i < phases.Length; ++i)
            {
                var computer = new Utils.IntCodeComputer(program);
                output = computer.Execute(phases[i], output) ?? throw new InvalidOperationException($"Phase {i} did not provide output.");
            }

            return output;
        }

        public static long GetThrusterSignal2(string program, string phases) => GetThrusterSignal2(program, phases.Split(',').Select(long.Parse).ToArray());

        public static long GetThrusterSignal2(string program, long[] phases)
        {
            // TODO
            return -1;
        }
    }
}