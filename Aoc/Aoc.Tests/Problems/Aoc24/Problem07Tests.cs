using Aoc.Problems.Aoc24;
using System.Collections.Generic;

namespace Aoc.Tests.Problems.Aoc24
{
    [TestClass]
    public class Problem07Tests : ProblemTests<Problem07>
    {
        private const string SmallInput = """
                                          190: 10 19
                                          3267: 81 40 27
                                          83: 17 5
                                          156: 15 6
                                          7290: 6 8 6 15
                                          161011: 16 10 13
                                          192: 17 8 14
                                          21037: 9 7 18 13
                                          292: 11 6 16 20
                                          """;

        [TestMethod]
        public void SolvePart1()
        {
            RunPart1(3749L, SmallInput);
        }

        [TestMethod]
        public void SolvePart2()
        {
            RunPart2(11387L, SmallInput);
        }

        [TestMethod]
        public void OperationPermutations_WorksCorrectly_ForOne()
        {
            // Length of 1 should work:
            var permutations = Problem07.OperationPermutations(1, new List<char>());

            permutations.Should().HaveCount(2);
            permutations.Should().AllSatisfy(p => p.Should().HaveCount(1));

            permutations.Should().Contain(p => p[0] == '*');
            permutations.Should().Contain(p => p[0] == '+');
        }

        [TestMethod]
        public void OperationPermutations_WorksCorrectly_ForTwo()
        {
            // Length of 1 should work:
            var permutations = Problem07.OperationPermutations(2, new List<char>());

            permutations.Should().HaveCount(4);
            permutations.Should().AllSatisfy(p => p.Should().HaveCount(2));

            permutations.Should().Contain(p => new string(p) == "**");
            permutations.Should().Contain(p => new string(p) == "++");
            permutations.Should().Contain(p => new string(p) == "*+");
            permutations.Should().Contain(p => new string(p) == "+*");
        }

        [DataTestMethod]
        [DataRow("190: 10 19", 1)]
        [DataRow("3267: 81 40 27", 2)]
        [DataRow("83: 17 5", 0)]
        [DataRow("156: 15 6", 0)]
        [DataRow("7290: 6 8 6 15", 0)]
        [DataRow("161011: 16 10 13", 0)]
        [DataRow("192: 17 8 14", 0)]
        [DataRow("21037: 9 7 18 13", 0)]
        [DataRow("292: 11 6 16 20", 1)]
        public void ProcessEquation_WorksFor_SmallInputLines(string input, int correctPossibleCount)
        {
            var (_, possibleCount) = Problem07.ProcessEquation(input);
            possibleCount.Should().Be(correctPossibleCount);
        }

        [DataTestMethod]
        [DataRow("156: 15 6", 1)]
        [DataRow("7290: 6 8 6 15", 1)]
        [DataRow("192: 17 8 14", 1)]
        public void ProcessEquation_WorksFor_Concat(string input, int correctPossibleCount)
        {
            var (_, possibleCount) = Problem07.ProcessEquation(input, true);
            possibleCount.Should().Be(correctPossibleCount);
        }

    }
}