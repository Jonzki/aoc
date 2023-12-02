using System.Collections.Concurrent;

namespace Aoc.Problems.Aoc18;

public class Problem05 : IProblem
{
    public object Solve1(string input)
    {
        var output = React(input.AsSpan());
        return output.Length - output.Count('-');
    }

    public object Solve2(string input)
    {
        var lengths = new ConcurrentBag<int>();

        Parallel.ForEach(CharUtils.Alphabet, (c) =>
        {
            // Remove all occurrences of the character from the input.
            var replacedInput = input
                .RemoveAll(c)
                .RemoveAll(char.ToUpper(c));

            // Run the Part 1 simulation.
            var length = (int)Solve1(replacedInput);
            lengths.Add(length);

            Console.WriteLine(c + " => " + length);
        });

        return lengths.Min();
    }

    public static ReadOnlySpan<char> React(ReadOnlySpan<char> span)
    {
        var output = new Span<char>(new char[span.Length]);
        span.CopyTo(output);

        var limit = int.MaxValue;

        var loop = 0;
        while (true)
        {
            ++loop;
            bool removes = false;

            int i = 0;
            for (; i < Math.Min(output.Length - 1, limit); ++i)
            {
                // Scan for next character.
                int j = i + 1;
                for (; j < output.Length && j < limit && output[j] == '-'; ++j) { }

                if (j == output.Length || j == limit)
                {
                    limit = i + 1; // Everything at the end of the string is a dash - mark the limit.
                    break;
                }

                // Check for the same character in different casing.
                if (output[i] != output[j] && char.ToLower(output[i]) == char.ToLower(output[j]))
                {
                    // Remove both occurrences.
                    output[i] = '-';
                    output[j] = '-';
                    removes = true;

                    // Jump ahead to the j position.
                    i = j;
                }
            }

            if (removes)
            {
                if (loop % 100 == 0)
                {
                    // Trim the output buffer.
                    var buffer = new Span<char>(new char[output.Length - output.Count('-')]);
                    int bi = 0;
                    for (var oi = 0; oi < output.Length; ++oi)
                    {
                        if (output[oi] != '-')
                        {
                            buffer[bi++] = output[oi];
                        }
                    }
                    output = buffer;
                }

                //limit = output.IndexOf('-');
                //if (output.Length - limit > 500)
                //{
                //    Console.WriteLine($"Trim {output.Length} to {limit}");
                //    // Slice to clean up dashes if there are many.
                //    output = output.Slice(0, output.IndexOf('-'));
                //}
            }
            else
            {
                break;
            }
        }

        return output;
    }
}
