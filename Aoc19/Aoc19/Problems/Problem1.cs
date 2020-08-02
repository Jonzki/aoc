using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc19.Problems
{
    public class Problem1 : IProblem
    {
        public object Solve1(string input)
        {
            var masses = input.Split(Environment.NewLine).Select(long.Parse);
            return masses.Sum(CalculateFuel1);
        }

        public object Solve2(string input)
        {
            var masses = input.Split(Environment.NewLine).Select(long.Parse);
            return masses.Sum(CalculateFuel2);
        }

        public static long CalculateFuel1(long mass)
        {
            return (mass / 3) - 2;
        }

        public static long CalculateFuel2(long mass)
        {
            var totalFuel = 0L;
            var currentMass = mass;
            long i = 0;
            long limit = 1_000_000;
            for (; i < limit; ++i)
            {
                var fuel = CalculateFuel1(currentMass);
                if (fuel > 0)
                {
                    totalFuel += fuel;
                    currentMass = fuel;
                }
                else
                {
                    break;
                }
            }
            if (i >= limit)
            {
                Console.WriteLine("ERROR: Loop reached limit.");
                return -1;
            }
            return totalFuel;
        }

    }
}
