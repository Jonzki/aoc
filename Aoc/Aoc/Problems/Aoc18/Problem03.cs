using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aoc.Utils;

namespace Aoc.Problems.Aoc18;

public class Problem03 : IProblem
{
    public object Solve1(string input)
    {
        var claims = input.SplitLines().Select(Claim.Parse).ToList();

        var claimCounts = GetClaimCounts(claims);

        return claimCounts.Count(kvp => kvp.Value.Count >= 2);
    }

    public object Solve2(string input)
    {
        // Parse the claims, then count the squares.
        var claims = input.SplitLines().Select(Claim.Parse).ToList();

        var claimCounts = GetClaimCounts(claims);

        // Start with all IDs.
        var ids = new HashSet<int>(claims.Select(c => c.Id));

        foreach (var kvp in claimCounts)
        {
            if (kvp.Value.Count >= 2)
            {
                ids.RemoveRange(kvp.Value);
            }
        }

        // There should now be exactly one ID left.
        if (ids.Count != 1)
        {
            throw new InvalidOperationException($"{ids.Count} IDs remain, was expecting 1.");
        }
        return ids.First();
    }

    /// <summary>
    /// Calculates all points and their claims.
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    private Dictionary<Point2D, List<int>> GetClaimCounts(List<Claim> claims)
    {
        var counts = new Dictionary<Point2D, List<int>>();
        foreach (var claim in claims)
        {
            for (var x = 0; x < claim.Width; x++)
            {
                for (var y = 0; y < claim.Height; y++)
                {
                    var position = claim.Position + new Point2D(x, y);
                    // Slightly inefficient to add a zero and then increment it, but more readable.
                    counts.TryAdd(position, new List<int>());
                    counts[position].Add(claim.Id);
                }
            }
        }
        return counts;
    }


    class Claim
    {
        public int Id { get; set; }
        public Point2D Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public static Claim Parse(string input)
        {
            // Split the input by a whole bunch of stuff.
            // This should give us exactly 5 numbers.
            var values = input
                .Split(new[] { '#', ' ', '@', ',', ':', 'x' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
            return new Claim
            {
                Id = values[0],
                Position = new Point2D(values[1], values[2]),
                Width = values[3],
                Height = values[4]
            };
        }
    }

}
