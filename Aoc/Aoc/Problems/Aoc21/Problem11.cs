using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aoc.Utils;

namespace Aoc.Problems.Aoc21;

public class Problem11 : IProblem
{
    public object Solve1(string input)
    {
        var (map, width, height) = ParseInput(input);

        long flashes = 0;
        for (var i = 0; i < 100; ++i)
        {
            //Print(map, width, height);
            //Console.WriteLine(new string('-', 10));
            flashes += Simulate(map, width, height);
        }

        return flashes;
    }

    public object Solve2(string input)
    {
        var (map, width, height) = ParseInput(input);

        // Run until we get 100 flashes at once.
        var i = 1;
        for (; i <= 1_000_000; ++i)
        {
            var flashes = Simulate(map, width, height);
            if (flashes == 100)
            {
                Console.WriteLine($"Synchronization at step {i}.");
                return i;
            }
        }

        return -1;
    }

    public (int[] Map, int Width, int Height) ParseInput(string input)
    {
        var lines = input.SplitLines();
        var w = lines[0].Length;

        return (lines.SelectMany(l => l.Select(c => c - '0')).ToArray(), w, lines.Length);
    }

    private void Print(int[] map, int width, int height)
    {
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var i = ArrayUtils.GetIndex(x, y, width);
                Console.Write(map[i]);
            }
            Console.WriteLine("");
        }
    }

    /// <summary>
    /// Executes one simulation round on the map.
    /// </summary>
    /// <param name="map"></param>
    /// <returns>Amount of flashes on this simulation.</returns>
    public static long Simulate(int[] map, int width, int height)
    {
        long flashes = 0;

        // First, the energy level of each octopus increases by 1.
        for (var i = 0; i < map.Length; ++i) map[i]++;

        bool hadFlash;
        do
        {
            hadFlash = false;
            // Then, any octopus with an energy level greater than 9 flashes.
            for (var i = 0; i < map.Length; ++i)
            {
                if (map[i] > 9)
                {
                    // Negate to indicate a flash.
                    map[i] *= -1;
                    hadFlash = true;
                    ++flashes;

                    // This increases the energy level of all adjacent octopuses by 1, including octopuses that are diagonally adjacent.
                    var surr = GetSurroundings(i, width, height);
                    foreach (var s in surr)
                    {
                        map[s]++;
                    }
                }
            }
            // This process continues as long as new octopuses keep having their energy level increased beyond 9.
        } while (hadFlash);

        // Finally, any octopus that flashed during this step has its energy level set to 0.
        for (var i = 0; i < map.Length; ++i)
        {
            if (map[i] > 9 || map[i] < 0) map[i] = 0;
        }

        return flashes;
    }

    /// <summary>
    /// Returns surrounding indices based on width and height.
    /// </summary>
    /// <param name="i"></param>
    /// <param name="w"></param>
    /// <param name="h"></param>
    /// <returns></returns>
    static int[] GetSurroundings(int i, int w, int h)
    {
        var (x, y) = ArrayUtils.GetCoordinates(w, h, i);

        var surroundings = new (int x, int y)[]
        {
            (x-1, y-1),
            (x-1, y),
            (x-1, y+1),
            (x, y-1),
            (x, y+1),
            (x+1, y-1),
            (x+1, y),
            (x+1, y+1)
        };

        return surroundings
            .Where(c => 0 <= c.x && c.x < w && 0 <= c.y && c.y < h)
            .Select(c => ArrayUtils.GetIndex(c.x, c.y, w))
            .ToArray();
    }


}
