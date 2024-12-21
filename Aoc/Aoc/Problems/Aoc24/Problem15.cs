namespace Aoc.Problems.Aoc24;

public class Problem15 : IProblem
{
    public bool DrawSteps = false;

    public object Solve1(string input)
    {
        var (map, commands) = ParseInput(input);

        map.Draw("Initial state:");

        for (var i = 0; i < commands.Count; ++i)
        {
            map.Move(commands[i]);

            if (DrawSteps && i < 15)
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
        var (map, commands) = ParseInput(input, wide: true);

        map.Draw("Initial state:");

        for (var i = 0; i < commands.Count; ++i)
        {
            map.Move(commands[i]);

            if (DrawSteps && i < 15)
            {
                map.Draw($"Move {commands[i]}:");
            }
        }

        map.Draw("Final state:");

        // For the result, calculate the sum of GPS coordinate of each box.
        var result = map.Boxes.Sum(boxPosition => 100 * boxPosition.Y + boxPosition.X);

        return result;
    }

    public static (Map Map, List<char> Moves) ParseInput(string input, bool wide = false)
    {
        // Separate map and moves.
        var parts = input.Split(Environment.NewLine + Environment.NewLine);

        return (ParseMap(parts[0], wide), parts[1].Replace(Environment.NewLine, "").ToList());
    }

    public static Map ParseMap(string input, bool wide = false)
    {
        var lines = input.SplitLines();

        var height = lines.Length;
        var width = lines[0].Length;

        var map = new Map { Width = (wide ? 2 : 1) * width, Height = height, IsWide = wide };

        for (var y = 0; y < height; ++y)
        {
            for (var x = 0; x < width; ++x)
            {
                if (wide)
                {
                    switch (lines[y][x])
                    {
                        case '#':
                            map.Walls.Add((2 * x, y));
                            map.Walls.Add((2 * x + 1, y));
                            break;

                        case 'O':
                            map.Boxes.Add((2 * x, y));
                            break;

                        case '@':
                            map.Robot = (2 * x, y);
                            break;
                    }
                }
                else
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
        }

        return map;
    }

    public class Map
    {
        public bool IsWide { get; init; }

        public int Width { get; init; }
        public int Height { get; init; }

        public HashSet<Point2D> Walls { get; } = new();

        public HashSet<Point2D> Boxes { get; } = new();

        public Point2D Robot { get; set; }

        public void Move(char dir)
        {
            if (IsWide)
            {
                MoveWide(dir);
                return;
            }

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

        private void MoveWide(char dir)
        {
            // 4 directions special handling.
            if (dir == '<')
            {
                // Left. Check if direct left has a wall.
                if (Walls.Contains(Robot.Left()))
                {
                    return;
                }

                // If double left contains a Box, try move it.
                if (Boxes.Contains(Robot.Left().Left()))
                {
                    MoveBoxWide(Robot.Left().Left(), '<');
                }

                // If there is not a box next to us now, we are free to move.
                if (Boxes.Contains(Robot.Left().Left()))
                {
                    return;
                }

                Robot = Robot.Left();
            }
            else if (dir == '>')
            {
                // Right. Check for walls again.
                if (Walls.Contains(Robot.Right()))
                {
                    return;
                }

                // If right side contains a Box, try move it.
                // Left edge coordinates.
                if (Boxes.Contains(Robot.Right()))
                {
                    MoveBoxWide(Robot.Right(), '>');
                }

                // Re-check for obstacles (box stuck).
                if (Boxes.Contains(Robot.Right()))
                {
                    return;
                }

                Robot = Robot.Right();
            }
            else if (dir == '^')
            {
                // Check if the spot above has a wall.
                if (Walls.Contains(Robot.Up()))
                {
                    return;
                }

                // Check for boxes either directly above or offset one to the left.
                // Try move either up.
                if (Boxes.Contains(Robot.Up()))
                {
                    MoveBoxWide(Robot.Up(), '^');
                }
                if (Boxes.Contains(Robot.Up()))
                {
                    return;
                }

                if (Boxes.Contains(Robot.Up().Left()))
                {
                    MoveBoxWide(Robot.Up().Left(), '^');
                }
                if (Boxes.Contains(Robot.Up().Left()))
                {
                    return;
                }

                Robot = Robot.Up();
            }
            else if (dir == 'v')
            {
                // Check if the spot below has a wall.
                if (Walls.Contains(Robot.Down()))
                {
                    return;
                }

                // Check for boxes either directly below or offset one to the left.
                // Try move either down.
                if (Boxes.Contains(Robot.Down()))
                {
                    MoveBoxWide(Robot.Down(), 'v');
                }
                if (Boxes.Contains(Robot.Down()))
                {
                    return;
                }

                if (Boxes.Contains(Robot.Down().Left()))
                {
                    MoveBoxWide(Robot.Down().Left(), 'v');
                }
                if (Boxes.Contains(Robot.Down().Left()))
                {
                    return;
                }

                Robot = Robot.Down();
            }
        }

        private bool MoveBoxWide(Point2D position, char dir)
        {
            // Can't move a nonexistent box.
            if (!Boxes.Contains(position))
            {
                return false;
            }

            if (!CanMoveBoxWide(position, dir))
            {
                return false;
            }

            // 4 directions special handling.
            if (dir == '<')
            {
                // Any boxes to our left should be movable now.
                MoveBoxWide(position.Left().Left(), '<');

                // Should be free now.
                Boxes.Add(position.Left());
                Boxes.Remove(position);
            }
            else if (dir == '>')
            {
                MoveBoxWide(position.Right().Right(), '>');

                // Should be free now.
                Boxes.Add(position.Right());
                Boxes.Remove(position);
            }
            else if (dir == '^')
            {
                // There may also be a box in 3 possible locations.
                // Move any boxes found.
                Point2D[] boxPositions = [position.Up().Left(), position.Up(), position.Up().Right()];
                foreach (var b in boxPositions)
                {
                    MoveBoxWide(b, '^');
                }

                // Should be clear to move now.
                Boxes.Add(position.Up());
                Boxes.Remove(position);
            }
            else if (dir == 'v')
            {
                // There may also be a box in 3 possible locations.
                // Move any boxes found.
                Point2D[] boxPositions = [position.Down().Left(), position.Down(), position.Down().Right()];
                foreach (var b in boxPositions)
                {
                    MoveBoxWide(b, 'v');
                }

                // Should be clear to move now.
                Boxes.Add(position.Down());
                Boxes.Remove(position);
            }

            return true;
        }

        /// <summary>
        /// Checks if box movement is allowed.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        private bool CanMoveBoxWide(Point2D position, char dir)
        {
            // Can't move a nonexistent box.
            if (!Boxes.Contains(position))
            {
                return false;
            }

            // 4 directions special handling.
            if (dir == '<')
            {
                // Check if there is a free spot to the left. May be blocked by a wall:
                if (Walls.Contains(position.Left()))
                {
                    return false;
                }
                // Or by a box:
                if (Boxes.Contains(position.Left().Left()))
                {
                    return CanMoveBoxWide(position.Left().Left(), '<');
                }
                // Otherwise free to move.
                return true;
            }
            if (dir == '>')
            {
                // Check if there is a free spot to the double right. May be blocked by a wall:
                if (Walls.Contains(position.Right().Right()))
                {
                    return false;
                }
                // Or by a box:
                if (Boxes.Contains(position.Right().Right()))
                {
                    return CanMoveBoxWide(position.Right().Right(), '>');
                }

                return true;
            }
            if (dir == '^')
            {
                // Moving up might be blocked by two walls.
                if (Walls.Contains(position.Up()) || Walls.Contains(position.Up().Right()))
                {
                    return false;
                }

                // There may also be a box in 3 possible locations.
                // Move any boxes found.
                Point2D[] boxPositions = [position.Up().Left(), position.Up(), position.Up().Right()];
                foreach (var b in boxPositions)
                {
                    if (Boxes.Contains(b) && !CanMoveBoxWide(b, '^'))
                    {
                        return false;
                    }
                }
                return true;
            }
            if (dir == 'v')
            {
                // Moving down might be blocked by two walls.
                if (Walls.Contains(position.Down()) || Walls.Contains(position.Down().Right()))
                {
                    return false;
                }

                // There may also be a box in 3 possible locations.
                // Move any boxes found.
                Point2D[] boxPositions = [position.Down().Left(), position.Down(), position.Down().Right()];
                foreach (var b in boxPositions)
                {
                    if (Boxes.Contains(b) && !CanMoveBoxWide(b, 'v'))
                    {
                        return false;
                    }
                }

                return true;
            }

            throw new ArgumentOutOfRangeException(nameof(dir), $"Unsupported direction '{dir}'.");
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
                    else if (!IsWide && Boxes.Contains((x, y)))
                    {
                        Console.Write('O');
                    }
                    else if (IsWide && Boxes.Contains((x, y)))
                    {
                        Console.Write('[');
                    }
                    else if (IsWide && Boxes.Contains((x - 1, y)))
                    {
                        Console.Write(']');
                    }
                    else if (Walls.Contains((x, y)))
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
