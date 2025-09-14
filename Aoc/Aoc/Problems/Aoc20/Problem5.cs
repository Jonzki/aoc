namespace Aoc.Problems.Aoc20;

public class Problem5 : IProblem
{
    public object Solve1(string input)
    {
        var boardingPasses = input
            .Split('\n')
            .Select(x => ParseBoardingPass(x.Trim()))
            .ToArray();

        // What is the highest seat ID on a boarding pass?
        return boardingPasses.Max(x => x.SeatId);
    }

    public object Solve2(string input)
    {
        // We're only interested in the seat IDs.
        var seatIds = input
            .Split('\n')
            .Select(x => ParseBoardingPass(x.Trim()).SeatId)
            .OrderBy(x => x)
            .ToArray();

        // Look for a seat between two others.
        for (var i = 0; i < seatIds.Length - 1; ++i)
        {
            if (seatIds[i + 1] - seatIds[i] == 2) return seatIds[i] + 1;
        }

        return -1;
    }

    public static BoardingPass ParseBoardingPass(string input)
    {
        // First 7 chars for row, last 3 for column.
        var rowInput = input.Substring(0, 7);
        var colInput = input.Substring(7);

        // Binary search for row.
        var row = BinarySearch(0, 127, rowInput);
        var col = BinarySearch(0, 7, colInput);

        return new BoardingPass { Row = row, Column = col };
    }

    public static int BinarySearch(int start, int end, string input)
    {
        // Binary search for row.
        int blockStart = start, blockEnd = end;

        for (int i = 0; i < input.Length; ++i)
        {
            var half = (1 + blockEnd - blockStart) / 2;
            if (input[i] == 'B' || input[i] == 'R')
            {
                // Back half.
                blockStart += half;
            }
            else if (input[i] == 'F' || input[i] == 'L')
            {
                // Front half.
                blockEnd -= half;
            }
            else throw new InvalidOperationException($"Unexpected char '{input[i]}' at position {i}.");
        }

        if (blockEnd - blockStart != 0)
        {
            throw new InvalidOperationException("Failed to find row.");
        }

        return blockStart;
    }

    public record BoardingPass
    {
        public int Row { get; set; }

        public int Column { get; set; }

        public int SeatId => 8 * Row + Column;
    }
}
