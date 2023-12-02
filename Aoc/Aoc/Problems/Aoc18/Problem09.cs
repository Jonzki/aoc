using System.Diagnostics;

namespace Aoc.Problems.Aoc18;

/// <summary>
/// https://adventofcode.com/2018/day/9
/// </summary>
public class Problem09 : IProblem
{
    public object Solve1(string input)
    {
        var parsed = ParseInput(input);
        return Solve2(parsed.PlayerCount, parsed.LastMarble);
    }

    public object Solve2(string input)
    {
        var parsed = ParseInput(input);
        return Solve2(parsed.PlayerCount, parsed.LastMarble * 100);
    }

    private (int PlayerCount, int LastMarble) ParseInput(string input)
    {
        var parts = input
            .Replace(" players", "")
            .Replace(" last marble is worth ", "")
            .Replace(" points", "")
            .Split(';');

        var playerCount = int.Parse(parts[0]);
        var lastMarble = int.Parse(parts[1]);

        return (playerCount, lastMarble);
    }

    private void Print(LinkedList<int> marbles, int current)
    {
        Console.Write($"[{current}] ");
        for (var m = marbles.First; m != null; m = m.Next)
        {
            if (m.Value == current)
            {
                Console.Write($"({m.Value}) ");
            }
            else
            {
                Console.Write($"{m.Value} ");
            }
        }
        Console.WriteLine("");
    }

    // List<T> - slow
    private long Solve1(int playerCount, int lastMarble)
    {
        // Player Id -> Score
        var players = new Dictionary<int, int>();
        for (var i = 1; i <= playerCount; ++i)
        {
            players.Add(i, 0);
        }

        // Marble array
        var index = 0;
        var marbles = new List<int>() { 0 };
        var marble = 0;
        var player = 1;

        var sw = new Stopwatch();
        sw.Start();
        while (true)
        {
            ++marble;
            if ((100 * marble) % lastMarble == 0)
            {
                Console.WriteLine($"{(100 * marble) / lastMarble}% ... {sw.Elapsed}");
                sw.Restart();
            }

            if (marble % 23 == 0)
            {
                // Add player points for the marble.
                players[player] += marble;
                // Add points from the -7 marble.
                index = index - 7;
                if (index < 0)
                {
                    index += marbles.Count;
                }
                index = index % marbles.Count;

                players[player] += marbles[index];
                // Remove the -7 marble.
                marbles.RemoveAt(index);
            }
            else
            {
                // Place the marble.
                index = index + 2;
                if (index > marbles.Count)
                {
                    index -= marbles.Count;
                }
                marbles.Insert(index, marble);
            }

            // this.printTurn(player, marbles, index);

            if (marble == lastMarble)
            {
                // Console.WriteLine('Last marble placed.');
                break;
            }

            player++;
            if (player > playerCount) player = 1;
        }

        // Console.WriteLine(players);

        var winningPlayer = 0;
        var winningScore = 0;
        foreach (var p in players.Keys)
        {
            if (players[p] > winningScore)
            {
                winningPlayer = p;
                winningScore = players[p];
            }
        }

        Console.WriteLine($"Player {winningPlayer} wins with a score of {winningScore}.");
        return winningScore;
    }

    // LinkedList<T> - fast?
    private long Solve2(int playerCount, int lastMarble)
    {
        // Player Id -> Score
        var players = new Dictionary<int, long>();
        for (var i = 1; i <= playerCount; ++i)
        {
            players.Add(i, 0);
        }

        // Marble array
        var marbles = new LinkedList<int>();
        var marble = marbles.AddFirst(0);

        var player = 1;

        var sw = new Stopwatch();
        sw.Start();
        for (var val = 1; val <= lastMarble; ++val)
        {
            //Console.WriteLine("Placing marble " + val);
            if (val % 23 == 0)
            {
                // Add player points for the marble.
                players[player] += val;

                var removeMarble = marble;
                for (var i = 0; i < 7; ++i)
                {
                    removeMarble = removeMarble.Previous ?? marbles.Last;
                }
                // Add points from the -7 marble.
                players[player] += removeMarble.Value;
                // Next marble will be the next from the removed one.
                marble = removeMarble.Next ?? marbles.First;
                // Remove the marble.
                marbles.Remove(removeMarble);
            }
            else
            {
                // Add the new Marble.
                var temp = marble;
                for (int i = 0; i < 1; ++i)
                {
                    temp = temp.Next ?? marbles.First;
                }
                //Console.WriteLine($"Inserting {val} after {temp.Value}.");
                marble = marbles.AddAfter(temp, val);
            }

            // this.printTurn(player, marbles, index);
            if (val >= lastMarble)
            {
                //Console.WriteLine("Last marble placed.");
                break;
            }

            player++;
            if (player > playerCount) player = 1;

            //Print(marbles, marble.Value);
        }

        //Console.WriteLine("Final marbles:");
        //Print(marbles, marble.Value);
        // Console.WriteLine(players);

        var winningPlayer = 0;
        long winningScore = 0;
        foreach (var p in players.Keys)
        {
            if (players[p] > winningScore)
            {
                winningPlayer = p;
                winningScore = players[p];
            }
        }

        Console.WriteLine($"Player {winningPlayer} wins with a score of {winningScore}.");
        return winningScore;
    }
}
