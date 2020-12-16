using Aoc.Utils;
using System;

namespace Aoc.Problems.Aoc20
{
    public class Problem12 : IProblem
    {
        public object Solve1(string input)
        {
            var commands = input.SplitLines();

            var ship = new Ship();

            foreach (var command in commands) ship.Move1(command);

            return NumberUtils.ManhattanDistance(ship.Position, (0, 0));
        }

        public object Solve2(string input)
        {
            var commands = input.SplitLines();

            var ship = new Ship();

            foreach (var command in commands) ship.Move2(command);

            return NumberUtils.ManhattanDistance(ship.Position, (0, 0));
        }

        public class Ship
        {
            /// <summary>
            /// Current position.
            /// </summary>
            public (int X, int Y) Position { get; set; } = (0, 0);

            /// <summary>
            /// Waypoint position relative to the Ship. (for part 2).
            /// </summary>
            public (int X, int Y) Waypoint { get; set; } = (10, 1);

            /// <summary>
            /// Current direction.
            /// </summary>
            public char Direction { get; set; } = 'E'; // The ship starts by facing east.

            private readonly char[] Directions = { 'N', 'E', 'S', 'W' };

            /// <summary>
            /// Moves according to the given command, with the ruleset of part 1.
            /// </summary>
            /// <param name="command"></param>
            public void Move1(string command)
            {
                var dir = command[0];
                var amount = int.Parse(command.Substring(1));

                if (dir == 'N')
                {
                    Position = (Position.X, Position.Y + amount);
                }
                else if (dir == 'S')
                {
                    Position = (Position.X, Position.Y - amount);
                }
                else if (dir == 'E')
                {
                    Position = (Position.X + amount, Position.Y);
                }
                else if (dir == 'W')
                {
                    Position = (Position.X - amount, Position.Y);
                }
                else if (dir == 'R')
                {
                    // Turn right by given amount of degrees.
                    var turnCount = (amount / 90) % Directions.Length;
                    var newIndex = (Directions.Length + Array.IndexOf(Directions, Direction) + turnCount) % Directions.Length;
                    Direction = Directions[newIndex];
                }
                else if (dir == 'L')
                {
                    // Turn left by given amount of degrees.
                    var turnCount = -(amount / 90) % Directions.Length;
                    var newIndex = (Directions.Length + Array.IndexOf(Directions, Direction) + turnCount) % Directions.Length;
                    Direction = Directions[newIndex];
                }
                else if (dir == 'F')
                {
                    // Move to the current direction by "amount".
                    Move1(string.Join("", Direction, amount));
                }
            }

            /// <summary>
            /// Moves according to the given command, with the ruleset of part 1.
            /// </summary>
            /// <param name="command"></param>
            public void Move2(string command)
            {
                var dir = command[0];
                var amount = int.Parse(command.Substring(1));

                // N,S,E,W move the waypoint in part 2.
                if (dir == 'N')
                {
                    Waypoint = (Waypoint.X, Waypoint.Y + amount);
                }
                else if (dir == 'S')
                {
                    Waypoint = (Waypoint.X, Waypoint.Y - amount);
                }
                else if (dir == 'E')
                {
                    Waypoint = (Waypoint.X + amount, Waypoint.Y);
                }
                else if (dir == 'W')
                {
                    Waypoint = (Waypoint.X - amount, Waypoint.Y);
                }
                else if (dir == 'R' || dir == 'L')
                {
                    // Turn right by given amount of degrees.
                    var turnCount = (Directions.Length + (dir == 'R' ? 1 : -1) * (amount / 90)) % Directions.Length;
                    var newIndex = (Directions.Length + Array.IndexOf(Directions, Direction) + turnCount) % Directions.Length;
                    Direction = Directions[newIndex];

                    // Turn count is 0-3. Update the waypoint position manually.
                    Waypoint = turnCount switch
                    {
                        // No turns -> stay as is.
                        0 => Waypoint,
                        // 1 = 90deg Right.
                        1 => (Waypoint.Y, -Waypoint.X),
                        // 2 = Turn around.
                        2 => (-Waypoint.X, -Waypoint.Y),
                        // 3 = 90deg Left.
                        3 => (-Waypoint.Y, Waypoint.X),
                        // Should never happen.
                        _ => throw new InvalidOperationException($"Turn count was {turnCount}.")
                    };
                }
                else if (dir == 'F')
                {
                    // Move towards the waypoint "amount" times.
                    Position = (Position.X + Waypoint.X * amount, Position.Y + Waypoint.Y * amount);
                }
            }
        }
    }
}