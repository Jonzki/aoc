using System.Text.RegularExpressions;

namespace Aoc.Utils;

using System.IO;

/// <summary>
/// Helper class for reading problem inputs.
/// </summary>
public static class InputReader
{
    /// <summary>
    /// Returns the problem input for the given year and number.
    /// The input file must be located under the Inputs directory,
    /// and be named "[year].[number](.[modifier]).txt" (number can be zero padded).
    /// </summary>
    /// <param name="year">Problem year</param>
    /// <param name="number">Problem number</param>
    /// <param name="modifier">Optional modifier name for the input, typically "small"</param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException">A File matching the </exception>
    public static string ReadInput(int year, int number, string? modifier = null)
    {
        // Find all input files.
        var allInputs = Directory.GetFiles("Inputs", "*.txt", SearchOption.AllDirectories);

        // Regex pattern for locating file "002024.0012.modifier.txt"
        var pattern = $@"0*{year}\.0*{number}";
        if (modifier != null)
        {
            pattern += $@"\.{modifier}";
        }
        pattern += @"\.txt";

        foreach (var input in allInputs)
        {
            // Grab the last path segment.
            var fileName = input.Substring(input.LastIndexOf("\\"));
            var match = Regex.Match(fileName, pattern);
            if (match.Success)
            {
                return File.ReadAllText(input);
            }
        }

        throw new FileNotFoundException($"No input file found for year {year}, number {number}.");
    }

    public static string[] ParseList(string input, params string[] separators)
    {
        if (separators.Length > 0)
        {
            return input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }
        else
        {
            return input.Split(new string[] { Environment.NewLine, ";" }, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    public static int[] ParseNumberList(string input, params string[] separators)
    {
        return ParseList(input, separators).Select(int.Parse).ToArray();
    }
}
