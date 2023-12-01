using Aoc.Problems.Aoc22;

namespace Aoc.Tests.Problems.Aoc22;

[TestClass]
public class Problem11Tests : ProblemTests<Problem11>
{
    const string SmallInput = @"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1";

    const long CorrectOutput1 = 10605;

    const long CorrectOutput2 = 2713310158;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);

    /// <summary>
    /// Checks that the method Monkey.Parse parses correctly.
    /// </summary>
    [TestMethod]
    public void ParseMonkey_Is_Correct()
    {
        var input = @"Monkey 9:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3";

        var monkey = Problem11.Monkey.Parse(input);

        Assert.AreEqual(9, monkey.Id);
        CollectionAssert.AreEqual(new int[] { 79, 98 }, monkey.Items);

        Assert.IsNull(monkey.OperatorLeft);
        Assert.AreEqual('*', monkey.Operator);
        Assert.AreEqual(19, monkey.OperatorRight);


        Assert.AreEqual(23, monkey.TestValue);

        Assert.AreEqual(2, monkey.TrueMonkey);
        Assert.AreEqual(3, monkey.FalseMonkey);
    }
}
