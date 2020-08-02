using Aoc19.Problems;
using Aoc19.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc19.Tests.Problems
{
    [TestClass]
    public class Problem5Tests
    {
        [DataTestMethod]
        [DataRow("1002,4,3,4,33", "1002,4,3,4,99")]

        public void Part1_SmallInput_Is_Correct(string input, string output)
        {
            var inputArray = input.Split(',').Select(long.Parse).ToArray();
            var outputArray = output.Split(',').Select(long.Parse).ToArray();

            // Run one operation on the array.
            var inputBuffer = new Queue<long>();
            inputBuffer.Enqueue(1);
            var operationResult = IntCodeComputer.DoOperation(ref inputArray, 0, inputBuffer);

            Assert.AreEqual(string.Join(',', output), string.Join(',', inputArray));
        }
    }
}
