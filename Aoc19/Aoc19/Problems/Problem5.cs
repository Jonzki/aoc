namespace Aoc19.Problems
{
    using System.Linq;

    public class Problem5 : IProblem
    {
        public object Solve1(string input)
        {
            var array = input.Split(',').Select(long.Parse).ToArray();

            return Process(array, 1);
        }

        public object Solve2(string input)
        {
            var array = input.Split(',').Select(long.Parse).ToArray();

            return Process(array, 5);
        }

        private object Process(long[] array, params long[] input)
        {
            var program = new Utils.IntCodeComputer(array);
            var result = program.Execute(input);
            return result;
        }
    }
}