namespace Aoc.Problems.Aoc18;

/// <summary>
/// https://adventofcode.com/2018/day/11
/// </summary>
public class Problem11 : IProblem
{
    private const string ActualInput = "8979";

    public object Solve1(string? input)
    {
        _serial = int.Parse(input ?? ActualInput);

        Console.WriteLine($"Grid Serial: {GridSerial}");
        this.Grid = new int?[300, 300];

        // Fill the grid.
        int blockX = -1, blockY = -1, blockPower = int.MinValue;
        for (int i = 0; i < 300 - 2; ++i)
        {
            for (int j = 0; j < 300 - 2; ++j)
            {
                var bp = GetBlockPower(i, j);
                if (bp > blockPower)
                {
                    blockPower = bp;
                    blockX = i;
                    blockY = j;
                }
            }
        }

        // Part 1 is only interested in the block coordinates.
        return $"{blockX},{blockY}";
    }

    public object Solve2(string? input)
    {
        _serial = int.Parse(input ?? ActualInput);
        Console.WriteLine($"Grid Serial: {GridSerial}");

        //this.Grid = new int?[300, 300];
        //this.Grid2 = new int?[300 * 300];

        var g = new GridX(GridSerial);

        var gridSize = 300;
        //Print(gridSize, this.GridSerial);

        // Fill the grid.
        int blockX = -1, blockY = -1, blockPower = int.MinValue, blockSize = 0;
        for (int i = 1; i <= gridSize; ++i)
        {
            for (int j = 1; j <= gridSize; ++j)
            {
                //Console.WriteLine($"{i},{j} ...");

                var bp = 0;
                for (int s = 1; i + s <= gridSize + 1 && j + s <= gridSize + 1; ++s)
                {
                    // Start with cell power (1x1)
                    if (s == 1)
                    {
                        bp = GetCellPower(i, j, this.GridSerial);
                    }
                    else
                    {
                        // Use the previous block power and add the edges.
                        var block = bp;
                        int x = 0, y = 0, t = 0;
                        for (var k = 0; k < s - 1; ++k)
                        {
                            x = i + s - 1;
                            y = j + k;

                            t = GetCellPower(x, y, this.GridSerial);
                            block += t;
                            //Console.WriteLine($"> Add {x},{y} -> {t} => {block}");

                            x = i + k;
                            y = j + s - 1;

                            t = GetCellPower(x, y, this.GridSerial);
                            block += t;
                            //Console.WriteLine($"> Add {x},{y} -> {t} => {block}");
                        }
                        // Add the corner.
                        x = i + s - 1;
                        y = j + s - 1;
                        t = GetCellPower(x, y, this.GridSerial);
                        block += t;
                        //Console.WriteLine($"> Add {x},{y} -> {t} => {block}");

                        //Console.WriteLine($"SUB {s - 1},{s - 1} -> {t2} => {block}");

                        bp = block;
                    }

                    //Console.WriteLine($"{i},{j}, s{s} -> BP: {bp}");

                    if (bp > blockPower)
                    {
                        blockX = i;
                        blockY = j;
                        blockPower = bp;
                        blockSize = s;
                    }
                }
            }
        }

        return $"{blockX},{blockY},{blockSize}";
    }

    private int _serial = 0;
    private int GridSerial => _serial;

    private int?[,] Grid { get; set; } = new int?[0, 0];
    private int?[] Grid2 { get; set; } = [];

    private int GetCellPower(int x, int y, int gridSerial)
    {
        //Console.WriteLine($"GetCellPower {x},{y}");
        //Find the fuel cell's rack ID, which is its X coordinate plus 10.
        var rackId = x + 10;

        //Begin with a power level of the rack ID times the Y coordinate.
        var power = rackId * y;

        //Increase the power level by the value of the grid serial number(your puzzle input).
        power += gridSerial;

        //Set the power level to itself multiplied by the rack ID.
        power *= rackId;

        //Keep only the hundreds digit of the power level(so 12345 becomes 3; numbers with no hundreds digit become 0).
        power = power %= 1000;
        power = (power - (power % 100)) / 100;

        //Subtract 5 from the power level.
        power -= 5;

        return power;
    }

    private int GetBlockPower(int x, int y, int s = 3)
    {
        var power = 0;
        for (int i = 0; i < s; ++i)
        {
            for (int j = 0; j < s; ++j)
            {
                if (Grid2 != null)
                {
                    if (!Grid2[(x + i) * 300 + (y + j)].HasValue)
                    {
                        Grid2[(x + i) * 300 + (y + j)] = GetCellPower(x + i, y + j, this.GridSerial);
                    }
                    power += Grid2[(x + i) * 300 + (y + j)]!.Value;
                }
                else
                {
                    if (!Grid[x + i, y + j].HasValue)
                    {
                        Grid[x + i, y + j] = GetCellPower(x + i, y + j, this.GridSerial);
                    }
                    power += Grid[x + i, y + j]!.Value;
                }
            }
        }
        return power;
    }

    private class GridX
    {
        private int[,] Grid1 { get; }

        private Dictionary<int, int[,]> blockGrids = new Dictionary<int, int[,]>();

        private List<int> blockSizes = new List<int>();

        public GridX(int gridSerial)
        {
            Grid1 = new int[300, 300];
            for (int i = 0; i < 300; ++i)
            {
                for (int j = 0; j < 300; ++j)
                {
                    Grid1[i, j] = GetCellPower(i + 1, j + 1, gridSerial);
                }
            }

            for (int i = 2; i <= 30; ++i)
            {
                if (300 % i == 0)
                {
                    blockGrids.Add(i, BuildBlockGrid(i));
                    blockSizes.Add(i);
                }
            }

            blockSizes = blockSizes.OrderByDescending(x => x).ToList();
        }

        private int[,] BuildBlockGrid(int s)
        {
            //Console.WriteLine("Build block grid for size " + s);
            var grid = new int[300 / s, 300 / s];
            for (int x = 0; x < 300; x += s)
            {
                for (int y = 0; y < 300; y += s)
                {
                    grid[x / s, y / s] = GetBlockPower(x, y, s, false);
                }
            }
            return grid;
        }

        private int GetCellPower(int x, int y, int gridSerial)
        {
            //Console.WriteLine($"GetCellPower {x},{y}");
            //Find the fuel cell's rack ID, which is its X coordinate plus 10.
            var rackId = x + 10;

            //Begin with a power level of the rack ID times the Y coordinate.
            var power = rackId * y;

            //Increase the power level by the value of the grid serial number(your puzzle input).
            power += gridSerial;

            //Set the power level to itself multiplied by the rack ID.
            power *= rackId;

            //Keep only the hundreds digit of the power level(so 12345 becomes 3; numbers with no hundreds digit become 0).
            power = power %= 1000;
            power = (power - (power % 100)) / 100;

            //Subtract 5 from the power level.
            power -= 5;

            return power;
        }

        public int GetBlockPower(int x, int y, int s = 3, bool useBlockGrids = true)
        {
            var power = 0;
            if (useBlockGrids)
            {
                int b = -1;
                foreach (var t in blockSizes)
                {
                    if (t <= s)
                    {
                        b = t;
                        break;
                    }
                }

                var skipCoords = new HashSet<(int x, int y)>();
                for (int i = 0; i < s; ++i)
                {
                    for (int j = 0; j < s; ++j)
                    {
                        if (skipCoords.Contains((x, y))) continue;

                        if (b != -1 && i % b == 0 && j % b == 0 && i + b < 300 - s + 1 && j + b < 300 - s + 1)
                        {
                            //Console.WriteLine($"Use block {b} for {i},{j}");
                            power += blockGrids[b][i / b, j / b];

                            for (int ii = 0; ii < b; ++ii)
                            {
                                for (int jj = 0; jj < b; ++jj)
                                {
                                    skipCoords.Add((i + ii, j + jj));
                                }
                            }
                        }
                        else
                        {
                            power += Grid1[x + i, y + j];
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < s; ++i)
                {
                    for (int j = 0; j < s; ++j)
                    {
                        power += Grid1[x + i, y + j];
                    }
                }
            }
            return power;
        }
    }

    private void Print(int gridSize, int gridSerial)
    {
        Console.WriteLine("GRID (10)");

        for (int y = 0; y < gridSize; ++y)
        {
            for (int x = 0; x < gridSize; ++x)
            {
                Console.Write($"{GetCellPower(x + 1, y + 1, gridSerial)}".PadLeft(3, ' '));
            }
            Console.WriteLine();
        }
        Console.WriteLine(new string('-', 10));
    }
}
