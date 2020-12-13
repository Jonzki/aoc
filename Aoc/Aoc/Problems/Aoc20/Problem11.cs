using Aoc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Problems.Aoc20
{
    public class Problem11 : IProblem
    {
        public object Solve1(string input)
        {
            //input = "L.LL.LL.LL\nLLLLLLL.LL\nL.L.L..L..\nLLLL.LL.LL\nL.LL.LL.LL\nL.LLLLL.LL\n..L.L.....\nLLLLLLLLLL\nL.LLLLLL.L\nL.LLLLL.LL";

            var lines = input.SplitLines();

            // Figure out the dimensions.
            var height = lines.Length;
            var width = lines[0].Length;

            // Join back into a 1D array, without the line separators.
            var map = lines.SelectMany(c => c).ToArray();
            int changes = 0;

            for (int i = 0; i < 1_000_000; ++i)
            {
                // Simulate a round.
                (map, changes) = SimulateMap1(map, width, height);

                // If there were no seat changes, stop.
                if (changes == 0) break;
            }

            // How many seats end up occupied?
            return map.Count(c => c == '#');
        }

        public object Solve2(string input)
        {
            //input = "L.LL.LL.LL\nLLLLLLL.LL\nL.L.L..L..\nLLLL.LL.LL\nL.LL.LL.LL\nL.LLLLL.LL\n..L.L.....\nLLLLLLLLLL\nL.LLLLLL.L\nL.LLLLL.LL";

            var lines = input.SplitLines();

            // Figure out the dimensions.
            var height = lines.Length;
            var width = lines[0].Length;

            // Join back into a 1D array, without the line separators.
            var map = lines.SelectMany(c => c).ToArray();
            int changes = 0;

            for (int i = 0; i < 1_000_000; ++i)
            {
                // Simulate a round.
                (map, changes) = SimulateMap2(map, width, height);

                // If there were no seat changes, stop.
                if (changes == 0) break;
            }

            // How many seats end up occupied?
            return map.Count(c => c == '#');
        }

        public static (char[] NewMap, int changes) SimulateMap1(char[] map, int width, int height)
        {
            var output = new char[map.Length];
            int changes = 0;

            // Run through all map positions.
            for (int i = 0; i < map.Length; ++i)
            {
                // Floor (.) never changes; seats don't move, and nobody sits on the floor.
                if (map[i] == '.')
                {
                    output[i] = '.';
                    continue;
                }

                // If a seat is empty (L) and there are no occupied seats adjacent to it, the seat becomes occupied.
                if (map[i] == 'L' && CountAdjacent(map, width, height, i, '#') == 0)
                {
                    output[i] = '#';
                    ++changes;
                }
                // If a seat is occupied (#) and four or more seats adjacent to it are also occupied, the seat becomes empty.
                else if (map[i] == '#' && CountAdjacent(map, width, height, i, '#') >= 4)
                {
                    output[i] = 'L';
                    ++changes;
                }
                // Otherwise, the seat's state does not change.
                else
                {
                    output[i] = map[i];
                }
            }

            return (output, changes);
        }

        public static (char[] NewMap, int changes) SimulateMap2(char[] map, int width, int height)
        {
            var output = new char[map.Length];
            int changes = 0;

            // Run through all map positions.
            for (int i = 0; i < map.Length; ++i)
            {
                // Floor (.) never changes; seats don't move, and nobody sits on the floor.
                if (map[i] == '.')
                {
                    output[i] = '.';
                    continue;
                }

                // If a seat is empty (L) and there are no occupied seats adjacent to it, the seat becomes occupied.
                if (map[i] == 'L' && CountDiagonal(map, width, height, i, '#') == 0)
                {
                    output[i] = '#';
                    ++changes;
                }
                // It now takes five or more visible occupied seats for an occupied seat to become empty
                else if (map[i] == '#' && CountDiagonal(map, width, height, i, '#') >= 5)
                {
                    output[i] = 'L';
                    ++changes;
                }
                // Otherwise, the seat's state does not change.
                else
                {
                    output[i] = map[i];
                }
            }

            return (output, changes);
        }

        /// <summary>
        /// Counts the amount of adjacent target characters on the map.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="index"></param>
        /// <param name="targetChar"></param>
        /// <returns></returns>
        private static int CountAdjacent(char[] map, int width, int height, int index, char targetChar)
        {
            // Convert the position to coordinates.
            var (x, y) = ArrayUtils.GetCoordinates(width, height, index);

            // Find adjacent indices.
            var adjacentCoords = new (int X, int Y)[]
            {
                (x-1, y), // L
                (x+1, y), // R
                (x, y-1), // U
                (x, y+1), // D
                (x-1, y-1), // LU
                (x+1, y-1), // RU
                (x-1, y+1), // 
                (x+1, y+1)
            }.ToArray();

            return adjacentCoords
                // Filter to those actually in the map.
                .Where(c => c.X.BetweenInclusive(0, width - 1) && c.Y.BetweenInclusive(0, height - 1))
                // From these, count the amount of target characters.
                .Count(c => map[ArrayUtils.GetIndex(c.X, c.Y, width)] == targetChar);
        }

        private static int CountDiagonal(char[] map, int width, int height, int index, char targetChar)
        {
            // Form deltas.
            var deltas = new (int dX, int dY)[]
            {
                (1, 0),
                (-1, 0),
                (0, 1),
                (0, -1),
                (-1, 1),
                (1, 1),
                (-1, -1),
                (1, -1)
            };

            // Try find the target character on each diagonal.
            int found = 0;
            foreach (var delta in deltas)
            {
                var (x, y) = ArrayUtils.GetCoordinates(width, height, index);

                // Run until we encounter an invalid position or find a seat.
                while (true)
                {
                    x += delta.dX;
                    y += delta.dY;

                    // Abort if we're out of the map.
                    if (x < 0 || x >= width || y < 0 || y >= height) break;

                    var c = map[ArrayUtils.GetIndex(x, y, width)];

                    // Floor tile - continue.
                    if (c == '.') continue;

                    // Either # or L - check for target char.
                    if (c == targetChar) ++found;

                    // Break in any case (can only "see" the first seat).
                    break;
                }
            }

            return found;
        }

        /// <summary>
        /// Renders the map to the console.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private static void RenderMap(char[] map, int width, int height)
        {
            Console.WriteLine(new string('-', width));
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    Console.Write(map[y * height + x]);
                }
                Console.WriteLine("");
            }
        }
    }
}
