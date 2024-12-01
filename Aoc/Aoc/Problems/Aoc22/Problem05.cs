namespace Aoc.Problems.Aoc22;

public class Problem05 : IProblem
{
    public object Solve1(string input) => SolveInternal(input, true);

    public object Solve2(string input) => SolveInternal(input, false);

    private object SolveInternal(string input, bool part1)
    {
        var (state, instructions) = ParseInput(input);

        // Run through the instructions.
        foreach (var instruction in instructions)
        {
            if (part1)
            {
                state.Move1(instruction);
            }
            else
            {
                state.Move2(instruction);
            }
        }

        // Collect the top items from each stack.
        var sb = new StringBuilder();
        foreach (var stack in state.Stacks)
        {
            sb.Append(stack.Peek());
        }
        return sb.ToString();
    }

    /// <summary>
    /// Parses the input into a State object and a list of Instructions.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static (State State, List<Instruction> Instructions) ParseInput(string input)
    {
        // Replace a bunch of stuff to simplify the input,
        // then split by double endline to separate the stack and the instructions.
        // Start by splitting with a double endline.
        var parts = input
            .RemoveStrings("move", "from", "to")
            .Split(Environment.NewLine + Environment.NewLine);

        // First part is the stack. Find the stack count first and prepare the state.
        var stackLines = parts[0].SplitLines();
        var stackCount = stackLines.Last().Count(char.IsDigit);

        // Reverse-fill the stacks.
        var state = new State(stackCount);
        for (var i = stackLines.Length - 2; i >= 0; --i)
        {
            for (var j = 0; j < stackCount; ++j)
            {
                // Pick the middle letter from the substring.
                var item = stackLines[i].Substring(4 * j, 3)[1];
                // If it's an item and not empty, push into the stack.
                if (item != ' ')
                {
                    state.Stacks[j].Push(item);
                }
            }
        }

        // The instructions are simple - since we removed the command texts,
        // just read the 3 numbers.
        var instructions = new List<Instruction>();
        foreach (var line in parts[1].SplitLines())
        {
            var numbers = InputReader.ParseNumberList(line, " ");
            instructions.Add(new Instruction
            {
                Count = numbers[0],
                From = numbers[1],
                To = numbers[2],
            });
        }

        return (state, instructions);
    }

    public class State
    {
        public State(int stackCount)
        {
            Stacks = new Stack<char>[stackCount];
            for (var i = 0; i < Stacks.Length; ++i)
            {
                Stacks[i] = new Stack<char>();
            }
        }

        public Stack<char>[] Stacks { get; set; }

        /// <summary>
        /// Moves item(s) from one stack to another.
        /// This is the Part 1 solution and moves items one by one.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void Move1(Instruction instruction)
        {
            for (var i = 0; i < instruction.Count; i++)
            {
                Stacks[instruction.To - 1].Push(Stacks[instruction.From - 1].Pop());
            }
        }

        /// <summary>
        /// Moves item(s) from one stack to another.
        /// Part 2 moves the items as a batch, keeping the order.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void Move2(Instruction instruction)
        {
            // Read the items into a temporary list first.
            var temp = new List<char>();
            for (var i = 0; i < instruction.Count; i++)
            {
                temp.Add(Stacks[instruction.From - 1].Pop());
            }
            // Reverse the list, then play them back onto the target stack.
            temp.Reverse();
            foreach (var item in temp)
            {
                Stacks[instruction.To - 1].Push(item);
            }
        }

    }

    public record Instruction
    {
        public int Count;
        public int From;
        public int To;
    }
}
