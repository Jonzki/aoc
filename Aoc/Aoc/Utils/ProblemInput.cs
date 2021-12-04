namespace Aoc.Utils
{
    using System;
    using System.IO;
    using System.Linq;

    public static class ProblemInput
    {
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

        public static string[] ParseList(string input)
        {
            return input.Split(new string[] { Environment.NewLine, ";" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static int[] ParseNumberList(string input)
        {
            return ParseList(input).Select(int.Parse).ToArray();
        }
    }
}
