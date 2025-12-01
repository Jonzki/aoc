using System.ComponentModel.Design;

namespace Aoc.Problems.Aoc25;

public class Problem01 : IProblem
{
    public object Solve1(string input)
    {
        var actions = ParseDialActions(input);

        var dial = new Dial();

        // For part 1: The actual password is the number of times the dial is left pointing at 0 after any rotation in the sequence.
        int zeroCount = 0;

        foreach (var action in actions)
        {
            dial.Rotate(action);
            if (dial.Position == 0)
            {
                ++zeroCount;
            }
        }

        return zeroCount;
    }

    public object Solve2(string input)
    {
        var actions = ParseDialActions(input);

        var dial = new Dial();

        // For part 2: The actual password is the number of times the dial is left pointing at 0 after any rotation in the sequence.
        int clicks = 0;

        foreach (var action in actions)
        {
            clicks += dial.Rotate(action);
        }

        return clicks;
    }

    public class Dial(int position = 50)
    {
        public int Position { get; private set; } = position;

        public const int Steps = 100;

        /// <summary>
        /// Rotates the Dial by input amount. Returns the amount of zeroes visited (clicks for part 2).
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public int Rotate(int amount)
        {
            if (amount == 0)
            {
                return 0;
            }

            int clicks = 0;
            int sign = Math.Sign(amount);

            for (var i = 0; i < Math.Abs(amount); ++i)
            {
                Position += sign;

                if (Position < 0)
                {
                    Position += Steps;
                }
                if (Position >= Steps)
                {
                    Position -= Steps;
                }

                if (Position == 0)
                {
                    ++clicks;
                }
            }

            return clicks;
        }
    }

    private List<int> ParseDialActions(string input)
    {
        var actions = input.SplitLines().Select(x =>
        {
            if (x.StartsWith('L'))
            {
                return -int.Parse(x.Substring(1));
            }

            if (x.StartsWith('R'))
            {
                return int.Parse(x.Substring(1));
            }

            throw new ArgumentException($"Unexpected format for input line: '{x}'");
        }).ToList();

        return actions;
    }
}
