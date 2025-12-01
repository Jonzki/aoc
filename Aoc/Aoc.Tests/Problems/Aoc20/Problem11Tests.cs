using Aoc.Problems.Aoc20;
using System.Linq;

namespace Aoc.Tests.Problems.Aoc20;

[TestClass]
public class Problem11Tests
{
    [TestMethod]
    public void Part1_SmallInput_Works()
    {
        // Start with the test input.
        var map = "L.LL.LL.LLLLLLLLL.LLL.L.L..L..LLLL.LL.LLL.LL.LL.LLL.LLLLL.LL..L.L.....LLLLLLLLLLL.LLLLLL.LL.LLLLL.LL".Select(c => c).ToArray();

        var steps = new[]
        {
            "#.##.##.#########.###.#.#..#..####.##.###.##.##.###.#####.##..#.#.....###########.######.##.#####.##",
            "#.LL.L#.###LLLLLL.L#L.L.L..L..#LLL.LL.L##.LL.LL.LL#.LLLL#.##..L.L.....#LLLLLLLL##.LLLLLL.L#.#LLLL.##",
            "#.##.L#.###L###LL.L#L.#.#..#..#L##.##.L##.##.LL.LL#.###L#.##..#.#.....#L######L##.LL###L.L#.#L###.##",
            "#.#L.L#.###LLL#LL.L#L.L.L..#..#LLL.##.L##.LL.LL.LL#.LL#L#.##..L.L.....#L#LLLL#L##.LLLLLL.L#.#L#L#.##",
            "#.#L.L#.###LLL#LL.L#L.#.L..#..#L##.##.L##.#L.LL.LL#.#L#L#.##..L.L.....#L#L##L#L##.LLLLLL.L#.#L#L#.##",
            // Stabilized after the last step.
            "#.#L.L#.###LLL#LL.L#L.#.L..#..#L##.##.L##.#L.LL.LL#.#L#L#.##..L.L.....#L#L##L#L##.LLLLLL.L#.#L#L#.##"
        };

        for (var i = 0; i < steps.Length; ++i)
        {
            // Apply one round.
            (map, _) = Problem11.SimulateMap1(map, 10, 10);

            Assert.AreEqual(steps[i], string.Join("", map), $"Correct after step {i + 1}.");
        }

        Assert.AreEqual(37, map.Count(c => c == '#'), "37 seats should be occupied after stabilizing.");
    }

    [DataTestMethod]
    [DataRow("L.LL.LL.LL\nLLLLLLL.LL\nL.L.L..L..\nLLLL.LL.LL\nL.LL.LL.LL\nL.LLLLL.LL\n..L.L.....\nLLLLLLLLLL\nL.LLLLLL.L\nL.LLLLL.LL", 26)]
    public void Part2_SmallInput_Works(object input, object correctResult)
    {
        var problem = new Problem11();

        var result = problem.Solve2((string)input);

        Assert.AreEqual((int)correctResult, result);
    }
}