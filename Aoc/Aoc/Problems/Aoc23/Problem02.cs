namespace Aoc.Problems.Aoc23;

public class Problem02 : IProblem
{
    public object Solve1(string input)
    {
        int red = 12, green = 13, blue = 14;

        var output = input
            .SplitLines()
            .Select(Game.Parse)
            .Where(g => g.IsPossible(red, green, blue))
            .Select(g => g.Id)
            .Sum();

        return output;
    }

    public object Solve2(string input)
    {
        var output = input
            .SplitLines()
            .Select(Game.Parse)
            .Select(g => g.LowestPossible())
            .Select(x => x.Red * x.Green * x.Blue)
            .Sum();
        return output;
    }

    internal class Game
    {
        public static Game Parse(string input)
        {
            var parts = input.Split(':');

            var id = int.Parse(parts[0].Substring("Game ".Length));

            var hints = new List<(int Red, int Green, int Blue)>();

            parts = parts[1].Split(';');
            foreach (var hintString in parts)
            {
                var colors = hintString.Split(',', ' ', StringSplitOptions.RemoveEmptyEntries);

                int.TryParse(colors.FirstOrDefault(c => c.EndsWith("red"))?.Replace("red", ""), out var red);
                int.TryParse(colors.FirstOrDefault(c => c.EndsWith("green"))?.Replace("green", ""), out var green);
                int.TryParse(colors.FirstOrDefault(c => c.EndsWith("blue"))?.Replace("blue", ""), out var blue);

                hints.Add((red, green, blue));
            }

            return new Game { Id = id, Hints = hints };
        }

        public int Id { get; init; }

        public required List<(int Red, int Green, int Blue)> Hints { get; init; }

        /// <summary>
        /// Checks if the game is possible for the input amount of cubes.
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <returns></returns>
        public bool IsPossible(int red, int green, int blue)
        {
            foreach (var hint in Hints)
            {
                if (hint.Red > red || hint.Green > green || hint.Blue > blue) return false;
            }
            return true;
        }

        /// <summary>
        /// Calculates the lowest possible amount of cubes to make the game possible.
        /// </summary>
        /// <returns></returns>
        public (int Red, int Green, int Blue) LowestPossible()
        {
            int r = 0, g = 0, b = 0;
            foreach (var hint in Hints)
            {
                if (hint.Red > r) r = hint.Red;
                if (hint.Green > g) g = hint.Green;
                if (hint.Blue > b) b = hint.Blue;
            }
            return (r, g, b);
        }
    }
}
