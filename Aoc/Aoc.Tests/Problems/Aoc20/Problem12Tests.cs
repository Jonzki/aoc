using Aoc.Problems.Aoc20;
using Aoc.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aoc.Tests.Problems.Aoc20
{
    [TestClass]
    public class Problem12Tests
    {
        [TestMethod]
        public void Part1_SmallInput_Works()
        {
            (string Command, int X, int Y)[] testCases = {
                ("F10",10, 0),
                ("N3",10,3),
                ("F7", 17, 3),
                ("R90", 17,3),
                ("F11", 17, -8)
            };

            var ship = new Problem12.Ship();

            foreach (var testCase in testCases)
            {
                ship.Move1(testCase.Command);
                Assert.AreEqual((testCase.X, testCase.Y), ship.Position);
            }

            var distance = NumberUtils.ManhattanDistance(ship.Position, (0, 0));
            Assert.AreEqual(25, distance);
        }

        [TestMethod]
        public void Part2_SmallInput_Works()
        {
            var ship = new Problem12.Ship();

            // F10 moves the ship to the waypoint 10 times (a total of 100 units east and 10 units north), 
            // leaving the ship at east 100, north 10. 
            ship.Move2("F10");
            Assert.AreEqual((100, 10), ship.Position);
            // The waypoint stays 10 units east and 1 unit north of the ship.
            Assert.AreEqual((10, 1), ship.Waypoint);

            // N3 moves the waypoint 3 units north to 10 units east and 4 units north of the ship.
            ship.Move2("N3");
            Assert.AreEqual((10, 4), ship.Waypoint);
            // The ship remains at east 100, north 10.
            Assert.AreEqual((100, 10), ship.Position);

            // F7 moves the ship to the waypoint 7 times (a total of 70 units east and 28 units north), 
            // leaving the ship at east 170, north 38. 
            ship.Move2("F7");
            Assert.AreEqual((170, 38), ship.Position);
            // The waypoint stays 10 units east and 4 units north of the ship.
            Assert.AreEqual((10, 4), ship.Waypoint);

            // R90 rotates the waypoint around the ship clockwise 90 degrees, 
            // moving it to 4 units east and 10 units south of the ship.
            // The ship remains at east 170, north 38.
            ship.Move2("R90");
            Assert.AreEqual((4, -10), ship.Waypoint);
            Assert.AreEqual((170, 38), ship.Position);

            // F11 moves the ship to the waypoint 11 times (a total of 44 units east and 110 units south), 
            // leaving the ship at east 214, south 72. 
            // The waypoint stays 4 units east and 10 units south of the ship.
            ship.Move2("F11");
            Assert.AreEqual((214, -72), ship.Position);
            Assert.AreEqual((4, -10), ship.Waypoint);

            // After these operations, the ship's Manhattan distance from its starting position is 214 + 72 = 286.
            var distance = NumberUtils.ManhattanDistance(ship.Position, (0, 0));
            Assert.AreEqual(286, distance);
        }
    }
}