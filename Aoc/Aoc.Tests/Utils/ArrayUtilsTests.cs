using Aoc.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aoc.Tests.Utils
{
    [TestClass]
    public class ArrayUtilsTests
    {
        [TestMethod]
        public void To2D_Works()
        {
            var input = new[] { 1, 2, 3, 4, 5, 6 };
            var output = ArrayUtils.To2D(input, 3, 2);

            Assert.AreEqual(2, output.GetLength(0));
            Assert.AreEqual(3, output.GetLength(1));

            Assert.AreEqual(1, output[0, 0]);
            Assert.AreEqual(2, output[0, 1]);
            Assert.AreEqual(3, output[0, 2]);
            Assert.AreEqual(4, output[1, 0]);
            Assert.AreEqual(5, output[1, 1]);
            Assert.AreEqual(6, output[1, 2]);
        }

        [TestMethod]
        public void To1D_Works()
        {
            var input = new int[2, 3];
            input[0, 0] = 1;
            input[0, 1] = 2;
            input[0, 2] = 3;
            input[1, 0] = 4;
            input[1, 1] = 5;
            input[1, 2] = 6;

            var output = ArrayUtils.To1D(input);
            var correctOutput = new[] { 1, 2, 3, 4, 5, 6 };

            CollectionAssert.AreEqual(correctOutput, output);
        }

        [TestMethod]
        public void DimensionConversion_Is_Invertable()
        {
            var input = new[] { 1, 2, 3, 4, 5, 6 };

            var output = ArrayUtils.To1D(ArrayUtils.To2D(input, 3, 2));

            CollectionAssert.AreEqual(input, output);
        }
    }
}