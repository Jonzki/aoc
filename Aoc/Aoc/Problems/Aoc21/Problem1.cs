using Aoc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Problems.Aoc21;

public class Problem1 : IProblem
{
    public object Solve1(string input)
    {
        var numbers = input.SplitLines().Select(int.Parse).ToArray();
        return FindIncrements(numbers);
    }

    public object Solve2(string input)
    {
        var numbers = input.SplitLines().Select(int.Parse).ToArray();
        return FindWindowIncrements(numbers);
    }

    public static int FindIncrements(int[] array)
    {
        // 0 increments if there is 0 or 1 items.
        if (array.Length <= 1) { return 0; }

        var increments = 0;
        for (var i = 1; i < array.Length; i++)
        {
            if (array[i] > array[i - 1]) ++increments;
        }

        return increments;
    }

    public static int FindWindowIncrements(int[] array)
    {
        var windows = new List<int>();

        for (var i = 2; i < array.Length; ++i)
        {
            windows.Add(array[i] + array[i - 1] + array[i - 2]);
        }

        return FindIncrements(windows.ToArray());
    }
}
