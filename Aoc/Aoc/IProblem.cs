using System.Data.SqlTypes;

namespace Aoc
{
    public interface IProblem
    {
        /// <summary>
        /// Returns a custom input file name to read.
        /// If this returns null, the program will default to returning the standard input.
        /// </summary>
        /// <param name="part">Problem part to solve (1/2)</param>
        /// <returns></returns>
        string? ReadInput() => null;

        object Solve1(string input);

        object Solve2(string input);
    }
}
