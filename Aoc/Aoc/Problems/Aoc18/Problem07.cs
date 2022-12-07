using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Aoc.Utils;

namespace Aoc.Problems.Aoc18;

public class Problem07 : IProblem
{
    public object Solve1(string input)
    {
        var graph = BuildGraph(input);

        // Track the node order.
        var order = new List<char>();

        // 
        while (true)
        {
            var workableIds = GetWorkableNodes(graph, x => !x.Done);
            if (workableIds.Count == 0)
            {
                // All nodes done.
                break;
            }

            var potentialIds = new List<char>();
            foreach (var id in workableIds)
            {
                // If there are no previous nodes, this must be the starting node.
                if (graph[id].PreviousNodes.Count == 0)
                {
                    potentialIds.Add(id);
                }
                else if (graph[id].PreviousNodes.All(previousId => graph[previousId].Done))
                {
                    // Found a node whose all prerequisites are done.
                    potentialIds.Add(id);
                }
            }

            if (potentialIds.Count > 0)
            {
                // Sort the potential nodes alphabetically, then pick the first one.
                var firstId = potentialIds.OrderBy(id => id).First();

                // Add the first potential Node into the output order and mark it done.
                order.Add(firstId);
                graph[firstId].Done = true;
            }
            else
            {
                // No nodes to complete - probably an issue?
                break;
            }
        }

        return new string(order.ToArray());
    }

    public object Solve2(string input)
    {
        // Node work cost: base + charCode (A = 1)
        var baseCost = 60;
        // Amount of workers.
        var workerCount = 5;

        // Start with the same input.
        var graph = this.BuildGraph(input);

        // Set up work amounts and worker allocations.
        foreach (var node in graph.Values)
        {
            node.Work = baseCost + (node.Id - 'A') + 1;
            node.Done = false;
            node.WorkerId = -1;
        }

        // Set up workers.
        // Index = worker ID
        // Value = Node the Worker is currently working on. '-' for no work.
        var workers = new char[workerCount];
        Array.Fill(workers, '-');

        // Run the simulation.
        var second = 0;
        while (true)
        {
            // Get nodes with work remaining and no workers assigned.
            var workableIds = GetWorkableNodes(graph, node => node.Work > 0 && node.WorkerId == -1);
            if (workableIds.Count == 0)
            {
                // console.log('All nodes done.');
                break;
            }

            // Check for prerequisites.
            foreach (var workableId in workableIds)
            {
                var canWork = false;
                if (graph[workableId].PreviousNodes.Count == 0)
                {
                    // Starting node.
                    canWork = true;
                }
                else if (graph[workableId].PreviousNodes.All(id => graph[id].Work == 0))
                {
                    // All prerequisites done (work remaining = 0).
                    canWork = true;
                }

                if (canWork)
                {
                    // Assign a worker for the node (if it's still available).
                    for (var i = 0; i < workers.Length; ++i)
                    {
                        if (workers[i] == '-' && graph[workableId].WorkerId == -1)
                        {
                            // console.log(`Worker ${workers[i].id} takes node ${id} at ${second}.`);
                            workers[i] = workableId;
                            graph[workableId].WorkerId = i;
                        }
                    }
                }
            };

            // Update work amounts & allocations.
            for (var i = 0; i < workers.Length; ++i)
            {
                if (workers[i] != '-')
                {
                    // Reduce remaining work by 1.
                    graph[workers[i]].Work--;
                    // If the node got finished, free up the worker.
                    if (graph[workers[i]].Work == 0)
                    {
                        // console.log(`Worker ${worker.id} finished node ${worker.node} at ${second}.`);
                        workers[i] = '-';
                    }
                }
            }

            ++second;
        }

        // All work completed, return the amount of seconds this took.
        return second;
    }

    /// <summary>
    /// Constructs a graph from the input.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private Dictionary<char, GraphNode> BuildGraph(string input)
    {
        var nodes = new Dictionary<char, GraphNode>();

        // Preprocessing: remove unnecessary bits, then split lines.
        var lines = input
            .RemoveString("Step ")
            .RemoveString(" must be finished before step ")
            .RemoveString(" can begin.")
            .SplitLines();

        foreach (var line in lines)
        {
            // "before" must be finished before "node" can begin.
            var before = line[0];
            var node = line[1];

            // Set up both nodes.
            nodes.TryAdd(before, new GraphNode(before));
            nodes.TryAdd(node, new GraphNode(node));

            nodes[before].NextNodes.Add(node);
            nodes[node].PreviousNodes.Add(before);
        }

        return nodes;
    }

    private List<char> GetWorkableNodes(Dictionary<char, GraphNode> graph, Func<GraphNode, bool> expression)
    {
        return graph.Values.Where(expression).Select(x => x.Id).OrderBy(c => c).ToList();
    }

    class GraphNode
    {
        public GraphNode(char id)
        {
            Id = id;
            Done = false;
            PreviousNodes = new List<char>();
            NextNodes = new List<char>();
        }

        /// <summary>
        /// Node state.
        /// </summary>
        public bool Done { get; set; }

        /// <summary>
        /// Node ID.
        /// </summary>
        public char Id { get; set; }

        /// <summary>
        /// Prerequisite node IDs.
        /// </summary>
        public List<char> PreviousNodes { get; set; }

        /// <summary>
        /// Potential followup node IDs.
        /// </summary>
        public List<char> NextNodes { get; set; }

        public int Work { get; set; }

        public int WorkerId { get; set; }
    }

}
