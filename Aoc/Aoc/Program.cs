namespace Aoc
{
    using Aoc.Utils;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class Program
    {
        private const string CacheFileName = "./input-cache.txt";

        private static void Main(string[] args)
        {
            var problemTypes = GetProblemTypes();

            IProblem problem = null;
            string problemInput = null;

            while (problem == null)
            {
                var (year, problemNumber) = ReadProblemInput();

                // Attempt to resolve a Problem class.
                try
                {
                    var type = problemTypes.FirstOrDefault(x => x.Year == year && x.Number == problemNumber).Type;
                    if (type != null)
                    {
                        problem = Activator.CreateInstance(type) as IProblem;
                    }
                    else
                    {
                        Console.WriteLine($"No solver found for {year}.{problemNumber} - not implemented?");
                        problem = null;
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create solver for Problem {problemNumber}:");
                    Console.WriteLine(ex.Message);
                    problem = null;
                }

                // Solver found & constructed - cache the input.
                SaveInputSuggestion(year, problemNumber);

                try
                {
                    problemInput = ProblemInput.ReadInput(year, problemNumber);
                }
                catch (FileNotFoundException f)
                {
                    Console.WriteLine($"Failed to get input for problem {year}.{problemNumber}: " + f.Message);
                    problem = null;
                }
            }

            Console.WriteLine($"{problem.GetType().Name} -> Solving part 1..");

            try
            {
                var result1 = problem.Solve1(problemInput);
                Console.WriteLine($"Result 1: " + (result1 ?? "NULL"));
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("Part 1 is not implemented.");
            }


            Console.WriteLine(new string('-', 20));
            Console.WriteLine($"{problem.GetType().Name} -> Solving part 2..");

            try
            {
                var result2 = problem.Solve2(problemInput);
                Console.WriteLine($"Result 2: " + (result2 ?? "NULL"));
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("Part 2 is not implemented.");
            }

            Console.WriteLine("Enter to exit.");
            Console.ReadLine();
        }

        /// <summary>
        /// Scans for implemented problem solvers.
        /// </summary>
        /// <returns></returns>
        private static List<(int Year, int Number, Type Type)> GetProblemTypes()
        {
            var problems = new List<(int Year, int Number, Type Type)>();

            var problemTypes = typeof(Program).Assembly.GetTypes().Where(t => t.IsClass && t.GetInterface(nameof(IProblem)) != null).ToList();
            Console.WriteLine($"{problemTypes.Count} problem types found.");
            foreach (var type in problemTypes)
            {
                // Eg. Aoc.Problems.Aoc20.Problem1.
                var ns = type.Namespace.Split('.');

                // Year can be dug from Aoc20, index 2.
                var year = int.Parse(ns[2].Replace("Aoc", "")) + 2000;
                var num = int.Parse(type.Name.Replace("Problem", ""));

                //Console.WriteLine($"> Found {year}.{num}");
                problems.Add((year, num, type));
            }

            return problems;
        }

        /// <summary>
        /// Reads requested problem year and number to solve.
        /// </summary>
        /// <returns></returns>
        private static (int Year, int Number) ReadProblemInput()
        {
            while (true)
            {
                var suggestion = GetInputSuggestion();
                int year = suggestion.Year, problemNumber = suggestion.Number;

                Console.Write($"Problem number ({year}.{problemNumber}): ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    return (year, problemNumber);
                }

                var parts = input.Split(' ', '.', ',', ':');
                if (parts.Length == 1 && int.TryParse(parts[0], out problemNumber))
                {
                    return (year, problemNumber);
                }
                else if (parts.Length == 2)
                {
                    // Both inputs given.
                    if (int.TryParse(parts[0], out year) && int.TryParse(parts[1], out problemNumber))
                    {
                        return (year, problemNumber);
                    }
                    // Only latter input given - use suggestion year but override the problem number.
                    if (string.IsNullOrWhiteSpace(parts[0]) && int.TryParse(parts[1], out problemNumber))
                    {
                        return (suggestion.Year, problemNumber);
                    }
                }
                Console.WriteLine("Invalid input.");
            }
        }

        private static (int Year, int Number) GetInputSuggestion()
        {
            // Check if we have a cache file.
            int year = 0, problemNumber = 0;
            if (File.Exists(CacheFileName))
            {
                try
                {
                    var parts = File.ReadAllText(CacheFileName).Split(';');

                    year = int.Parse(parts[0]);
                    problemNumber = int.Parse(parts[1]);

                    var savedAt = new DateTime(long.Parse(parts[2]));

                    if (DateTime.Now.AddMinutes(-5) < savedAt)
                    {
                        // Found cached (and non-expired) suggestion.
                        return (year, problemNumber);
                    }
                }
                catch { }// Don't care about the cache failing - just return today's problem.
            }

            return (DateTime.Today.Year, DateTime.Today.Day);
        }

        /// <summary>
        /// Caches the input problem as a suggestion for next launch.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="number"></param>
        private static void SaveInputSuggestion(int year, int number)
        {
            File.WriteAllText(CacheFileName, string.Join(";", year, number, DateTime.Now.Ticks));
        }
    }
}