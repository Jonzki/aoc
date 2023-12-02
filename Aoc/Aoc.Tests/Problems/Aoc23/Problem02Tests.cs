using Aoc.Problems.Aoc23;

namespace Aoc.Tests.Problems.Aoc23;

[TestClass]
public class Problem02Tests : ProblemTests<Problem02>
{
    private const string SmallInput = @"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green";

    [TestMethod]
    public void Solve1Test()
    {
        RunPart1(8, SmallInput);
    }

    [TestMethod]
    public void Solve2Test()
    {
        RunPart2(2286, SmallInput);
    }

    [TestMethod]
    public void Game_ParsesCorrectly()
    {
        var input = "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green";
        var game = Problem02.Game.Parse(input);

        Assert.IsNotNull(game);
        Assert.AreEqual(1, game.Id);

        Assert.AreEqual(3, game.Hints.Count);

        Assert.AreEqual((4, 0, 3), game.Hints[0]);
        Assert.AreEqual((1, 2, 6), game.Hints[1]);
        Assert.AreEqual((0, 2, 0), game.Hints[2]);
    }

    [TestMethod]
    public void LowestPossible_CalculatesCorrectly()
    {
        var input = "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green";
        var game = Problem02.Game.Parse(input);

        Assert.IsNotNull(game);
        Assert.AreEqual((4, 2, 6), game.LowestPossible());
    }
}