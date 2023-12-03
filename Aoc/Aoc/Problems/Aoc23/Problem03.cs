using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aoc.Problems.Aoc23;

public class Problem03 : IProblem
{
    public object Solve1(string input)
    {
        var schematic = EngineSchematic.Parse(input);

        int sum = 0;
        foreach (var number in schematic.Numbers)
        {
            var symbol = schematic.GetSurroundingSymbol(number);
            if (symbol.Symbol != default)
            {
                sum += number.Value;
            }
        }
        return sum;
    }

    public object Solve2(string input)
    {
        var schematic = EngineSchematic.Parse(input);

        // Track all numbers with a star.
        var numbersWithStar = new Dictionary<Point2D, List<int>>();
        foreach (var number in schematic.Numbers)
        {
            var symbol = schematic.GetSurroundingSymbol(number);
            if (symbol.Symbol == '*')
            {
                numbersWithStar.TryAdd(symbol.Position, new List<int>());
                numbersWithStar[symbol.Position].Add(number.Value);
            }
        }

        // Look for stars with exactly two values.
        return numbersWithStar.Values
            .Where(x => x.Count == 2)
            .Select(x => x[0] * x[1])
            .Sum();
    }

    public class EngineSchematic
    {
        /// <summary>
        /// Lists numbers on the schematic and their (starting) locations.
        /// </summary>
        public List<(int Value, Point2D Position)> Numbers { get; } = new();

        /// <summary>
        /// Lists symbols on the schematic.
        /// </summary>
        public List<(char Symbol, Point2D Position)> Symbols { get; } = new();

        /// <summary>
        /// Parses a Schematic from the input string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static EngineSchematic Parse(string input)
        {
            var lines = input.SplitLines();

            var schematic = new EngineSchematic();

            // Parse the map.
            int number = 0, numX = -1, numY = -1;
            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    // If we find a charac
                    if (char.IsDigit(lines[y][x]))
                    {
                        // Store the number position if we were at zero.
                        if (number == 0)
                        {
                            numX = x; numY = y;
                        }
                        // Increase the number.
                        number = (number * 10) + (lines[y][x] - '0');
                    }
                    else
                    {
                        // Not a digit. Save and reset the current number if we were processing one.
                        if (number > 0)
                        {
                            schematic.Numbers.Add((number, (numX, numY)));
                            number = 0;
                            numX = -1;
                            numY = -1;
                        }
                        if (lines[y][x] != '.')
                        {
                            schematic.Symbols.Add((lines[y][x], (x, y)));
                        }
                    }
                }
            }

            return schematic;
        }

        /// <summary>
        /// Returns the first Symbol surrounding the input Number.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public (char Symbol, Point2D Position) GetSurroundingSymbol((int Value, Point2D Position) number)
        {
            var numberLength = (int)Math.Log10(number.Value) + 1;
            var position = number.Position;

            // Look for surrounding symbols.
            for (int x = position.X - 1; x <= position.X + numberLength; x++)
            {
                for (int y = position.Y - 1; y <= position.Y + 1; y++)
                {
                    var symbol = Symbols.FirstOrDefault(s => s.Position.Equals(x, y));
                    if (symbol.Symbol != default)
                    {
                        return symbol;
                    }
                }
            }

            return default;
        }

        public void Print()
        {
            var minX = Math.Min(
                Numbers.Min(x => x.Position.X),
                Symbols.Min(x => x.Position.X)
            );
            var maxX = Math.Max(
                Numbers.Max(x => x.Position.X),
                Symbols.Max(x => x.Position.X)
            );
            var minY = Math.Min(
                Numbers.Min(x => x.Position.Y),
                Symbols.Min(x => x.Position.Y)
            );
            var maxY = Math.Max(
                Numbers.Max(x => x.Position.Y),
                Symbols.Max(x => x.Position.Y)
            );

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    var num = Numbers.FirstOrDefault(n => n.Position.Equals(x, y));
                    var symbol = Symbols.FirstOrDefault(s => s.Position.Equals(x, y));

                    if (num.Value > 0)
                    {
                        var s = num.Value.ToString();
                        Console.Write(num.Value);
                        x += (s.Length - 1);
                    }
                    else if (symbol.Symbol != default)
                    {
                        Console.Write(symbol.Symbol);
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }

                Console.WriteLine();
            }
        }
    }
}
