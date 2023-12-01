using Aoc.Problems.Aoc20;

namespace Aoc.Tests.Problems.Aoc20
{
    [TestClass]
    public class Problem8Tests
    {
        [DataTestMethod]
        [DataRow("nop +0\nacc +1\njmp +4\nacc +3\njmp -3\nacc -99\nacc +1\njmp -4\nacc +6", 5)]
        public void Solve1_WorksFor_SmallInput(string input, int correctOutput)
        {
            var output = new Problem8().Solve1(input);

            Assert.AreEqual(correctOutput, output);
        }

        [DataTestMethod]
        [DataRow("nop +0\nacc +1\njmp +4\nacc +3\njmp -3\nacc -99\nacc +1\njmp -4\nacc +6", 8)]
        public void Solve2_WorksFor_SmallInput(string input, int correctOutput)
        {
            var output = new Problem8().Solve2(input);

            Assert.AreEqual(correctOutput, output);
        }
    }
}