using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aoc.Utils;

namespace Aoc.Problems.Aoc21;

public class Problem10 : IProblem
{
    public object Solve1(string input)
    {
        var lines = input.SplitLines();
        return lines.Sum(GetScore);
    }

    public object Solve2(string input)
    {
        var lines = input.SplitLines();
        var scores = lines
            .Select(GetCompletionScore)
            .Where(x => x > 0) // Completed lines.
            .OrderBy(x => x)
            .ToArray();

        // Take the middle score.
        return scores[(int)scores.Length / 2];
    }

    public static int GetScore(string line)
    {
        int i = 0;

        var stack = new Stack<char>();
        string chunkOpen = "([{<";

        for (; i < line.Length; i++)
        {
            // Chunk opening is always allowed.
            if (chunkOpen.Contains(line[i]))
            {
                stack.Push(line[i]);
            }
            else
            {
                var expected = stack.Peek() switch
                {
                    '(' => ')',
                    '[' => ']',
                    '{' => '}',
                    '<' => '>',
                    _ => throw new InvalidOperationException($"Invalid characted '{line[i]}'!")
                };
                if (line[i] != expected)
                {
                    //Console.WriteLine($"{line} - Expected '{expected}', but found '{line[i]} instead. Position {i}.");
                    // Return a score based on the illegal characted.
                    return line[i] switch
                    {
                        ')' => 3,
                        ']' => 57,
                        '}' => 1197,
                        '>' => 25137,
                        _ => throw new InvalidOperationException($"Invalid characted '{line[i]}'!")
                    };
                }
                else
                {
                    // Proceed, pop the chunk from the stack.
                    stack.Pop();
                }
            }
        }

        // Valid or incomplete line: 0 points.
        return 0;
    }

    public static long GetCompletionScore(string line)
    {
        int i = 0;

        var stack = new Stack<char>();
        string chunkOpen = "([{<";

        for (; i < line.Length; i++)
        {
            // Chunk opening is always allowed.
            if (chunkOpen.Contains(line[i]))
            {
                stack.Push(line[i]);
            }
            else
            {
                var expected = stack.Peek() switch
                {
                    '(' => ')',
                    '[' => ']',
                    '{' => '}',
                    '<' => '>',
                    _ => throw new InvalidOperationException($"Invalid characted '{line[i]}'!")
                };
                if (line[i] != expected)
                {
                    // Corrupt line - in part 2 we need to discard this.
                    return -1;
                }
                else
                {
                    // Proceed, pop the chunk from the stack.
                    stack.Pop();
                }
            }
        }

        // Valid line if there is nothing left in the stack.
        if (stack.Count == 0) return -1;

        // Complete the line.
        long completionScore = 0;
        var completion = new StringBuilder();
        while (stack.TryPop(out var c))
        {
            completion.Append(c switch
            {
                '(' => ')',
                '[' => ']',
                '{' => '}',
                '<' => '>',
                _ => throw new InvalidOperationException($"Unexpected character: '{c}'")
            });

            // Update the score.
            completionScore = completionScore * 5 + completion[completion.Length - 1] switch
            {
                ')' => 1,
                ']' => 2,
                '}' => 3,
                '>' => 4,
                _ => throw new InvalidOperationException($"Unexpected character: '{c}'")
            };
        }

        //Console.WriteLine($"{completion} - {completionScore} total points.");

        return completionScore;
    }

}
