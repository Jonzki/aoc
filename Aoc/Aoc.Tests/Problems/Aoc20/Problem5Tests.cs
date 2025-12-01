using Aoc.Problems.Aoc20;

namespace Aoc.Tests.Problems.Aoc20;

[TestClass]
public class Problem5Tests
{
    [DataTestMethod]
    [DataRow("FBFBBFFRLR", 44, 5, 357)]
    [DataRow("BFFFBBFRRR", 70, 7, 567)]
    [DataRow("FFFBBBFRRR", 14, 7, 119)]
    [DataRow("BBFFBBFRLL", 102, 4, 820)]
    public void ParseBoardingPass_WorksFor_SmallInput(object input, object correctRow, object correctCol, object correctSeatId)
    {
        var pass = Problem5.ParseBoardingPass((string)input);

        Assert.AreEqual(correctRow, pass.Row);
        Assert.AreEqual(correctCol, pass.Column);
        Assert.AreEqual(correctSeatId, pass.SeatId);
    }
}