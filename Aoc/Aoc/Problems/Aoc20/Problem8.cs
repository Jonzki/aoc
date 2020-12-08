using Aoc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc.Problems.Aoc20
{
    public class Problem8 : IProblem
    {
        public object Solve1(string input)
        {
            // Parse commands.
            var commands = input.SplitLines().Select(ParseCommand).ToArray();
            var console = new GameConsole(commands);

            // Run the console until a loop is detected.
            var result = RunUntilTerminatedOrLoop(console);

            // Immediately before any instruction is executed a second time, what value is in the accumulator?
            return result.Accumulator;
        }

        public object Solve2(string input)
        {
            // Parse all commands.
            var commands = input.SplitLines().Select(ParseCommand).ToArray();

            // Test the initial computer.
            var console = new GameConsole(commands);
            var result = RunUntilTerminatedOrLoop(console);

            if (result.Terminated) return result.Accumulator;

            // Run through all command indices. Flip each encountered nop or jmp one at a time.
            for (var i = 0; i < commands.Length; ++i)
            {
                var originalCode = commands[i].Code + "";
                // Flip "nop" to "jmp" and vice versa.
                if (originalCode == "nop") commands[i].Code = "jmp";
                else if (originalCode == "jmp") commands[i].Code = "nop";
                // If we're not going to modify any codes, we don't need to run this console variant.
                else continue;

                // Try run this computer.
                console = new GameConsole(commands);
                result = RunUntilTerminatedOrLoop(console);

                // If we found a successful termination, return.
                // "What is the value of the accumulator after the program terminates?"
                if (result.Terminated) return result.Accumulator;

                // Otherwise, restore the command for next cycle.
                commands[i].Code = originalCode;
            }

            // None of the variations terminated - there's a bug somewhere..
            return -1;
        }

        /// <summary>
        /// Runs the input Game console until it either terminates or a loop is detected.
        /// </summary>
        /// <param name="console"></param>
        /// <returns>True if terminated, false if loop detected; Accumulator value in both events.</returns>
        public static (bool Terminated, int Accumulator) RunUntilTerminatedOrLoop(GameConsole console)
        {
            // Track visited positions.
            var visitedPositions = new HashSet<int>();

            while (true)
            {
                // Run until we encounter a position we've visited before.
                if (!visitedPositions.Add(console.Position))
                {
                    return (false, console.Accumulator);
                }

                // Run a single step.
                console.RunSingle();

                // If the console position went over the end of the commands memory, terminate.
                if (console.Position >= console.Commands.Length)
                {
                    break;
                }
            }

            // Terminated gracefully - return accumulator value.
            return (true, console.Accumulator);
        }

        public static Command ParseCommand(string input)
        {
            var parts = input.Split(' ', 2);
            return new Command
            {
                Code = parts[0],
                Argument = int.Parse(parts[1])
            };
        }
    }

    public class GameConsole
    {
        public GameConsole(Command[] commands)
        {
            Commands = commands;
            Accumulator = 0;
            Position = 0;
        }

        public int Accumulator { get; set; }

        public int Position { get; set; }

        public Command[] Commands { get; set; }

        /// <summary>
        /// Runs a single command.
        /// </summary>
        public void RunSingle()
        {
            var command = Commands[Position];
            switch (command.Code)
            {
                // acc increases or decreases a single global value called the accumulator by the value given in the argument.
                case "acc":
                    Accumulator += command.Argument;
                    ++Position;
                    break;
                // jmp jumps to a new instruction relative to itself.
                case "jmp":
                    Position = (Position + command.Argument); // TODO: Possible index rollover here.
                    break;
                // nop stands for No OPeration - it does nothing.
                case "nop":
                    ++Position;
                    break;

                default:
                    throw new InvalidOperationException($"Command '{command.Code}' is not recognized.");
            };
        }
    }

    public record Command
    {
        public string Code { get; set; }

        public int Argument { get; set; }
    }
}