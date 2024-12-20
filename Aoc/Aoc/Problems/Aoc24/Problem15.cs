namespace Aoc.Problems.Aoc24;

public class Problem15 : IProblem
{
    public object Solve1(string input)
    {
        var (map, commands) = ParseInput(input);

        map.Draw("Initial state:");

        for (var i = 0; i < commands.Count; ++i)
        {
            map.Move(commands[i]);

            if (i < 15)
            {
                map.Draw($"Move {commands[i]}:");
            }
        }

        map.Draw("Final state:");

        // For the result, calculate the sum of GPS coordinate of each box.
        var result = map.Boxes.Sum(boxPosition => 100 * boxPosition.Y + boxPosition.X);

        return result;
    }

    public object Solve2(string input)
    {
        return 0;
    }

    public static (Map Map, List<char> Moves) ParseInput(string input)
    {
        // Separate map and moves.
        var parts = input.Split(Environment.NewLine + Environment.NewLine);

        return (ParseMap(parts[0]), parts[1].Replace(Environment.NewLine, "").ToList());
    }

    public static Map ParseMap(string input)
    {
        var lines = input.SplitLines();

        var height = lines.Length;
        var width = lines[0].Length;

        var map = new Map { Width = width, Height = height, };

        for (var y = 0; y < height; ++y)
        {
            for (var x = 0; x < width; ++x)
            {
                switch (lines[y][x])
                {
                    case '#':
                        map.Walls.Add((x, y));
                        break;

                    case 'O':
                        map.Boxes.Add((x, y));
                        break;

                    case '@':
                        map.Robot = (x, y);
                        break;
                }
            }
        }

        return map;
    }

    public class Map
    {
        public int Width { get; init; }
        public int Height { get; init; }

        public HashSet<Point2D> Walls { get; } = new();

        public HashSet<Point2D> Boxes { get; } = new();

        public Point2D Robot { get; set; }

        public void Move(char dir)
        {
            var target = dir switch
            {
                '^' => Robot.Up(),
                'v' => Robot.Down(),
                '<' => Robot.Left(),
                '>' => Robot.Right(),
                _ => throw new InvalidOperationException($"Unexpected move direction '{dir}'.")
            };

            // If the target position has a wall, we can't move.
            if (Walls.Contains(target))
            {
                return;
            }
            // If the target position has a box, try to move it.
            if (Boxes.Contains(target))
            {
                MoveBox(target, dir);
            }
            // If the target STILL has a box, we can't move there.
            if (Boxes.Contains(target))
            {
                return;
            }
            // Otherwise the position should be free.
            Robot = target;
        }

        public void MoveBox(Point2D position, char dir)
        {
            var target = dir switch
            {
                '^' => position.Up(),
                'v' => position.Down(),
                '<' => position.Left(),
                '>' => position.Right(),
                _ => throw new InvalidOperationException($"Unexpected move direction '{dir}'.")
            };

            // If the target has a wall, we can't move the box.
            if (Walls.Contains(target))
            {
                return;
            }
            // If the target has a box, try move that one first.
            if (Boxes.Contains(target))
            {
                MoveBox(target, dir);
            }
            // If the position is now free from boxes, move the current box.
            if (Boxes.Add(target))
            {
                Boxes.Remove(position);
            }
        }

        /// <summary>
        /// Draws the map on the console.
        /// </summary>
        /// <param name="title"></param>
        public void Draw(string? title = null)
        {
            if (title != null)
            {
                Console.WriteLine(title);
            }
            for (var y = 0; y < Height; ++y)
            {
                for (var x = 0; x < Width; ++x)
                {
                    if (Robot.PositionEquals(x, y))
                    {
                        Console.Write('@');
                    }
                    else if (Boxes.Contains((x, y)))
                    {
                        Console.Write('O');
                    }
                    else if (Walls.Contains((x, y)))
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
