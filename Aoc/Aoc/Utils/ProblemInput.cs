namespace Aoc.Utils
{
    using System.IO;

    public static class ProblemInput
    {
        public static string ReadInput(int year, int number)
        {
            var fileName = $"Inputs/{year}.{number}.txt";
            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }
            throw new FileNotFoundException($"Input file {fileName} could not be found.", fileName);
        }
    }
}