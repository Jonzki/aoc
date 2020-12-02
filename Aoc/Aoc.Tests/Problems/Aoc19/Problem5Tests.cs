using Aoc.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Aoc.Tests.Problems.Aoc19
{
    [TestClass]
    public class Problem5Tests
    {
        [DataTestMethod]
        [DataRow("1002,4,3,4,33", "1002,4,3,4,99")]
        public void Part1_SmallInput_Is_Correct(string input, string correctOutput)
        {
            var inputArray = input.Split(',').Select(long.Parse).ToArray();
            var correctOutputArray = correctOutput.Split(',').Select(long.Parse).ToArray();

            var computer = new IntCodeComputer(inputArray);

            // Run with an input of 1.
            var state = computer.Execute(1);

            // This computer should have halted.
            Assert.AreEqual(IntCodeComputer.ExecutionState.Halted, state);

            // Output should be whats' expected.
            CollectionAssert.AreEqual(correctOutputArray, computer.GetExecutionMemory());
        }
    }
}