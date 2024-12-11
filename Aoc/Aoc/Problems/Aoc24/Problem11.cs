namespace Aoc.Problems.Aoc24;

public class Problem11 : IProblem
{
    public object Solve1(string input)
    {
        var stones = ParseStones(input);

        for (var i = 0; i < 25; ++i)
        {
            Blink(stones);
        }

        // How many stones?
        return stones.Count;
    }

    public object Solve2(string input)
    {
        var stones = ParseStones2(input);

        for (var i = 0; i < 75; ++i)
        {
            Blink2(stones);
        }

        // How many stones?
        return stones.Values.Sum();
    }

    public static LinkedList<long> ParseStones(string input)
    {
        return new LinkedList<long>(input.Split(' ').Select(long.Parse));
    }

    public static Dictionary<long, long> ParseStones2(string input)
    {
        var stones = new Dictionary<long, long>();
        foreach (var stone in input.Split(' ').Select(long.Parse))
        {
            if (!stones.TryAdd(stone, 1))
            {
                stones[stone]++;
            }
        }
        return stones;
    }

    /// <summary>
    /// Part 1 solution, simply run a simulation with a linked list.
    /// </summary>
    /// <param name="stones"></param>
    public static void Blink(LinkedList<long> stones)
    {
        if (stones.Count == 0)
        {
            return;
        }

        var stone = stones.First;
        while (stone != null)
        {
            var output = Blink(stone.Value);

            if (output.Right >= 0)
            {
                stones.AddBefore(stone, output.Left);
                stone.Value = output.Right;
            }
            else
            {
                stone.Value = output.Left;
            }

            // Continue iterating.
            // This works because we only add a stone to before the current one.
            stone = stone.Next;
        }
    }

    /// <summary>
    /// Performs the blink operation on each Stone in the dictionary.
    /// Assignment is a bit sneaky, it mentions the strict ordering,
    /// but the order does not in fact affect the total amount of stones in the end.
    /// </summary>
    /// <param name="stones"></param>
    public static void Blink2(Dictionary<long, long> stones)
    {
        var source = stones.ToArray();

        foreach (var stone in source)
        {
            var output = Blink(stone.Key);

            // Increase the amount of target stones.
            if (!stones.TryAdd(output.Left, stone.Value))
            {
                stones[output.Left] += stone.Value;
            }

            if (output.Right >= 0 && !stones.TryAdd(output.Right, stone.Value))
            {
                stones[output.Right] += stone.Value;
            }

            // We transfer all the stones to the target - clear the source.
            stones[stone.Key] -= stone.Value;
            if (stones[stone.Key] == 0)
            {
                stones.Remove(stone.Key);
            }
        }
    }

    /// <summary>
    /// Runs a "blink" on the input Stone.
    /// </summary>
    /// <param name="stone"></param>
    /// <returns>A single stone (Right = -1) or a pair of Stones.</returns>
    public static (long Left, long Right) Blink(long stone)
    {
        // If the stone is 0, replace it with 1.
        if (stone == 0)
        {
            return (1, -1);
        }

        var numDigits = NumDigits(stone);
        if (numDigits % 2 == 0)
        {
            return Split(stone);
        }

        // If none of the other rules apply,
        // the stone is replaced by (stone * 2024).
        return (stone * 2024, -1);
    }

    public static int NumDigits(long stone)
    {
        // Could do fancy Log10 stuff but... eh.
        return stone.ToString().Length;
    }

    public static (long Left, long Right) Split(long number, int numDigits = 0)
    {
        if (numDigits == 0)
        {
            numDigits = NumDigits(number);
        }

        var temp = (long)Math.Pow(10, numDigits / 2);

        // Get an initial value for the left side.
        var left = number / temp;

        // Right side will be input number minus (left * temp).
        long right = number - (left * temp);

        return (left, right);
    }
}
