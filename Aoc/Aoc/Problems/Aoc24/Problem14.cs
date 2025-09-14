namespace Aoc.Problems.Aoc24;

public class Problem14 : IProblem
{
    // Predict the motion of the robots in your list within a space
    // which is 101 tiles wide and 103 tiles tall.

    public int Width = 101;
    public int Height = 103;

    public object Solve1(string input)
    {
        var robots = input.SplitLines().Select(ParseRobot).ToArray();

        // Draw initial position.
        if (Width < 20)
        {
            Console.WriteLine("Initial:");
            Draw(Width, Height, robots);
        }

        foreach (var robot in robots)
        {
            robot.Simulate(100, Width, Height);
        }

        if (Width < 20)
        {
            Console.WriteLine("After 100 seconds:");
            Draw(Width, Height, robots);
        }

        var safetyFactor = SafetyFactor(Width, Height, robots);

        return safetyFactor;
    }

    public object Solve2(string input)
    {
        var robots = input.SplitLines().Select(ParseRobot).ToArray();

        Console.WriteLine("Left and right arrow to modify time. Enter to begin:");
        Console.ReadLine();

        int second = 0;
        while (true)
        {
            Console.WriteLine($"SECOND: {second}");
            Draw(Width, Height, robots);

            var key = Console.ReadKey(true);

            int dir = 0;
            if (key.Key == ConsoleKey.LeftArrow)
            {
                dir = -1;
            }
            else if (key.Key == ConsoleKey.RightArrow)
            {
                dir = 1;
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                Console.WriteLine("Scanning for a possible tree..");
                // Scan for a possible tree.
                for (int i = 0; i < 100_000; ++i)
                {
                    second++;
                    SimulateAll(Width, Height, second, robots);

                    if (IsPossibleTree(Width, Height, robots))
                    {
                        Console.WriteLine($"Possible tree at second {second}");
                        break;
                    }
                }
                continue;
            }
            else
            {
                break;
            }

            second += dir;
            SimulateAll(Width, Height, second, robots);
        }

        return 0;
    }

    public static Robot ParseRobot(string input)
    {
        var parts = input.Split('=', ',', ' ');
        var robot = new Robot()
        {
            InitialPosition = new Point2D(int.Parse(parts[1]), int.Parse(parts[2])),
            Velocity = new Point2D(int.Parse(parts[4]), int.Parse(parts[5]))
        };
        robot.Position = robot.InitialPosition.Clone();
        return robot;
    }

    public class Robot
    {
        public Point2D InitialPosition { get; set; }

        public Point2D Position { get; set; }

        public Point2D Velocity { get; set; }

        public void Simulate(int seconds, int width, int height)
        {
            // Calculate our position based on initial position, velocity and duration.
            Position = InitialPosition.Clone() + Velocity * seconds;

            // Apply map rollover with a remainder.
            var newX = Position.X % width;
            if (newX < 0)
            {
                newX += width;
            }

            var newY = Position.Y % height;
            if (newY < 0)
            {
                newY += height;
            }

            Position = new Point2D(newX, newY);
        }
    }

    public static void SimulateAll(int width, int height, int seconds, Robot[] robots)
    {
        foreach (var robot in robots)
        {
            robot.Simulate(seconds, width, height);
        }
    }

    public static void Draw(int width, int height, Robot[] robots)
    {
        const int divider = 10;

        for (var y = 0; y < height; ++y)
        {
            for (var x = 0; x < width; ++x)
            {
                // Count the number of robots in each position:
                var count = robots.Count(r => r.Position.PositionEquals(x, y));

                if (count > 0)
                {
                    Console.Write("X");
                }
                else
                {
                    if (y < height / divider && x < width / divider)
                    {
                        Console.Write(" ");
                    }
                    else if (y < height / divider && x > (divider - 1) * width / divider)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
            }

            Console.WriteLine();
        }
    }

    public static int SafetyFactor(int width, int height, Robot[] robots)
    {
        var midX = width / 2;
        var midY = height / 2;

        // Quadrant scores (top left, top right, bottom left, bottom right)
        int[] quadrants = [0, 0, 0, 0];

        foreach (var robot in robots)
        {
            if (robot.Position.Y == midY || robot.Position.X == midX)
            {
                continue;
            }

            if (robot.Position.Y < midY)
            {
                if (robot.Position.X < midX)
                {
                    quadrants[0]++;
                }
                else
                {
                    quadrants[1]++;
                }
            }
            else
            {
                if (robot.Position.X < midX)
                {
                    quadrants[2]++;
                }
                else
                {
                    quadrants[3]++;
                }
            }
        }

        // Multiply the quadrants together.
        return quadrants.Aggregate(1, (acc, x) => acc * x);
    }

    public static bool IsPossibleTree(int width, int height, Robot[] robots)
    {
        // Check the top left and top right thirds of the map.
        // If we don't have any robots in these areas, we have a possible tree.

        var positions = new HashSet<Point2D>(robots.Select(r => r.Position));

        Point2D downLeft = new Point2D(-1, 1);
        Point2D downRight = new Point2D(1, 1);

        // Look for a "cone":
        //    X
        //   XXX
        //  XXXXX

        foreach (var position in positions)
        {
            if (
                // Row 1 below
                positions.Contains(position.Down())
                &&
                positions.Contains(position + downLeft)
                &&
                positions.Contains(position + downRight)
                &&
                // Row 2 below
                positions.Contains(position.Down().Down())
                &&
                positions.Contains(position + downLeft * 2)
                &&
                positions.Contains(position + downRight * 2)
                &&
                positions.Contains(position + (downLeft * 2).Right())
                &&
                positions.Contains(position + (downRight * 2).Left())
            )
            {
                return true;
            }
        }

        return false;
    }
}
