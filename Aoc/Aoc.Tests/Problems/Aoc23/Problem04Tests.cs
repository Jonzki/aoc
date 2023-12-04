using Aoc.Problems.Aoc23;

namespace Aoc.Tests.Problems.Aoc23;

[TestClass]
public class Problem04Tests : ProblemTests<Problem04>
{
    private const string SmallInput = @"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11";

    [TestMethod]
    public void Part1_WorksWith_SmallInput()
    {
        RunPart1(13, SmallInput);
    }

    [TestMethod]
    public void Part2_WorksWith_SmallInput()
    {
        RunPart2(30, SmallInput);
    }

    [TestMethod]
    public void CardParsesAndCalculatesCorrectly()
    {
        var card = Problem04.Card.Parse("Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53");

        Assert.IsNotNull(card);
        Assert.AreEqual(1, card.Id);
        Assert.AreEqual(5, card.WinningNumbers.Length);
        Assert.AreEqual(8, card.Numbers.Length);

        var value = card.CalculateValue1();
        Assert.AreEqual(8, value);
    }
}