namespace Aoc.Problems.Aoc20
{
    public class Problem3 : IProblem
    {
        public object Solve1(string input)
        {
            var map = ParseMap(input);

            var trees = TravelMap(map);

            return trees;
        }

        public object Solve2(string input)
        {
            var map = ParseMap(input);

            var steps = new[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) };

            // Multiply the tree counts together - so start with 1.
            var trees = 1;
            for (int i = 0; i < steps.Length; ++i)
            {
                trees *= TravelMap(map, steps[i].Item1, steps[i].Item2);
            }

            return trees;
        }

        /// <summary>
        /// Parses the input string into a character map.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static char[,] ParseMap(string input)
        {
            var lines = input.Split(Environment.NewLine);

            var output = new char[lines.Length, lines[0].Length];

            for (var y = 0; y < lines.Length; ++y)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    output[y, x] = lines[y][x];
                }
            }

            return output;
        }

        /// <summary>
        /// Travels the map and counts the amount of "trees" hit along the way.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static int TravelMap(char[,] map, int stepX = 3, int stepY = 1)
        {
            const char treeChar = '#';

            var mapWidth = map.Width();
            var mapHeight = map.Height();

            int posX = 0;
            var trees = 0;

            for (int posY = 0; posY < mapHeight; posY += stepY)
            {
                // Check for tree at the current position.
                if (map[posY, posX] == treeChar)
                {
                    ++trees;
                }

                // Move right by the given step.
                // Modulo by map width to move over to "the next map".
                posX = (posX + stepX) % mapWidth;
            }

            return trees;
        }

    }
}