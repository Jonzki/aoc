using System.Buffers;

namespace Aoc.Problems.Aoc23;

public class Problem04 : IProblem
{
    public object Solve1(string input)
    {
        return input
            .SplitLines()
            .Select(Card.Parse)
            .Sum(c => c.CalculateValue1());
    }

    public object Solve2(string input)
    {
        // Organize the cards into a queue.
        var cards = new Queue<Card>(
            input.SplitLines().Select(Card.Parse)
        ).ToDictionary(c => c.Id);

        var totalCards = cards.Count;

        var cardQueue = new Queue<byte>(cards.Keys);

        while (cardQueue.TryDequeue(out var id))
        {
            // Get the winning cards for the current card.
            var winningCards = cards[id].GetWinningCards();

            // Increase total amount of cards we have.
            totalCards += winningCards.Length;

            // Add all our cards to queue.
            foreach (var winningCard in winningCards)
            {
                cardQueue.Enqueue(winningCard);
            }
        }

        return totalCards;
    }

    public class Card
    {
        private readonly SearchValues<byte> _winSearch;

        public byte Id { get; }
        public byte[] Numbers { get; }
        public byte[] WinningNumbers { get; }

        // Buffer for the winning cards (part 2).
        private byte[]? _winningCards = null;

        /// <summary>
        /// Parses a Card from the input.
        /// </summary>
        /// <param name="input"></param>
        public Card(string input)
        {
            var parts = input.Split(':', '|');
            Id = byte.Parse(parts[0].Substring(parts[0].IndexOf(' ')));
            WinningNumbers = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(byte.Parse).ToArray();
            Numbers = parts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(byte.Parse).ToArray();
            _winSearch = SearchValues.Create(WinningNumbers);
        }

        public static Card Parse(string input) => new Card(input);

        /// <summary>
        /// Calculates the value/worth of the card (part 1).
        /// </summary>
        /// <returns></returns>
        public int CalculateValue1()
        {
            // Count the amount of numbers.
            var count = Numbers.Count(_winSearch.Contains);
            // Total value is 2^(n-1).
            return (int)Math.Pow(2, count - 1);
        }

        /// <summary>
        /// Part 2: returns the output (winning) card numbers.
        /// </summary>
        /// <returns></returns>
        public byte[] GetWinningCards()
        {
            if (_winningCards != null)
            {
                return _winningCards;
            }
            // "you win copies of the scratchcards below the winning card equal to the number of matches"
            _winningCards = new byte[Numbers.Count(_winSearch.Contains)];
            for (byte i = 0; i < _winningCards.Length; i++)
            {
                _winningCards[i] = (byte)(Id + 1 + i);
            }
            return _winningCards;
        }
    }
}
