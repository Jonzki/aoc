using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aoc.Problems.Aoc18;

namespace Aoc.Tests.Problems.Aoc18;

[TestClass]
public class Problem11Tests : ProblemTests<Problem11>
{
    [DataTestMethod]
    [DataRow("33,45", "18")]
    [DataRow("21,61", "42")]
    public void Part1_SmallInput_Is_Correct(string correctOutput, string input) => RunPart1(correctOutput, input);

    [DataTestMethod]
    [DataRow("90,269,16", "18")]
    [DataRow("232,251,12", "42")]
    public void Part2_SmallInput_Is_Correct(string correctOutput, string input) => RunPart2(correctOutput, input);
}