using Aoc19.Problems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Aoc19.Tests.Problems
{
    [TestClass]
    public class Problem2Tests
    {
        [DataTestMethod]
        [DataRow("1,0,0,0,99", "2,0,0,0,99")]
        [DataRow("2,3,0,3,99", "2,3,0,6,99")]
        [DataRow("2,4,4,5,99,0", "2,4,4,5,99,9801")]
        [DataRow("1,1,1,4,99,5,6,0,99", "30,1,1,4,2,5,6,0,99")]
        [DataRow("1,9,10,3,2,3,11,0,99,30,40,50", "3500,9,10,70,2,3,11,0,99,30,40,50")]
        public void Part1_SmallInput_Is_Correct(string input, string output)
        {
            var inputArray = input.Split(',').Select(long.Parse).ToArray();

            var programResult = Problem2.Process1(inputArray);

            Assert.AreEqual(output, string.Join(',', programResult));
        }
    }
}
