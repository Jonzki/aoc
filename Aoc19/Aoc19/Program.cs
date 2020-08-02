using Aoc19.Utils;
using System;
using System.IO;
using System.Reflection;

namespace Aoc19
{
    class Program
    {
        static void Main(string[] args)
        {
            IProblem problem = null;
            string problemInput = null;

            var today = DateTime.Today.Day;

            while (problem == null)
            {
                Console.Write($"Problem number ({today}): ");
                if (!int.TryParse(Console.ReadLine(), out var problemNumber))
                {
                    Console.WriteLine($"Using current date: {today}.");
                    problemNumber = today;
                }
                // Attempt to resolve a Problem class.
                try
                {
                    var type = Type.GetType($"Aoc19.Problems.Problem{problemNumber}");
                    if (type != null)
                    {
                        problem = Activator.CreateInstance(type) as IProblem;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create instantiate solver for Problem {problemNumber}:");
                    Console.WriteLine(ex.Message);
                    problem = null;
                }

                Console.WriteLine("Getting input.");
                try
                {
                    problemInput = ProblemInput.ReadInput(problemNumber);
                }
                catch (FileNotFoundException f)
                {
                    Console.WriteLine($"Failed to get input for problem {problemNumber}: " + f.Message);
                    problem = null;
                }
            }

            Console.WriteLine($"{problem.GetType().Name} -> Solving part 1..");

            var result1 = problem.Solve1(problemInput);
            Console.WriteLine($"Result 1: " + (result1 ?? "NULL"));

            Console.WriteLine(new string('-', 20));
            Console.WriteLine($"{problem.GetType().Name} -> Solving part 2..");

            var result2 = problem.Solve2(problemInput);
            Console.WriteLine($"Result 2: " + (result2 ?? "NULL"));
        }
    }
}
