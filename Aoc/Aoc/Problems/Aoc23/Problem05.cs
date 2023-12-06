using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Problems.Aoc23;

public class Problem05 : IProblem
{
    public object Solve1(string input)
    {
        var (seeds, maps) = ParseInput(input);
        Console.WriteLine($"{seeds.Length} seeds to process.");

        var seedNumbers = new ConcurrentBag<(long Seed, long LocationNumber)>();

        foreach (var seed in seeds)
        {
            //Console.Write("Seed " + seed);
            long output = seed;
            for (var m = 0; m < maps.Length; ++m)
            {
                output = maps[m].MapNumber(output);
                //Console.Write($", {maps[m].Destination} {output}");
            }
            //Console.WriteLine("");

            seedNumbers.Add((seed, output));
        }

        return seedNumbers.MinBy(x => x.LocationNumber).LocationNumber;
    }

    public object Solve2(string input)
    {
        var (seeds, maps) = ParseInput(input);

        long totalSeeds = 0;
        long lowestLocation = long.MaxValue, lowestSeed = 0;
        for (var i = 0; i < seeds.Length; i += 2)
        {
            for (var seed = seeds[i]; seed < seeds[i] + seeds[i + 1]; ++seed)
            {
                ++totalSeeds;

                //Console.Write("Seed " + seed);
                long output = seed;
                for (var m = 0; m < maps.Length; ++m)
                {
                    output = maps[m].MapNumber(output);
                    //Console.Write($", {maps[m].Destination} {output}");
                }
                //Console.WriteLine("");

                if (output < lowestLocation)
                {
                    lowestLocation = output;
                    lowestSeed = seed;
                }
            }
        }

        //Console.WriteLine($"Lowest location: {lowestLocation} (seed {lowestSeed}). Processed {totalSeeds} seeds.");

        return lowestLocation;
    }

    public static (long[] Seeds, Map[] Maps) ParseInput(string input)
    {
        // Split main parts by double newline.
        var mainParts = input.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);

        // Seeds should be the first part.
        var seeds = mainParts[0]
            .Substring("seeds: ".Length)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToArray();

        // Each subsequent part is a Map.
        var maps = new Map[mainParts.Length - 1];

        for (var i = 1; i < mainParts.Length; i++)
        {
            var parts = mainParts[i].SplitLines();

            // Grab source and destination from the header line.
            var header = parts[0].Split('-', ' ');

            long[] destination = new long[parts.Length - 1];
            long[] source = new long[parts.Length - 1];
            long[] range = new long[parts.Length - 1];

            for (var j = 1; j < parts.Length; ++j)
            {
                var numbers = parts[j].Split(' ').Select(long.Parse).ToArray();

                destination[j - 1] = numbers[0];
                source[j - 1] = numbers[1];
                range[j - 1] = numbers[2];
            }

            maps[i - 1] = new Map(header[0], header[2], destination, source, range);
        }

        return (seeds, maps);
    }

    public record Map(
        string Source,
        string Destination,
        long[] DestinationRangeStart,
        long[] SourceRangeStart,
        long[] RangeLength)
    {
        public long MapNumber(long number)
        {
            for (var i = 0; i < SourceRangeStart.Length; ++i)
            {
                if (SourceRangeStart[i] <= number && number < SourceRangeStart[i] + RangeLength[i])
                {
                    return number + DestinationRangeStart[i] - SourceRangeStart[i];
                }
            }

            // Any source numbers that aren't mapped correspond to the same destination number
            return number;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{Source}-to-{Destination} map:");
            for (var i = 0; i < SourceRangeStart.Length; ++i)
            {
                sb.AppendLine(string.Join(' ', DestinationRangeStart[i], SourceRangeStart[i], RangeLength[i]));
            }
            return sb.ToString();
        }
    }
}
