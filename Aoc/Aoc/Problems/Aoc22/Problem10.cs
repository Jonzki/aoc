using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aoc.Utils;

namespace Aoc.Problems.Aoc22;

/// <summary>
/// https://adventofcode.com/2022/day/10
/// </summary>
public class Problem10 : IProblem
{
    public object Solve1(string input)
    {
        // Parse commands.
        var commands = ParseCommands(input);

        // Set up checkpoints.
        var checkpoints = new Dictionary<int, int>
        {
            [20] = 0,
            [60] = 0,
            [100] = 0,
            [140] = 0,
            [180] = 0,
            [220] = 0,
        };

        // Set up a computer.
        var computer = new Computer(checkpoints);

        computer.Execute(commands);

        return checkpoints.Select(kvp => kvp.Key * kvp.Value).Sum();
    }

    public object Solve2(string input)
    {
        return 0;
    }

    private Queue<Command> ParseCommands(string input)
    {
        var commands = new Queue<Command>();
        foreach (var line in input.SplitLines())
        {
            var parts = line.Split(' ');
            var command = parts[0] switch
            {
                "noop" => new Command { Identifier = "noop", Duration = 1, Value = 0 },
                "addx" => new Command { Identifier = "addx", Duration = 2, Value = int.Parse(parts[1]) },
                _ => throw new ArgumentException($"Command '{parts[0]}' is not recognized.")
            };
            commands.Enqueue(command);
        }
        return commands;
    }

    class Computer
    {
        public Computer(Dictionary<int, int> checkpoints)
        {
            Cycle = 0;
            // The CPU has a single register, X, which starts with the value 1.
            RegisterX = 1;
            RegisterXCheckpoints = checkpoints;
        }

        public int Cycle = 0;

        public int RegisterX { get; set; }

        /// <summary>
        /// Checkpoints for register X.
        /// </summary>
        public Dictionary<int, int> RegisterXCheckpoints { get; set; }

        /// <summary>
        /// Executes a command.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="value"></param>
        public void Execute(Queue<Command> commands)
        {
            while (commands.TryDequeue(out var command))
            {
                // Run through the cycles.
                for (var i = 1; i <= command.Duration; ++i)
                {
                    Cycle++;
                    if (RegisterXCheckpoints.ContainsKey(Cycle))
                    {
                        RegisterXCheckpoints[Cycle] = RegisterX;
                    }

                    if (i == command.Duration && command.Identifier == "addx")
                    {
                        RegisterX += command.Value;
                    }
                }
            }
        }
    }

    class Command
    {
        public string Identifier { get; set; }
        public int Value { get; set; }
        public int Duration { get; set; }
    }

}
