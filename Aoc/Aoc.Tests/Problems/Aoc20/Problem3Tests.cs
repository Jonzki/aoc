using Aoc.Problems.Aoc20;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Tests.Problems.Aoc20
{
    [TestClass]
    public class Problem3Tests
    {
        [TestMethod]
        public void ParseMap_Returns_Correct_Map()
        {
            // Correct output.
            var correctMap = new char[,] {
                { 'a', 'b' },
                { 'c', 'd' }
            };

            // Build input.
            var input = "ab" + Environment.NewLine + "cd";

            var map = Problem3.ParseMap(input);

            // ParseMap should return the expected map.
            CollectionAssert.AreEqual(correctMap, map);
        }

        [TestMethod]
        public void TravelMap_Counts_Trees()
        {
            var input = string.Join(Environment.NewLine, new[]
            {
                "..##.......",
                "#...#...#..",
                ".#....#..#.",
                "..#.#...#.#",
                ".#...##..#.",
                "..#.##.....",
                ".#.#.#....#",
                ".#........#",
                "#.##...#...",
                "#...##....#",
                ".#..#...#.#"
            });
            var map = Problem3.ParseMap(input);

            var trees = Problem3.TravelMap(map);
            Assert.AreEqual(7, trees);
        }

    }
}
