﻿namespace Aoc.Tests.Utils
{
    using Aoc.Utils;

    [TestClass]
    public class NumberUtilsTests
    {
        [TestMethod]
        public void RolloverIndex_Handles_Negative()
        {
            var array = new[] { 1, 2, 3, 4 };
            var index = NumberUtils.GetRolloverIndex(-1, array.Length);
            Assert.AreEqual(3, index);
        }

        [TestMethod]
        public void RolloverIndex_Handles_Zero()
        {
            var array = new[] { 1, 2, 3, 4 };
            var index = NumberUtils.GetRolloverIndex(0, array.Length);
            Assert.AreEqual(0, index);
        }

        [TestMethod]
        public void RolloverIndex_Handles_Larger_Than_Array()
        {
            var array = new[] { 1, 2, 3, 4 };
            var index = NumberUtils.GetRolloverIndex(4, array.Length);
            Assert.AreEqual(0, index);
        }

        [TestMethod]
        public void RolloverIndex_DoesNotModify_InRange()
        {
            var array = new[] { 1, 2, 3, 4 };
            var index = NumberUtils.GetRolloverIndex(2, array.Length);
            Assert.AreEqual(2, index);
        }

        [TestMethod]
        public void RolloverIndex_Handles_MuchLarger_Than_Array()
        {
            var array = new[] { 1, 2, 3, 4 };
            var index = NumberUtils.GetRolloverIndex(16, array.Length);
            Assert.AreEqual(0, index);
        }
    }
}