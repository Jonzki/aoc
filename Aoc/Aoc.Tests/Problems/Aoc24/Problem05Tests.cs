using System.Linq;
using Aoc.Problems.Aoc24;
using Aoc.Utils;

namespace Aoc.Tests.Problems.Aoc24
{
    [TestClass]
    public class Problem05Tests : ProblemTests<Problem05>
    {
        private const string SmallInput = """"
                                          47|53
                                          97|13
                                          97|61
                                          97|47
                                          75|29
                                          61|13
                                          75|53
                                          29|13
                                          97|29
                                          53|29
                                          61|53
                                          97|53
                                          61|29
                                          47|13
                                          75|47
                                          97|75
                                          47|61
                                          75|61
                                          47|29
                                          75|13
                                          53|13

                                          75,47,61,53,29
                                          97,61,53,29,13
                                          75,29,13
                                          75,97,47,61,53
                                          61,13,29
                                          97,13,75,29,47
                                          """";

        [TestMethod]
        public void SolvePart1()
        {
            RunPart1(143, SmallInput);
        }

        [TestMethod]
        public void SolvePart2()
        {
            RunPart2(123, SmallInput);
        }

        [DataTestMethod]
        [DataRow("75,47,61,53,29", true)]
        [DataRow("97,61,53,29,13", true)]
        [DataRow("75,29,13", true)]
        [DataRow("75,97,47,61,53", false)]
        [DataRow("61,13,29", false)]
        [DataRow("97,13,75,29,47", false)]
        public void IsCorrectlyOrdered_WorksFor_SmallInputs(string inputLine, bool correctOutput)
        {
            var (rules, _) = Problem05.ParseInput(SmallInput);

            var inputs = inputLine.Split(',').Select(int.Parse).ToArray();

            Problem05.IsCorrectlyOrdered(inputs, rules).Should().Be(correctOutput);
        }

        [DataTestMethod]
        [DataRow("75,97,47,61,53", "97,75,47,61,53")]
        [DataRow("61,13,29", "61,29,13")]
        [DataRow("97,13,75,29,47", "97,75,47,29,13")]
        public void Reorder_WorksFor_SmallInputs(string inputLine, string correctOutputLine)
        {
            var (rules, _) = Problem05.ParseInput(SmallInput);

            var inputs = inputLine.Split(',').Select(int.Parse).ToArray();
            var correctOutputs = correctOutputLine.Split(',').Select(int.Parse).ToArray();

            var output = Problem05.Reorder(inputs, rules);

            output.Should().BeEquivalentTo(correctOutputs, o => o.WithStrictOrdering());
        }
    }
}