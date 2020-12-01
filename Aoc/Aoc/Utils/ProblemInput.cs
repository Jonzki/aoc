namespace Aoc.Utils
{
    using System;
    using System.IO;

    public static class ProblemInput
    {
        public static string ReadInput(int year, int number)
        {
            var yearId = "Aoc" + (year - 2000);

            var fileName = $"Input/{yearId}/input{number}.txt";
            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }
            throw new FileNotFoundException($"Input file {fileName} could not be found.", fileName);
        }

    }
}
