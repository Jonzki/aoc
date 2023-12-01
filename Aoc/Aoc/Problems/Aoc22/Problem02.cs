namespace Aoc.Problems.Aoc22;

/// <summary>
/// https://adventofcode.com/2022/day/2
/// </summary>
public class Problem02 : IProblem
{
    public object Solve1(string input)
    {
        // Split the input into lines.
        var lines = input.SplitLines();

        // We get points based on whether we win, lose or draw,
        // and based on which shape we chose.
        // Since there are only 3*3=9 possible pairs, we can do a simple map.

        var totalScore = 0;

        // Define the scoring.
        const int
            win = 6, draw = 3, loss = 0,
            rock = 1, paper = 2, scissors = 3;

        foreach (var line in lines)
        {
            // Win +6, Draw +3, Loss +0
            // Rock +1, Paper +2, Scissors +3
            totalScore += line switch
            {
                "A X" => draw + rock,       // Rock-rock - DRAW
                "A Y" => win + paper,       // Rock-paper
                "A Z" => loss + scissors,   // Rock-scissors
                "B X" => loss + rock,       // Paper-rock
                "B Y" => draw + paper,      // paper-paper
                "B Z" => win + scissors,    // paper-scissors
                "C X" => win + rock,        // scissors-rock
                "C Y" => loss + paper,      // scissors-paper
                "C Z" => draw + scissors,   // scissors-scissors
                _ => throw new InvalidOperationException($"Unexpected line: '{line}'")
            };
        }

        return totalScore;
    }

    public object Solve2(string input)
    {
        // Split the input into lines.
        var lines = input.SplitLines();

        // We get points based on whether we win, lose or draw,
        // and based on which shape we chose.
        // Since there are only 3*3=9 possible pairs, we can do a simple map.

        var totalScore = 0;

        // Define the scoring.
        const int
            win = 6, draw = 3, lose = 0,
            rock = 1, paper = 2, scissors = 3;

        foreach (var line in lines)
        {
            // Win +6, Draw +3, Loss +0
            // Rock +1, Paper +2, Scissors +3

            // For part 2, X needs to lose, Y needs to draw, Z needs to win.
            // The same 9 options still remain.
            totalScore += line switch
            {
                "A X" => lose + scissors,   // Lose to rock with scissors.
                "A Y" => draw + rock,       // Draw with rock.
                "A Z" => win + paper,       // Win rock with paper.
                "B X" => lose + rock,       // Lose to paper with rock.
                "B Y" => draw + paper,      // Draw paper with paper
                "B Z" => win + scissors,    // Win paper with scissors
                "C X" => lose + paper,      // Lose with paper.
                "C Y" => draw + scissors,   // Draw with scissors
                "C Z" => win + rock,        // Win scissors with rock.
                _ => throw new InvalidOperationException($"Unexpected line: '{line}'")
            };
        }

        return totalScore;
    }


}
