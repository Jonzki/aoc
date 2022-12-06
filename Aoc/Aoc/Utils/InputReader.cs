namespace Aoc.Utils;

using System;
using System.IO;
using System.Linq;

/// <summary>
/// Helper class for reading problem inputs.
/// </summary>
public static class InputReader
{
    /// <summary>
    /// Returns the problem input for the given year and number.
    /// The input file must be located under the Inputs directory, 
    /// and be named "[year].[number].txt" (number can be zero padded).
    /// </summary>
    /// <param name="year">Problem year</param>
    /// <param name="number">Problem number</param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException">A File matching the </exception>
    public static string ReadInput(int year, int number)
    {
        // Find all input files.
        var allInputs = Directory.GetFiles("Inputs", "*.txt", SearchOption.AllDirectories);

        // Two potential file names (support zero-padding).
        var fileName1 = $"{year}.{number}.txt";
        // Format :00 = zero-padded number of 2 digits (eg. 01).
        var fileName2 = $"{year}.{number:00}.txt";

        var filePath = allInputs.FirstOrDefault(f => f.EndsWith("\\" + fileName1) || f.EndsWith("\\" + fileName2));
        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath);
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
