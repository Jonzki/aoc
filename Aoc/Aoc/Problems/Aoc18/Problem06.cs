using System;
using System.Collections.Generic;
using System.Linq;
using Aoc.Utils;

namespace Aoc.Problems.Aoc18;

public class Problem06 : IProblem
{
    public object Solve1(string input)
    {
        // Step 0: parse the input.
        var coordinates = ParseCoordinates(input);

        // Step 1: find a bounding box.
        (int minX, int minY, int maxX, int maxY) = GetBoundingBox(coordinates);

        // Finite areas can only be found within this box.
        var potentialLocations = coordinates
            .Where(c => c.IsWithin((minX, minY), (maxX, maxY)))
            .ToDictionary(c => c.Id);

        // Build and fill a map.
        var map = new Dictionary<Point2D, int>();
        // Fill proper coordinates first.
        foreach (var c in coordinates)
        {
            map.TryAdd(c, c.Id);
        }

        // Flood-fill from each location, while staying within the bounding box.
        for (var i = 0; i < 10_000; ++i)
        {
            // Use the coordinate class to set up changes.
            var changes = new List<Coordinate>();

            for (var x = minX; x <= maxX; ++x)
            {
                for (var y = minY; y <= maxY; ++y)
                {
                    var pos = new Point2D(x, y);
                    if (GetMapValue(map, pos) == 0)
                    {
                        // Scan for nearby items.
                        var ids = new List<int>();
                        foreach (var c in coordinates)
                        {
                            if (GetMapValue(map, pos.Left()) == c.Id
                                || GetMapValue(map, pos.Right()) == c.Id
                                || GetMapValue(map, pos.Up()) == c.Id
                                || GetMapValue(map, pos.Down()) == c.Id)
                            {
                                ids.Add(c.Id);
                            }
                        };

                        if (ids.Count == 1)
                        {
                            // Found a single closest location.
                            // console.log('> Fill with ' + ids[0]);
                            changes.Add(new Coordinate(ids[0], pos.X, pos.Y));
                        }
                        else if (ids.Count > 1)
                        {
                            // console.log(`> Multiple matches - fill with '.'`);
                            changes.Add(new Coordinate(-1, pos.X, pos.Y));
                        }
                        else
                        {
                            // console.log('> Could not fill.');
                        }
                    }
                }
            }

            if (changes.Count == 0)
            {
                // No changes made - break the loop.
                break;
            }
            else
            {
                // Apply all the changes.
                foreach (var c in changes)
                {
                    map[c] = c.Id;
                }
            }
        }

        // Resolve area sizes.
        var areaSizes = potentialLocations.ToDictionary(x => x.Key, x => 0);
        foreach (var id in map.Values)
        {
            if (areaSizes.ContainsKey(id))
            {
                areaSizes[id] = areaSizes[id] + 1;
            }
        }

        return areaSizes.Values.OrderByDescending(x => x).First();
    }

    public object Solve2(string input)
    {
        // Parse the coordinates.
        var coordinates = ParseCoordinates(input);

        // Step 1: find a bounding box again.
        (int minX, int minY, int maxX, int maxY) = GetBoundingBox(coordinates);

        var region = new List<Point2D>();

        // Calculate items that belong in this region.
        const int maxDistance = 10000;
        for (var x = minX; x <= maxX; ++x)
        {
            for (var y = minY; y <= maxY; ++y)
            {
                var totalDistance = 0;
                foreach (var c in coordinates)
                {
                    totalDistance += NumberUtils.ManhattanDistance(c, (x, y));
                }
                if (totalDistance < maxDistance)
                {
                    region.Add(new Point2D(x, y));
                }
            }
        }

        return region.Count;
    }

    /// <summary>
    /// Returns a map value for the input position.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private int GetMapValue(Dictionary<Point2D, int> map, Point2D position)
    {
        if (map.TryGetValue(position, out var temp)) return temp;
        return 0;
    }

    private (int MinX, int MinY, int MaxX, int MaxY) GetBoundingBox(List<Coordinate> coordinates)
    {
        int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
        foreach (var c in coordinates)
        {
            if (c.X < minX) minX = c.X;
            if (c.X > maxX) maxX = c.X;
            if (c.Y < minY) minY = c.Y;
            if (c.Y > maxY) maxY = c.Y;
        }
        return (minX, minY, maxX, maxY);
    }

    /// <summary>
    /// Parses coordinate objects from the input.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private List<Coordinate> ParseCoordinates(string input)
    {
        var coordinates = new List<Coordinate>();
        var lines = input.SplitLines();
        for (var i = 0; i < lines.Length; ++i)
        {
            var parts = lines[i].Split(',').Select(int.Parse).ToArray();
            coordinates.Add(new Coordinate(i + 1, parts[0], parts[1]));
        }
        return coordinates;
    }

    public class Coordinate : Point2D
    {
        public Coordinate(int id, int x, int y) : base(x, y)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
