using Aoc.Problems.Aoc20;

namespace Aoc.Tests.Problems.Aoc20;

[TestClass]
public class Problem7Tests
{
    [DataTestMethod]
    [DataRow("light red bags contain 1 bright white bag, 2 muted yellow bags.")]
    [DataRow("dark orange bags contain 3 bright white bags, 4 muted yellow bags.")]
    [DataRow("bright white bags contain 1 shiny gold bag.")]
    [DataRow("muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.")]
    [DataRow("shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.")]
    [DataRow("dark olive bags contain 3 faded blue bags, 4 dotted black bags.")]
    [DataRow("vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.")]
    [DataRow("faded blue bags contain no other bags.")]
    [DataRow("dotted black bags contain no other bags.")]
    public void ParseBagRule_WorksFor_SmallInput(string input)
    {
        var bagRule = Problem7.ParseBagRule(input);

        Assert.AreEqual(input, bagRule.ToString());
    }
}