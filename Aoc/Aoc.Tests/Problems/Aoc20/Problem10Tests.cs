using Aoc.Problems.Aoc20;

namespace Aoc.Tests.Problems.Aoc20;

[TestClass]
public class Problem10Tests
{
    [DataTestMethod]
    [DataRow("16\n10\n15\n5\n1\n11\n7\n19\n6\n12\n4", 35)]
    [DataRow("28\n33\n18\n42\n31\n14\n46\n20\n48\n47\n24\n23\n49\n45\n19\n38\n39\n11\n1\n32\n25\n35\n8\n17\n7\n9\n4\n2\n34\n10\n3", 220)]
    public void Part1_SmallInput_Works(object input, object correctResult)
    {
        Assert.AreEqual(correctResult, new Problem10().Solve1((string)input));
    }

    [DataTestMethod]
    [DataRow("16\n10\n15\n5\n1\n11\n7\n19\n6\n12\n4", 8L)]
    [DataRow("28\n33\n18\n42\n31\n14\n46\n20\n48\n47\n24\n23\n49\n45\n19\n38\n39\n11\n1\n32\n25\n35\n8\n17\n7\n9\n4\n2\n34\n10\n3", 19208L)]
    public void Part2_SmallInput_Works(object input, object correctResult)
    {
        Assert.AreEqual(correctResult, new Problem10().Solve2((string)input));
    }
}