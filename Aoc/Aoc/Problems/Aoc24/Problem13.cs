namespace Aoc.Problems.Aoc24;

public class Problem13 : IProblem
{
    public object Solve1(string input)
    {
        var machines = ParseMachines(input);

        var results = machines
            .Select(m => m.Play())
            .ToArray();

        return results
            .Where(r => r.PossibleToWin)
            .Sum(r => r.TokenCost);
    }

    private const long Part2PrizeModifier = 10000000000000;

    public object Solve2(string input)
    {
        var machines = ParseMachines(input);
        Console.WriteLine($"{machines.Count} machines.");

        var results = machines
            .Select(m => m.Play2(Part2PrizeModifier, int.MaxValue))
            .ToArray();

        return results
            .Where(r => r.PossibleToWin)
            .Sum(r => r.TokenCost);
    }

    public static List<Machine> ParseMachines(string input)
    {
        var machines = new List<Machine>();

        // First divide by double linebreak:
        var parts = input.Split(Environment.NewLine + Environment.NewLine);

        // Then parse further.
        foreach (var part in parts)
        {
            var lines = part.SplitLines();

            var buttonA = lines[0]
                .RemoveStrings("Button A: ", "X", "Y")
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            var buttonB = lines[1]
                .RemoveStrings("Button B: ", "X", "Y")
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            var prize = lines[2]
                .RemoveStrings("Prize: ", "X=", "Y=")
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            machines.Add(new Machine
            {
                ButtonA = new Point2D(buttonA[0], buttonA[1]),
                ButtonB = new Point2D(buttonB[0], buttonB[1]),
                Prize = new Point2D(prize[0], prize[1])
            });
        }

        return machines;
    }

    public class Machine
    {
        public int CostA { get; } = 3;
        public int CostB { get; } = 1;

        public Point2D ButtonA { get; init; }
        public Point2D ButtonB { get; init; }

        public Point2D Prize { get; init; }

        /// <summary>
        /// Plays the machine for a win.
        /// </summary>
        /// <returns>Boolean indicating whether the machine can be won, and the lowest token cost of doing so.</returns>
        public (bool PossibleToWin, int TokenCost) Play(long prizeModifier = 0, int moveLimit = 100)
        {
            bool possibleToWin = false;
            int lowestCost = int.MaxValue;

            // Very naive approach: just try all options towards the move limit.
            Point2D endPosition;
            for (int a = 0; a <= moveLimit; ++a)
            {
                for (int b = 0; b <= moveLimit; ++b)
                {
                    // Calculate where we end up with.
                    endPosition = new Point2D(
                        a * ButtonA.X + b * ButtonB.X,
                        a * ButtonA.Y + b * ButtonB.Y
                    );

                    if (endPosition.PositionEquals(Prize))
                    {
                        // Found a winner!
                        possibleToWin = true;
                        var cost = CostA * a + CostB * b;
                        if (cost < lowestCost)
                        {
                            lowestCost = cost;
                        }
                    }

                    if (endPosition.X > Prize.X || endPosition.Y > Prize.Y)
                    {
                        break;
                    }
                }
            }

            if (prizeModifier > 0)
            {
                Console.WriteLine($"Machine Result: {possibleToWin} - {lowestCost}");
            }

            return (possibleToWin, lowestCost);
        }

        /// <summary>
        /// Plays the machine for a win.
        /// </summary>
        /// <returns>Boolean indicating whether the machine can be won, and the lowest token cost of doing so.</returns>
        public (bool PossibleToWin, long TokenCost) Play2(long prizeModifier = 0, int moveLimit = 100)
        {
            // Very naive approach: just try all options towards the move limit.
            var xP = Prize.X + prizeModifier;
            var yP = Prize.Y + prizeModifier;

            var xA = ButtonA.X;
            var yA = ButtonA.Y;

            var xB = ButtonB.X;
            var yB = ButtonB.Y;

            // "Just math". Two equations, two unknowns. Solve B first (we want to maximize it),
            // Then calculate A
            long b = (xP * yA - xA * yP) / (xB * yA - xA * yB);
            long a = (yP - b * yB) / yA;

            // Sanity checks:
            bool possibleX = xP == a * xA + b * xB;
            bool possibleY = yP == a * yA + b * yB;

            if (possibleX && possibleY)
            {
                return (true, 3L * a + b);
            }

            return (false, 0);
        }
    }
}
