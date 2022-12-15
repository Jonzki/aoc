﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aoc.Problems.Aoc20;
using Aoc.Utils;

namespace Aoc.Problems.Aoc22;

/// <summary>
/// https://adventofcode.com/2022/day/11
/// </summary>
public class Problem11 : IProblem
{
    public object Solve1(string input)
    {
        var monkeys = Monkey.ParseMany(input);

        // Run 20 rounds.
        for (var round = 0; round < 20; ++round)
        {
            // The monkeys take turns inspecting and throwing items.
            for (var m = 0; m < monkeys.Count; ++m)
            {
                Inspect(monkeys, m, true);
            }
        }

        // Find the two monkeys with the most inspections.
        var mostActive = monkeys.OrderByDescending(m => m.InspectCount).Take(2).ToArray();

        // Multiply these.
        return mostActive[0].InspectCount * mostActive[1].InspectCount;
    }

    public object Solve2(string input)
    {
        var monkeys = Monkey.ParseMany(input);

        // Now we're running 10 000 rounds instead of 20.
        for (var round = 1; round <= 10_000; ++round)
        {
            // The monkeys take turns inspecting and throwing items.
            for (var m = 0; m < monkeys.Count; ++m)
            {
                Inspect(monkeys, m, false);
            }

            if (round == 1 || round == 20 || round % 1000 == 0)
            {
                PrintInspects(monkeys, round);
            }
        }

        // Find the two monkeys with the most inspections.
        var mostActive = monkeys.OrderByDescending(m => m.InspectCount).Take(2).ToArray();

        // Same multiplication.
        return mostActive[0].InspectCount * mostActive[1].InspectCount;
    }

    /// <summary>
    /// Runs an inspection for the given Monkey.
    /// </summary>
    /// <param name="monkeys"></param>
    /// <param name="index"></param>
    /// <param name="divide">If true, applies the part1 division (by 3).</param>
    public void Inspect(List<Monkey> monkeys, int index, bool divide)
    {
        var monkey = monkeys[index];

        // On a single monkey's turn, it inspects and throws all of the items
        // it is holding one at a time and in the order listed.
        for (var i = 0; i < monkey.Items.Count; i++)
        {
            // Apply inspect function.
            // Test: can we focus on only the most significant numbers?
            monkey.Items[i] = monkey.InspectItem(monkey.Items[i]);

            if (divide)
            {
                // Divide by 3.
                monkey.Items[i] /= 3;
            }

            // Check which monkey to throw to.
            var throwTo = monkey.ThrowItem(monkey.Items[i]);

            // Actually throw.
            monkeys[throwTo].Items.Add(monkey.Items[i]);
        }

        // Since we throw all items, we can clear the items last from this Monkey.
        monkey.Items = new List<long>();
    }

    private void PrintInspects(List<Monkey> monkeys, int round)
    {
        Debug.WriteLine($"== After round {round}");
        foreach (var m in monkeys)
        {
            Debug.WriteLine($"Monkey {m.Id} inspected items {m.InspectCount} times.");
        }
    }

    public class Monkey
    {
        public int Id { get; set; }

        public List<long> Items { get; set; } = new();

        public char Operator { get; set; }

        /// <summary>
        /// Left operand. Null = "old"
        /// </summary>
        public long? OperatorLeft { get; set; }

        /// <summary>
        /// Right operand. Null = "old"
        /// </summary>
        public long? OperatorRight { get; set; }

        public int TestValue { get; set; }

        public int TrueMonkey { get; set; }

        public int FalseMonkey { get; set; }

        // Total amount of inspections.
        public long InspectCount { get; set; }

        public static Monkey Parse(string input)
        {
            // Simplify the text with some tactical replacements,
            // then split into lines.
            var lines = input
                .Replace(":", "")
                .Split('\n', StringSplitOptions.TrimEntries);

            var monkey = new Monkey();

            // ID from first line.
            monkey.Id = int.Parse(lines[0].Split(' ').Last());

            // Items from second line.
            var items = lines[1]
                .Substring("Starting items".Length)
                .Split(',', StringSplitOptions.TrimEntries)
                .Select(long.Parse);
            monkey.Items.AddRange(items);

            // Operation sign & value.
            var op = lines[2].Split("new = ").Last().Split(' ');
            monkey.OperatorLeft = op[0] == "old" ? null : long.Parse(op[0]);
            monkey.Operator = op[1][0];
            monkey.OperatorRight = op[2] == "old" ? null : long.Parse(op[2]);

            // Test value.
            monkey.TestValue = int.Parse(lines[3].Split(' ').Last());

            // True/false monkeys.
            monkey.TrueMonkey = int.Parse(lines[4].Split(' ').Last());
            monkey.FalseMonkey = int.Parse(lines[5].Split(' ').Last());

            return monkey;
        }

        public static List<Monkey> ParseMany(string input)
        {
            var monkeyInputs = input.Split(Environment.NewLine + Environment.NewLine);
            return monkeyInputs.Select(Monkey.Parse).ToList();
        }

        public long InspectItem(long item)
        {
            // Keep track of inspection count.
            InspectCount++;

            var left = OperatorLeft ?? item;
            var right = OperatorRight ?? item;

            return Operator switch
            {
                '+' => left + right,
                '*' => left * right,
                _ => throw new InvalidOperationException($"Unknown operator '{Operator}'.")
            };
        }

        /// <summary>
        /// Uses the <see cref="TestValue"/> to throw the item to another monkey.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Monkey index to throw the item to.</returns>
        public int ThrowItem(long item)
        {
            return (item % TestValue == 0) ? TrueMonkey : FalseMonkey;
        }

    }

}