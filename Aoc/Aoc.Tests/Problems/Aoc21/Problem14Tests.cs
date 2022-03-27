using Aoc.Problems.Aoc21;
using Aoc.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Tests.Problems.Aoc21;

[TestClass]
public class Problem14Tests : ProblemTests<Problem14>
{
    protected override string SmallInput => @"NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C";

    protected override object CorrectOutput1 => 1588L;

    protected override object CorrectOutput2 => 2188189693529L;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);

    [DataTestMethod]
    [DataRow(1L, "NNCB")]
    [DataRow(5L, "NBCCNBBBCBHCB")]
    [DataRow(7L, "NBBBCNCCNBBNBNBBCHBHHBCHB")]
    public void CalculateScore_Works(long expectedScore, string input)
    {
        var (pairCounts, startChar, endChar) = Problem14.ParsePairCounts(input);
        var score = Problem14.CalculateScore(pairCounts, startChar, endChar);

        Assert.AreEqual(expectedScore, score);
    }

}
