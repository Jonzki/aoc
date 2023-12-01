namespace Aoc.Problems.Aoc20
{
    public class Problem9 : IProblem
    {
        public object Solve1(string input)
        {
            var list = input.SplitLines().Select(long.Parse).ToList();
            return FindFirstInvalid(list).firstInvalid;
        }

        public object Solve2(string input)
        {
            var list = input.SplitLines().Select(long.Parse).ToList();

            var (xmas, firstInvalid) = FindFirstInvalid(list);

            // find a contiguous set of at least two numbers in your list which sum to the invalid number from step 1.
            for (var i = 0; i < xmas.Buffer.Count; ++i)
            {
                long sum = 0;
                for (var j = i; j < xmas.Buffer.Count; ++j)
                {
                    sum += xmas.Buffer[j];
                    if (sum == firstInvalid)
                    {
                        // To find the encryption weakness, add together the smallest and largest number in this contiguous range.
                        // What is the encryption weakness in your XMAS-encrypted list of numbers?
                        var range = list.Skip(i).Take(j - i).OrderBy(x => x).ToArray();

                        return range.First() + range.Last();
                    }
                    if (sum > firstInvalid)
                    {
                        // No point in summing any longer.
                        break;
                    }
                }
            }

            return -1;
        }

        public static (XMAS xmas, long firstInvalid) FindFirstInvalid(List<long> numbers)
        {
            var xmas = new XMAS(numbers.Take(25).ToArray());

            // Add numbers until we encounter an invalid one.
            foreach (var number in numbers.Skip(25))
            {
                // find the first number in the list (after the preamble) which is not the sum of two of the 25 numbers before it. 
                // What is the first number that does not have this property?
                if (!xmas.TryAdd(number))
                {
                    return (xmas, number);
                }
            }

            return (xmas, -1);
        }

        public class XMAS
        {
            public XMAS(long[] preamble)
            {
                PreambleLength = preamble.Length;
                Buffer = new List<long>(preamble);
            }

            public int PreambleLength { get; set; }

            public List<long> Buffer { get; set; }

            /// <summary>
            /// Checks if the input number is valid for inserting into the buffer.
            /// </summary>
            /// <param name="input"></param>
            /// <returns></returns>
            public bool IsValid(long input)
            {
                // Each number you receive should be the sum of any two of the 25 (preamble length) immediately previous numbers.
                var startIndex = Buffer.Count - PreambleLength;

                long sum;
                for (var i = startIndex; i < Buffer.Count; ++i)
                {
                    for (var j = startIndex; j < Buffer.Count; ++j)
                    {
                        if (i == j) continue;

                        sum = Buffer[i] + Buffer[j];
                        if (input == sum)
                        {
                            // Found a sum -> valid.
                            return true;
                        }
                    }
                }

                // No sum found -> not valid.
                return false;
            }

            /// <summary>
            /// Adds the input number to the buffer if it's valid.
            /// </summary>
            /// <param name="input"></param>
            /// <returns>True if number was added to buffer; False otherwise.</returns>
            public bool TryAdd(long input)
            {
                if (IsValid(input))
                {
                    Buffer.Add(input);
                    return true;
                }
                return false;
            }
        }
    }
}
