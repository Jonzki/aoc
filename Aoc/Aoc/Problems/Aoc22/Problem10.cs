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
        var computer = new Computer(new(), commands);

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

    private class Computer
    {
        public Computer(Dictionary<int, int> checkpoints, Queue<Command> commands)
        {
            Cycle = 0;
            // The CPU has a single register, X, which starts with the value 1.
            RegisterX = 1;
            _checkpoints = checkpoints;
            _commands = commands;
        }

        public int Cycle = 0;

        public int RegisterX { get; set; }

        /// <summary>
        /// Checkpoints for register X.
        /// </summary>
        private readonly Dictionary<int, int> _checkpoints;

        private readonly Queue<Command> _commands;

        public bool HasCommands => _commands.Count > 0 || _currentCommand != null;

        private Command? _currentCommand;
        private int _currentCommandDuration = 0;

        public void Tick()
        {
            // Make sure we have a current command.
            if (_currentCommand == null && _commands.TryDequeue(out _currentCommand))
            {
                // Reset the command duration.
                _currentCommandDuration = 0;
            }
            // Exit if no commands left.
            if (_currentCommand == null) return;

            // Increase the tick count.
            Cycle++;
            _currentCommandDuration++;

            if (_checkpoints.ContainsKey(Cycle) == true)
            {
                _checkpoints[Cycle] = RegisterX;
            }

            if (_currentCommandDuration >= _currentCommand.Duration)
            {
                // Run the addx operation.
                if (_currentCommand.Identifier == "addx")
                {
                    RegisterX += _currentCommand.Value;
                }

                // Clear out the current command - next cycle will dequeue a new command.
                _currentCommand = null;
            }
        }
    }

    private class Command
    {
        public required string Identifier { get; init; }
        public int Value { get; init; }
        public int Duration { get; init; }
    }
}
