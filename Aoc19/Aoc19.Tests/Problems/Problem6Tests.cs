using Aoc19.Problems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Aoc19.Tests.Problems
{
    [TestClass]
    public class Problem6Tests
    {


        [DataTestMethod]
        [DataRow("COM)A", 1)]
        [DataRow("COM)B;B)C", 3)]
        [DataRow("COM)B;B)C;C)D;D)E;E)F;B)G;G)H;D)I;E)J;J)K;K)L", 42)]
        public void Part1_SmallInput_Is_Correct(string orbits, int correctCount)
        {
            var inputArray = orbits.Split(';');
            var orbitCount = Problem6.CountOrbits(inputArray);
            Assert.AreEqual(correctCount, orbitCount);
        }

        [DataTestMethod]
        [DataRow("COM)B;B)C;C)D;D)E;E)F;B)G;G)H;D)I;E)J;J)K;K)L;K)YOU;I)SAN", 4)]
        public void Part2_SmallInput_Is_Correct(string orbits, int correctCount)
        {
            var inputArray = orbits.Split(';');
            var orbitCount = Problem6.GetMinimumTransfer(inputArray, "YOU", "SAN");
            Assert.AreEqual(correctCount, orbitCount);
        }
    }
}
