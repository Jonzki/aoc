namespace Aoc.Problems.Aoc25;

public class Problem07 : IProblem
{
    public object Solve1(string input)
    {
        var map = Map.Parse(input);

        map.Simulate();

        // Count the amount of splits, which should be the amount of splitters reached.
        var split = map.Splitters.Count(s => map.Beams.Any(b => b.End.PositionEquals(s)));

        return split;
    }

    public object Solve2(string input)
    {
        var map = Map.Parse(input);

        // In part 2, we should simply calculate the paths cache for the map.
        // It will return the possible paths for the topmost splitter.
        var paths = map.CalculatePathCache();

        return paths;
    }

    public class Map
    {
        public List<Beam> Beams { get; set; } = new();

        public HashSet<Point2D> Splitters { get; set; } = new();

        public int Width { get; set; }

        public int Height { get; set; }

        /// <summary>
        /// Part 2: Cache of possible exit paths from any Splitter position.
        /// </summary>
        private Dictionary<Point2D, long> PathCache { get; set; } = new();

        /// <summary>
        /// Parses a Map from the input.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Map Parse(string input)
        {
            var lines = input.SplitLines();

            var map = new Map();

            map.Width = lines[0].Length;
            map.Height = lines.Length;

            for (var y = 0; y < map.Height; ++y)
            {
                for (var x = 0; x < map.Width; ++x)
                {
                    if (lines[y][x] == 'S')
                    {
                        // Found the beam starting position.
                        map.TryAddBeam(new Point2D(x, y));
                    }

                    if (lines[y][x] == '^')
                    {
                        // Found a splitter.
                        map.Splitters.Add(new Point2D(x, y));
                    }
                }
            }

            return map;
        }

        /// <summary>
        /// Prints the map.
        /// </summary>
        public void Print()
        {
            for (var y = 0; y < Height; ++y)
            {
                for (var x = 0; x < Width; ++x)
                {
                    var p = new Point2D(x, y);

                    if (Splitters.Contains(new Point2D(x, y)))
                    {
                        Console.Write('^');
                    }
                    else if (Beams.Any(b => p.IsWithin(b.Start, b.End)))
                    {
                        Console.Write('|');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Runs the Beam simulation.
        /// </summary>
        public void Simulate()
        {
            // Run the simulation until all Beams have stopped.
            bool moved;
            do
            {
                moved = SimulationCycle();
            } while (moved);
        }

        /// <summary>
        /// Runs a single simulation cycle on the Map.
        /// </summary>
        /// <returns>True if any Beam moved; false otherwise.</returns>
        public bool SimulationCycle()
        {
            var newBeams = new List<Point2D>();

            var moved = false;
            foreach (var beam in Beams)
            {
                // Skip beams that are already processed.
                if (beam.Stopped)
                {
                    continue;
                }

                // Advance the Beam down.
                moved = true;
                beam.End += Point2D.UnitDown;

                // If the Beam reached the bottom of the map, we can stop.
                if (beam.End.Y == Height - 1)
                {
                    beam.Stopped = true;
                }

                // If the beam reached a Splitter, we will stop it and spawn two new ones.
                if (Splitters.Contains(beam.End))
                {
                    beam.Stopped = true;
                    newBeams.Add(beam.End.Left());
                    newBeams.Add(beam.End.Right());
                }
            }

            // Add the new beams to the system.
            foreach (var beam in newBeams)
            {
                TryAddBeam(beam);
            }

            return moved;
        }

        /// <summary>
        /// Attempts to add a Beam to the given position.
        /// A Beam is not added if there is already a beam with this starting position.
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public bool TryAddBeam(Point2D start)
        {
            if (Beams.Any(b => b.Start.PositionEquals(start)))
            {
                return false;
            }

            Beams.Add(new Beam(start));
            return true;
        }

        /// <summary>
        /// Calculates the possible Paths cache for part 2.
        /// Returns the path count for the topmost splitter.
        /// </summary>
        public long CalculatePathCache()
        {
            PathCache.Clear();

            // Calculate the path cache from the topmost splitter.
            // Will recursively calculate from there.
            var startSplitter = Splitters.MinBy(x => x.Y);

            CalculatePathCount(startSplitter);

            return PathCache[startSplitter];
        }

        /// <summary>
        /// Calculates the available paths count for the input Splitter position.
        /// </summary>
        /// <param name="splitter"></param>
        public long CalculatePathCount(Point2D splitter)
        {
            if (PathCache.TryGetValue(splitter, out var pathCount))
            {
                return pathCount;
            }

            pathCount = 0;

            // Run the simulation for the two possible paths from this Splitter: left and right
            var leftPath = splitter.Left();
            for (; leftPath.Y < Height; leftPath = leftPath.Down())
            {
                if (Splitters.Contains(leftPath))
                {
                    pathCount += CalculatePathCount(leftPath);
                    break;
                }
            }
            if (leftPath.Y == Height)
            {
                pathCount += 1;
            }

            var rightPath = splitter.Right();
            for (; rightPath.Y < Height; rightPath = rightPath.Down())
            {
                if (Splitters.Contains(rightPath))
                {
                    pathCount += CalculatePathCount(rightPath);
                    break;
                }
            }
            if (rightPath.Y == Height)
            {
                pathCount += 1;
            }

            PathCache[splitter] = pathCount;

            return pathCount;
        }
    }

    public class Beam(Point2D start)
    {
        public Point2D Start { get; init; } = start;

        public Point2D End { get; set; } = start;

        public bool Stopped { get; set; } = false;
    }
}
