using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Problems.Aoc21;

public class Problem2 : IProblem
{
    public object Solve1(string input)
    {
        var instructions = input.Split(new string[] { Environment.NewLine, ";" }, StringSplitOptions.RemoveEmptyEntries);

        var position = Navigate1(instructions);

        return position.Position * position.Depth;
    }

    public object Solve2(string input)
    {
        var instructions = input.Split(new string[] { Environment.NewLine, ";" }, StringSplitOptions.RemoveEmptyEntries);

        var coordinates = Navigate2(instructions);

        return coordinates.Position * coordinates.Depth;
    }

    public static (int Position, int Depth) Navigate1(IEnumerable<string> instructions)
    {
        int position = 0, depth = 0;

        foreach (var instruction in instructions)
        {
            var (command, value) = ParseInstruction(instruction);

            (position, depth) = command switch
            {
                "forward" => (position + value, depth),
                "up" => (position, depth - value), // Up = negative depth.
                "down" => (position, depth + value), // Down = Positive depth.

                _ => throw new InvalidOperationException($"Unkown command '{command}'.")
            };
        }

        return (position, depth);
    }

    public static (int Position, int Depth) Navigate2(IEnumerable<string> instructions)
    {
        int position = 0, depth = 0, aim = 0;

        foreach (var instruction in instructions)
        {
            var (command, value) = ParseInstruction(instruction);

            if (command == "down")
            {
                // down X increases your aim by X units.
                aim += value;
            }
            else if (command == "up")
            {
                // up X decreases your aim by X units.
                aim -= value;
            }
            else if (command == "forward")
            {
                // forward X does two things:
                position += value; // It increases your horizontal position by X units.
                depth += aim * value;// It increases your depth by your aim multiplied by X.
            }
            else throw new InvalidOperationException($"Unkown command '{command}'.");
        }
        return (position, depth);
    }

    private static (string Command, int Value) ParseInstruction(string instruction)
    {
        var parts = instruction.Split(' ');
        var command = parts[0];
        var value = Math.Abs(int.Parse(parts[1]));
        return (command, value);
    }
}
