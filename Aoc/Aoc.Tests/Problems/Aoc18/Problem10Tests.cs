using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aoc.Problems.Aoc18;

namespace Aoc.Tests.Problems.Aoc18;

[TestClass]
public class Problem10Tests : ProblemTests<Problem10>
{
    [DataTestMethod]
    [DataRow(8317, "10 players; last marble is worth 1618 points")]
    [DataRow(146373, "13 players; last marble is worth 7999 points")]
    [DataRow(2764, "17 players; last marble is worth 1104 points")]
    [DataRow(54718, "21 players; last marble is worth 6111 points")]
    [DataRow(37305, "30 players; last marble is worth 5807 points")]
    public void Part1_SmallInput_Is_Correct(long correctOutput, string input) => RunPart1(correctOutput, input);
}