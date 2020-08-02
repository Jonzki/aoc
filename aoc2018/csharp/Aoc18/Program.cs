using Aoc18.Problems;
using System;

namespace Aoc18
{
    class Program
    {
        static void Main(string[] args)
        {
            var i = new Input11();
            var p = new Problem11();
            p.Solve(i);

            Console.WriteLine("Enter to exit.");
            Console.ReadLine();
        }
    }
}
