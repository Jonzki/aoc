using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Tests.Problems
{
    /// <summary>
    /// Base class for defining small input tests for parts 1 and 2 of a Problem.
    /// </summary>
    /// <typeparam name="TProblem"></typeparam>
    [TestClass]
    public abstract class ProblemTests<TProblem> where TProblem : IProblem, new()
    {
        protected abstract string SmallInput { get; }

        protected abstract object CorrectOutput1 { get; }

        protected abstract object CorrectOutput2 { get; }

        protected void RunPart1(object correctOutput, string input)
        {
            var output = new TProblem().Solve1(input);
            Assert.AreEqual(correctOutput, output);
        }

        protected void RunPart2(object correctOutput, string input)
        {
            var output = new TProblem().Solve2(input);
            Assert.AreEqual(correctOutput, output);
        }
    }
}
