using Aoc.Problems.Aoc19;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aoc.Tests.Problems.Aoc19
{
    [TestClass]
    public class Problem3Tests
    {
        [DataTestMethod]
        [DataRow("R8,U5,L5,D3", "U7,R6,D4,L4", 6)]
        [DataRow("R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83", 159)]
        [DataRow("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7", 135)]
        public void Part1_SmallInput_Is_Correct(string path1, string path2, int distance)
        {
            Assert.AreEqual(distance, Problem3.GetDistance(path1, path2));
        }

        [DataTestMethod]
        [DataRow("R8,U5,L5,D3", "U7,R6,D4,L4", 30)]
        [DataRow("R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83", 610)]
        [DataRow("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7", 410)]
        public void Part2_SmallInput_Is_Correct(string path1, string path2, int distance)
        {
            Assert.AreEqual(distance, Problem3.GetLeastSteps(path1, path2));
        }
    }
}