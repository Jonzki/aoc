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
        Print(map, width, height);


        return -1;
    }

    public object Solve2(string input)
    {
        throw new NotImplementedException();
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
    int Simulate(int[] map, int width, int height)
    {
        int flashes = 0;

        // First, the energy level of each octopus increases by 1.
        for (var i = 0; i < map.Length; ++i) map[i]++;

        var hadFlash = false;
        do
        {
            // Then, any octopus with an energy level greater than 9 flashes.
            for (var i = 0; i < map.Length; ++i)
            {
                if (map[i] > 9)
                {
                    // Negate to indicate a flash.
                    map[i] *= -1;

                    // This increases the energy level of all adjacent octopuses by 1, including octopuses that are diagonally adjacent.


                }

            }

            // This process continues as long as new octopuses keep having their energy level increased beyond 9.
        } while (hadFlash);




        return flashes;
    }

    /// <summary>
    /// Returns surrounding indices based on width and height.
    /// </summary>
    /// <param name="i"></param>
    /// <param name="w"></param>
    /// <param name="h"></param>
    /// <returns></returns>
    int[] GetSurroundings(int i, int w, int h)
    {
        var (x, y) = ArrayUtils.GetCoordinates(w, h, i);

        //return new[]
        //{
        //    (x-1, y-1),
        //    (x-1, y),
        //    (x-1, y+1),
        //    (x-1, y-1),
        //    (x-1, y-1),
        //    (x-1, y-1),
        //    (x-1, y-1),

        //}

        return Array.Empty<int>();
    }


}
