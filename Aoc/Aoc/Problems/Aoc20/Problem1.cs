using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc.Problems.Aoc20
{
    public class Problem1 : IProblem
    {
        public object Solve1(string input)
        {
            var numbers = input.Split(Environment.NewLine).Select(int.Parse).ToArray();

            var sumValue = FindSumValue(numbers, 2020, 2);

            return sumValue;
        }

        public object Solve2(string input)
        {
            var numbers = input.Split(Environment.NewLine).Select(int.Parse).ToArray();

            var sumValue = FindSumValue(numbers, 2020, 3);

            return sumValue;

        }

        /// <summary>
        /// Finds two numbers from the input array that sum to targetSum, and return their multiplication.
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="targetSum"></param>
        /// <returns></returns>
        public static int FindSumValue(int[] numbers, int targetSum, int numbersToSum = 2)
        {
            // Sort the numbers first.
            var sorted = numbers.OrderBy(x => x).ToArray();

            int sum = 0;
            // Run two loops, one from the beginning and the other from the end.
            int i = 0, j = 0, k = 0;
            for (; i < sorted.Length; ++i)
            {
                // Reset the sum.
                sum = 0;
                for (j = 0; j < sorted.Length; ++j)
                {
                    if (numbersToSum == 2)
                    {
                        sum = sorted[i] + sorted[j];
                        if (sum == targetSum) break; // Break if we found the target sum.
                        if (sum > targetSum) break; // Break from the inner loop if we went over the target sum.
                    }
                    else if (numbersToSum == 3)
                    {
                        for (k = 0; k < sorted.Length; ++k)
                        {
                            sum = sorted[i] + sorted[j] + sorted[k];
                            if (sum == targetSum) break; // Break if we found the target sum.
                            if (sum > targetSum) break; // Break from the inner loop if we went over the target sum.
                        }
                        if (sum == targetSum) break; // Break if we found the target sum.
                    }
                }
                if (sum == targetSum) break; // Break if we found the target sum.
            }

            if (sum == targetSum)
            {

                return sorted[i] * sorted[j] * (numbersToSum == 2 ? 1 : sorted[k]);
            }
            else
            {
                Console.WriteLine("Error: target sum was not found.");
                return -1;
            }
        }
    }
}
