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
    /// The input file must be located under the Inputs directory, and be named "[year].[number].txt".
    /// </summary>
    /// <param name="year">Problem year</param>
    /// <param name="number">Problem number</param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException">A File matching the </exception>
    public static string ReadInput(int year, int number)
    {
        var fileName = $"{year}.{number}.txt";
        var allInputs = Directory.GetFiles("Inputs", "*.txt", SearchOption.AllDirectories);

        var filePath = allInputs.FirstOrDefault(f => f.EndsWith("\\" + fileName));
        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath);
        }
        throw new FileNotFoundException($"Input file {fileName} could not be found.", fileName);
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
