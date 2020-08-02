using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc18
{
    public abstract class Problem<T>
    {
        protected virtual object Part1(T input) => throw new NotImplementedException();

        protected virtual object Part2(T input) => throw new NotImplementedException();

        public void Solve(T input)
        {
            SolvePart1(input);
            SolvePart2(input);
        }

        public void SolvePart1(T input)
        {
            try
            {
                Console.WriteLine("Solving part 1..");
                var sw = new Stopwatch();
                sw.Start();
                var result = Part1(input);
                sw.Stop();
                Console.WriteLine($"Part 1 result: {result}. Time: {sw.Elapsed}");
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("Part 1 is not implemented.");
            }
        }

        public void SolvePart2(T input)
        {
            try
            {
                Console.WriteLine("Solving part 1..");
                var sw = new Stopwatch();
                sw.Start();
                var result = Part2(input);
                sw.Stop();
                Console.WriteLine($"Part 1 result: {result}. Time: {sw.Elapsed}");
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("Part 2 is not implemented.");
            }
        }

    }
}
