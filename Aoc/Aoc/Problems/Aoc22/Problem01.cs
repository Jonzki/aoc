using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aoc.Utils;

namespace Aoc.Problems.Aoc22;

/// <summary>
/// https://adventofcode.com/2022/day/1
/// </summary>
public class Problem01 : IProblem
{
    internal struct Elf
    {
        public List<int> Foods { get; set; }

        /// <summary>
        /// Returns the total amount of calories in the Foods.
        /// </summary>
        public int TotalCalories => Foods.Sum();
    }

    public object Solve1(string input)
    {
        // Parse the elves, return the largest calorie total.
        var largestCalories = ParseElves(input)
            .OrderByDescending(e => e.TotalCalories)
            .FirstOrDefault()
            .TotalCalories;

        return largestCalories;
    }

    public object Solve2(string input)
    {
        // For part 2, instead of the top calorie count, we need top 3.
        var calories = ParseElves(input)
            .OrderByDescending(e => e.TotalCalories)
            .Take(3)
            .Sum(e => e.TotalCalories);

        return calories;
    }

    /// <summary>
    /// Parses the input string into a list of elves (sets of calories).
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    internal static List<Elf> ParseElves(string input)
    {
        var elves = new List<Elf>();

        // Split the input with double-endline first to get the elves.
        var parts = input.Split(Environment.NewLine + Environment.NewLine);

        foreach (var part in parts)
        {
            // Split each part with a single newline to get the food values.
            var elf = new Elf
            {
                Foods = InputReader.ParseNumberList(part, Environment.NewLine).ToList()
            };
            elves.Add(elf);
        }

        return elves;
    }

}
