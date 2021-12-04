using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aoc.Utils;

namespace Aoc.Problems.Aoc21;

public class Problem4 : IProblem
{
    public class BingoBoard
    {
        public int Id { get; init; }
        public int[,] Values { get; init; }
        public bool[,] Checks { get; init; }
        public bool HasBingo { get; private set; }

        public static BingoBoard Parse(string input, int index)
        {
            // Assume a 5x5 board.
            var values = input.Split(new string[] { Environment.NewLine, " " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            var boardNumbers = ArrayUtils.To2D(values, 5, 5);

            var board = new BingoBoard
            {
                Id = index,
                Values = boardNumbers,
                Checks = new bool[5, 5]
            };
            return board;
        }

        /// <summary>
        /// Adds one number to the board. Returns true if there's a Bingo, false otherwise.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool AddNumber(int number)
        {
            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    if (Values[x, y] == number)
                    {
                        Checks[x, y] = true;
                    }
                }
            }
            var bingo = CheckForBingo();
            if (bingo)
            {
                HasBingo = true;
            }
            return bingo;
        }

        /// <summary>
        /// Checks if there's a Bingo (any filled row or column).
        /// </summary>
        /// <returns></returns>
        public bool CheckForBingo()
        {
            var rows = new[] { true, true, true, true, true };
            var cols = new[] { true, true, true, true, true };

            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    rows[x] &= Checks[x, y];
                    cols[y] &= Checks[x, y];
                }
            }

            return rows.Any(x => x) || cols.Any(y => y);
        }

        /// <summary>
        /// Calculates the winning Bingo score for the board.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        internal object CalculateScore(int number)
        {
            // Start by finding the sum of all unmarked numbers on that board.
            var unmarkedSum = 0;
            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    if (!Checks[x, y])
                    {
                        unmarkedSum += Values[x, y];
                    }
                }
            }

            // Then, multiply that sum by the number that was just called when the board won to get the final score.
            return unmarkedSum * number;
        }
    }

    public object Solve1(string input)
    {
        var (boards, numbers) = ParseInput(input);

        foreach (var number in numbers)
        {
            foreach (var board in boards)
            {
                // Add number, check for bingo.
                if (board.AddNumber(number))
                {
                    Console.WriteLine($"Bingo on board {board.Id}!");
                    return board.CalculateScore(number);
                }

            }
        }

        Console.WriteLine("No winner.");
        return -1;
    }

    public object Solve2(string input)
    {
        var (boards, numbers) = ParseInput(input);
        var boardsRemaining = boards.Count;

        foreach (var number in numbers)
        {
            foreach (var board in boards)
            {
                // Bingoed boards don't need to play anymore.
                if (board.HasBingo) continue;

                // Add number, check for bingo.
                var isBingo = board.AddNumber(number);
                if (isBingo)
                {
                    boardsRemaining--;
                    // "Figure out which board will win last."
                    if (boardsRemaining == 0)
                    {
                        // "Once it wins, what would its final score be?"
                        return board.CalculateScore(number);
                    }
                }

            }
        }

        Console.WriteLine("No winner.");
        return -1;
    }

    public static (List<BingoBoard> Boards, List<int> Numbers) ParseInput(string input)
    {
        // Starts by splitting with double line ends.
        var blocks = input.Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        // First block is the numbers array - split with comma.
        var numbers = blocks[0].Split(',').Select(int.Parse).ToList();

        // Remaining blocks are bingo boards.
        var boards = blocks.Skip(1).Select((input, index) => BingoBoard.Parse(input, index)).ToList();

        return (boards, numbers);
    }
}
