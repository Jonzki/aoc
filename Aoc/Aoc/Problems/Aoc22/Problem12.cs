using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aoc.Problems.Aoc20;
using Aoc.Utils;

namespace Aoc.Problems.Aoc22;

/// <summary>
/// https://adventofcode.com/2022/day/12
/// </summary>
public class Problem12 : IProblem
{
    public object Solve1(string input)
    {
        var map = ParseMap(input);

        var startNode = map.Nodes.First(n => n.Value == 'S');

        var totalCost = FindBestPathDijkstra(map.Nodes, startNode.Position);

        return totalCost;
    }

    public object Solve2(string input)
    {
        var map = ParseMap(input);

        // For part 2, we have many potential starting nodes.
        var startNodes = map.Nodes.Where(n => n.Value == 'a' || n.Value == 'S').ToList();

        var minDistance = int.MaxValue;

        foreach (var startNode in startNodes)
        {
            var totalCost = FindBestPathDijkstra(map.Nodes, startNode.Position);
            if (totalCost < minDistance)
            {
                minDistance = totalCost;
            }
        }

        return minDistance;
    }


    /// <summary>
    /// Parses a map of Nodes from the problem input (Dijkstra approach).
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static (List<Node> Nodes, int Width, int Height) ParseMap(string input)
    {
        var lines = input.SplitLines();
        var width = lines[0].Length;
        var height = lines.Length;

        var temp = lines.SelectMany(l => l.Select(c => c)).ToArray();

        var points = ArrayUtils.To2D(temp, width, height);

        var nodes = new List<Node>();
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                nodes.Add(new Node
                {
                    Position = (x, y),
                    Value = points[y, x],
                    StepsFromStart = 1_000_000_000,
                    Visited = false
                });
            }
        }

        return (nodes, width, height);
    }

    /// <summary>
    /// Dijkstra's algorithm for pathfinding.
    /// Adapted from 2021.15.
    /// </summary>
    /// <returns></returns>
    public static int FindBestPathDijkstra(List<Node> input, Point2D startPosition)
    {
        // Resolve the end position first.
        var maxX = input.Max(n => n.Position.X);
        var maxY = input.Max(n => n.Position.Y);

        var width = maxX + 1;
        var height = maxY + 1;

        List<Node> visitedNodes = new List<Node>();

        // Construct a positional lookup (this leverages C# references).
        var nodeMap = new Node[height, width];

        // Also construct a priority queue
        var priorityMap = new SortedDictionary<int, List<Node>>();

        foreach (var node in input)
        {
            // Set the starting risk.
            if (node.PositionEquals(startPosition))
            {
                node.StepsFromStart = 0;
            }

            // Assign a positional shortcut.
            nodeMap[node.Position.Y, node.Position.X] = node;

            if (!priorityMap.ContainsKey(node.StepsFromStart))
            {
                priorityMap.Add(node.StepsFromStart, new List<Node>());
            }
            priorityMap[node.StepsFromStart].Add(node);
        }

        // Run until we reach the target.
        Node currentNode = null;
        var i = 0;
        do
        {
            ++i;
            if (priorityMap.Count == 0) break;

            // Find an unvisited node with the smallest risk value. Add a large value when sorting to push visited nodes to the back.
            var minPrio = priorityMap.First();
            // Since the risk value is the same in this pool, we should be safe to pick the last item.
            currentNode = minPrio.Value.Last();
            if (currentNode == null)
            {
                // Stop if no unvisited nodes could be found.
                break;
            }

            // Find unvisited neighbors and calculate the risk value from start.
            var neighborPositions = FindNeighborPositions(currentNode.Position, width, height);
            foreach (var position in neighborPositions)
            {
                // Calculate the risk value from start for the node.
                var neighbor = nodeMap[position.Y, position.X];

                // Check the elevation difference between the current position and the potential target.
                // Skip if target is more than 1 unit higher.
                var elevationDifference = neighbor.Elevation - currentNode.Elevation;
                if (elevationDifference > 1)
                {
                    continue;
                }

                var previousStepsFromStart = neighbor.StepsFromStart;
                neighbor.StepsFromStart = Math.Min(neighbor.StepsFromStart, currentNode.StepsFromStart + 1);
                if (previousStepsFromStart != neighbor.StepsFromStart)
                {
                    // Need to move the item from old risk to new.
                    priorityMap[previousStepsFromStart].Remove(neighbor);
                    if (priorityMap[previousStepsFromStart].Count == 0)
                    {
                        priorityMap.Remove(previousStepsFromStart);
                    }

                    priorityMap.TryAdd(neighbor.StepsFromStart, new List<Node>());
                    priorityMap[neighbor.StepsFromStart].Add(neighbor);
                }
            }

            // Mark the current node as visited.
            currentNode.Visited = true;

            // Remove the current node from the minPrio list, this should remove it from the dictionary.
            minPrio.Value.RemoveAt(minPrio.Value.Count - 1);
            if (minPrio.Value.Count == 0)
            {
                priorityMap.Remove(minPrio.Key);
            }

            visitedNodes.Add(currentNode);

            //if (currentNode.Position == endPosition)
            //{
            //    // Break out immediately when end position found.
            //    break;
            //}
        } while (currentNode != null);

        // Search complete. We should now have the smallest risk value as the "RiskFromStart" value of the final position.
        var endNode = visitedNodes.FirstOrDefault(n => n.Value == 'E');

        return endNode?.StepsFromStart ?? -1;
    }

    /// <summary>
    /// Returns neighboring positions for the input Position, clamped to within the input bounds.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static Point2D[] FindNeighborPositions(Point2D position, int width, int height)
    {
        return new[]
        {
            position.Left(),
            position.Right(),
            position.Up(),
            position.Down()
        }.Where(p => p.IsInBounds(width, height)).ToArray();
    }

    public class Node
    {
        public Point2D Position { get; set; }

        public char Value { get; set; }

        /// <summary>
        /// Returns the elevation value for the Node.
        /// </summary>
        public int Elevation => Value switch
        {
            'S' => 0,
            'E' => 'z' - 'a',
            _ => Value - 'a'
        };

        /// <summary>
        /// Tracks the amount of steps that have to be taken from the start to get to this Node.
        /// </summary>
        public int StepsFromStart { get; set; }

        public bool Visited { get; set; }

        public bool PositionEquals(Point2D position) => Position.Equals(position);
    }

}
