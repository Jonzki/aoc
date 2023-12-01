namespace Aoc.Problems.Aoc21;

public class Problem15 : IProblem
{
    public const string MiniInput = @"12
11";

    public const string MiniInput2 = @"123
111
321";


    public const string MazeInput = @"19111
19191
11191";

    public const string SmallInput = @"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581";


    public object Solve1(string input)
    {
        var (nodes, width, height) = ParseMap1(input);

        var risk = FindBestPathDijkstra(nodes);

        return risk;
    }

    public object Solve2(string input)
    {
        var (nodes, width, height) = ParseMap2(input);

        var risk = FindBestPathDijkstra(nodes);

        return risk;
    }

    /// <summary>
    /// Parses a map of Nodes from the problem input (Dijkstra approach).
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static (List<Node> Nodes, int Width, int Height) ParseMap1(string input)
    {
        var lines = input.SplitLines();
        var width = lines[0].Length;
        var height = lines.Length;

        var temp = lines.SelectMany(l => l.Select(c => (c - '0'))).ToArray();

        var points = ArrayUtils.To2D(temp, width, height);

        var nodes = new List<Node>();
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                nodes.Add(new Node
                {
                    Position = new Point2D(x, y),
                    Risk = points[y, x],
                    RiskFromStart = 1_000_000_000,
                    Visited = false
                });
            }
        }

        return (nodes, width, height);
    }

    /// <summary>
    /// Parses a map of Nodes for part 2 of the problem.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static (List<Node> Nodes, int Width, int Height) ParseMap2(string input)
    {
        var lines = input.SplitLines();
        var width = lines[0].Length;
        var height = lines.Length;

        var temp = lines.SelectMany(l => l.Select(c => (c - '0'))).ToArray();

        var points = ArrayUtils.To2D(temp, width, height);

        var nodes = new List<Node>();

        // Run a 5 times multiplier for the coordinates.
        // This makes the map way larger.
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                for (int y2 = 0; y2 < 5; y2++)
                {
                    for (var x2 = 0; x2 < 5; x2++)
                    {
                        var riskValue = points[y, x] + x2 + y2;
                        if (riskValue > 9) riskValue -= 9;

                        nodes.Add(new Node
                        {
                            Position = new Point2D(x + x2 * width, y + y2 * height),
                            Risk = riskValue,
                            RiskFromStart = 1_000_000_000,
                            Visited = false
                        });
                    }
                }
            }
        }

        nodes = nodes.OrderBy(n => n.Position.Y).ThenBy(n => n.Position.X).ToList();

        return (nodes, width, height);
    }

    public static int FindBestPathDijkstra(List<Node> input)
    {
        // Resolve the end position first.
        var maxX = input.Max(n => n.Position.X);
        var maxY = input.Max(n => n.Position.Y);

        var width = maxX + 1;
        var height = maxY + 1;

        var endPosition = new Point2D(maxX, maxY);

        List<Node> visitedNodes = new List<Node>();

        // Construct a positional lookup (this leverages C# references).
        var nodeMap = new Node[height, width];
        // Also construct a priority queue
        var priorityMap = new SortedDictionary<int, List<Node>>();

        foreach (var node in input)
        {
            // Set the starting risk.
            if (node.Position.PositionEquals(0, 0))
            {
                node.RiskFromStart = 0;
            }

            nodeMap[node.Position.X, node.Position.Y] = node;

            priorityMap.TryAdd(node.RiskFromStart, new List<Node>());
            priorityMap[node.RiskFromStart].Add(node);
        }

        // Run until we reach the target.
        Node currentNode;
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
                var neighbor = nodeMap[position.X, position.Y];

                var previousRisk = neighbor.RiskFromStart;
                neighbor.RiskFromStart = Math.Min(neighbor.RiskFromStart, currentNode.RiskFromStart + neighbor.Risk);
                if (previousRisk != neighbor.RiskFromStart)
                {
                    // Need to move the item from old risk to new.
                    priorityMap[previousRisk].Remove(neighbor);
                    if (priorityMap[previousRisk].Count == 0)
                    {
                        priorityMap.Remove(previousRisk);
                    }


                    priorityMap.TryAdd(neighbor.RiskFromStart, new List<Node>());
                    priorityMap[neighbor.RiskFromStart].Add(neighbor);
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
        var endNode = visitedNodes.FirstOrDefault(n => n.Position == endPosition);

        return endNode?.RiskFromStart ?? -1;
    }

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

        public int Risk { get; set; }
        public int RiskFromStart { get; set; }

        public bool Visited { get; set; }
    }
}
