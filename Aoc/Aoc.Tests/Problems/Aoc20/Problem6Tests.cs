using Aoc.Problems.Aoc20;

namespace Aoc.Tests.Problems.Aoc20;

[TestClass]
public class Problem6Tests
{
    [TestMethod]
    public void Problem6_Part1_SmallInput_Works()
    {
        var group = Problem6.ParseGroup("abc");
        Assert.AreEqual(1, group.People.Length);
        Assert.AreEqual("abc", group.People[0]);

        group = Problem6.ParseGroup("a\nb\nc");
        Assert.AreEqual(3, group.People.Length);
        Assert.AreEqual("a", group.People[0]);
        Assert.AreEqual("b", group.People[1]);
        Assert.AreEqual("c", group.People[2]);

        group = Problem6.ParseGroup("ab\nac");
        Assert.AreEqual(2, group.People.Length);
        Assert.AreEqual("ab", group.People[0]);
        Assert.AreEqual("ac", group.People[1]);
    }
}