using System.Linq;

namespace Aoc19.Problems
{
    public class Problem4 : IProblem
    {
        public object Solve1(string input)
        {
            var range = input.Split('-').Select(long.Parse).ToArray();

            var valid = 0;
            for (var i = range[0]; i <= range[1]; ++i)
            {
                if (IsValidPassword1(i + "")) ++valid;
            }

            return valid;
        }

        public object Solve2(string input)
        {
            var range = input.Split('-').Select(long.Parse).ToArray();

            var valid = 0;
            for (var i = range[0]; i <= range[1]; ++i)
            {
                if (IsValidPassword2(i + "")) ++valid;
            }

            return valid;
        }

        /// <summary>
        /// Checks if the input password is valid.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidPassword1(string input)
        {
            var hasDouble = false;
            for (var i = 1; i < input.Length; ++i)
            {
                // Previous number is bigger - not valid.
                if (input[i - 1] > input[i]) return false;
                // Must have at least one double.
                if (input[i - 1] == input[i]) hasDouble = true;
            }
            return hasDouble;
        }

        /// <summary>
        /// Checks if the input password is valid.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidPassword2(string input)
        {
            // Start double counter from 1 (first character is part of a possible double.
            char targetChar = input[0];
            var doubleCounter = 0;
            var hasDouble = false;

            var i = 0;
            for (; i < input.Length; ++i)
            {
                // Previous number is bigger - not valid.
                if (i > 0 && input[i - 1] > input[i]) return false;

                if (input[i] == targetChar) ++doubleCounter;
                else
                {
                    // Reset the double counter to 1. If counter was at two, set hasDouble.
                    if (doubleCounter == 2) { hasDouble = true; }
                    targetChar = input[i];
                    doubleCounter = 1;
                }
            }
            // Check if we had a double earlier or if the last counter is at 2.
            return hasDouble || doubleCounter == 2;
        }
    }
}