namespace Aoc19.Utils
{
    using System;
    using System.IO;

    public static class ProblemInput
    {
        public static string ReadInput(int number)
        {
            var fileName = $"Input/input{number}.txt";
            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }
            throw new FileNotFoundException($"Input file {fileName} could not be found.", fileName);
        }

    }
}
