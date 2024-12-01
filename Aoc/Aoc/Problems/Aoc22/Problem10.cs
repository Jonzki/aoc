namespace Aoc.Problems.Aoc22;

/// <summary>
/// https://adventofcode.com/2022/day/10
/// </summary>
public class Problem10 : IProblem
{
    public object Solve1(string input)
    {
        // Parse commands.
        var commands = ParseCommands(input);

        // Set up checkpoints.
        var checkpoints = new Dictionary<int, int>
        {
            [20] = 0,
            [60] = 0,
            [100] = 0,
            [140] = 0,
            [180] = 0,
            [220] = 0,
        };

        // Set up a computer.
        var computer = new Computer(checkpoints, commands);

        // Run the entire program.
        while (computer.HasCommands)
        {
            computer.Tick();
        }

        return checkpoints.Select(kvp => kvp.Key * kvp.Value).Sum();
    }

    public object Solve2(string input)
    {
        // Parse commands.
        var commands = ParseCommands(input);

        // Set up a computer.
        var computer = new Computer(null, commands);

        // Run the computer for 6 * 40 cycles.
        // This will print something.
        var stringBuilder = new StringBuilder();
        for (var y = 0; y < 6; ++y)
        {
            for (var x = 0; x < 40; ++x)
            {
                // Check if our X position is within 1 from the current X position.
                var draw = Math.Abs(computer.RegisterX - (computer.Cycle % 40)) <= 1;


                // Run a tick.
                computer.Tick();


                // Draw a lit or dark "pixel".
                stringBuilder.Append(draw ? '#' : '.');
            }
            stringBuilder.AppendLine();
        }

        return stringBuilder.ToString().Trim();
    }

    private Queue<Command> ParseCommands(string input)
    {
        var commands = new Queue<Command>();
        foreach (var line in input.SplitLines())
        {
            var parts = line.Split(' ');
            var command = parts[0] switch
            {
                "noop" => new Command { Identifier = "noop", Duration = 1, Value = 0 },
                "addx" => new Command { Identifier = "addx", Duration = 2, Value = int.Parse(parts[1]) },
                _ => throw new ArgumentException($"Command '{parts[0]}' is not recognized.")
            };
            commands.Enqueue(command);
        }
        return commands;
    }

    class Computer
    {
        public Computer(Dictionary<int, int> checkpoints, Queue<Command> commands)
        {
            Cycle = 0;
            // The CPU has a single register, X, which starts with the value 1.
            RegisterX = 1;
            Checkpoints = checkpoints;
            Commands = commands;
        }

        public int Cycle = 0;

        public int RegisterX { get; set; }

        /// <summary>
        /// Checkpoints for register X.
        /// </summary>
        public Dictionary<int, int> Checkpoints { get; set; }
        public Queue<Command> Commands { get; set; }

        public bool HasCommands => Commands.Count > 0 || CurrentCommand != null;

        private Command CurrentCommand;
        private int CurrentCommandDuration = 0;

        public void Tick()
        {
            // Make sure we have a current command.
            if (CurrentCommand == null && Commands.TryDequeue(out CurrentCommand))
            {
                // Reset the command duration.
                CurrentCommandDuration = 0;
            }
            // Exit if no commands left.
            if (CurrentCommand == null) return;

            // Increase the tick count.
            Cycle++;
            CurrentCommandDuration++;

            if (Checkpoints?.ContainsKey(Cycle) == true)
            {
                Checkpoints[Cycle] = RegisterX;
            }

            if (CurrentCommandDuration >= CurrentCommand.Duration)
            {
                // Run the addx operation.
                if (CurrentCommand.Identifier == "addx")
                {
                    RegisterX += CurrentCommand.Value;
                }

                // Clear out the current command - next cycle will dequeue a new command.
                CurrentCommand = null;
            }
        }
    }

    class Command
    {
        public string Identifier { get; set; }
        public int Value { get; set; }
        public int Duration { get; set; }
    }

}
