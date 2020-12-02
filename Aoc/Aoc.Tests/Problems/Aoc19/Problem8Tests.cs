using Aoc.Problems.Aoc19;
using Aoc.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aoc.Tests.Problems.Aoc19
{
    [TestClass]
    public class Problem8Tests
    {
        [TestMethod]
        public void ParseImage_Parses_Image()
        {
            string input = "123456789012";
            int width = 3, height = 2;

            var image = Problem8.ParseImage(input, width, height);

            Assert.AreEqual(width, image.Width);
            Assert.AreEqual(height, image.Height);
            Assert.AreEqual(2, image.Layers.Count);

            var layer1 = new[] { 1, 2, 3, 4, 5, 6 };
            var layer2 = new[] { 7, 8, 9, 0, 1, 2 };

            CollectionAssert.AreEqual(layer1, image.Layers[0]);
            CollectionAssert.AreEqual(layer2, image.Layers[1]);
        }

        [TestMethod]
        public void MergeLayers_Works_ForSmallInput()
        {
            string input = "0222112222120000";
            var correctOutput = new[] { false, true, true, false };

            var image = Problem8.ParseImage(input, 2, 2);
            var output = Problem8.MergeLayers(image);

            CollectionAssert.AreEqual(correctOutput, output);
        }
    }
}