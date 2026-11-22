namespace Aoc.Problems.Aoc25;

/// <summary>
/// https://adventofcode.com/2025/day/8
/// </summary>
public class Problem08 : IProblem
{
    public object Solve1(string input)
    {
        var map = ParseMap(input);

        Console.WriteLine("Calculating distance pairs..");
        map.CalculatePointPairs();

        Console.WriteLine("Applying connections..");
        ApplyConnections(map, 1000);

        Console.WriteLine("Calculating circuit sizes.");
        var sizes = map.GetCircuitSizes();

        // We are interested in the three largest.
        if (sizes.Count < 3)
        {
            Console.WriteLine("WARNING: less than 3 circuits formed, cannot multiply.");
            return 0;
        }

        return 1L * sizes[0] * sizes[1] * sizes[2];
    }

    public object Solve2(string input)
    {
        var map = ParseMap(input);

        Console.WriteLine("Calculating distance pairs..");
        map.CalculatePointPairs();
        Console.WriteLine($"> {map.PointPairsByDistance.Count} point pairs calculated.");

        // In part 2, we want to keep applying connections until we end up with one big circuit.
        Console.WriteLine("Applying connections..");
        for (var i = 0; i < map.PointPairsByDistance.Count; ++i)
        {
            ApplySingleConnection(map, i);

            if (map.GetCircuitSizes().Count == 1)
            {
                var finalPair = map.PointPairsByDistance[i];
                var pointA = map.Points.First(p => p.Id == finalPair.PointA);
                var pointB = map.Points.First(p => p.Id == finalPair.PointB);

                Console.WriteLine($"All junctions connected on iteration {i}!");
                Console.WriteLine($"Point 1: {pointA}");
                Console.WriteLine($"Point 1: {pointB}");

                // Result is X coordinates multiplied.
                return pointA.X * pointB.X;
            }
        }

        Console.WriteLine("No single circuit after all connections!");
        return -1;
    }

    public static void ApplyConnections(Map map, int iterations)
    {
        for (var i = 0; i < iterations; ++i)
        {
            ApplySingleConnection(map, i);

            if (i % 10 == 0 || iterations - i < 10)
            {
                var sizes = map.GetCircuitSizes();
                Console.WriteLine($"Iteration {i}/{iterations}: {sizes.Count} circuits. Top 3: {string.Join(", ", sizes.Take(3))}");
            }
        }
    }

    /// <summary>
    /// Connects a single pair of closest "not already connected" closest points together.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="pairIndex">Index of the Point Pair to process.</param>
    public static void ApplySingleConnection(Map map, int pairIndex)
    {
        // Find a point pair where both points are not connected to anything (circuit zero)
        // or are connected to different circuits.
        var (a, b, _) = map.PointPairsByDistance[pairIndex];

        var circuitA = map.GetCircuitId(a);
        var circuitB = map.GetCircuitId(b);

        if (circuitA == circuitB)
        {
            if (circuitA != 0)
            {
                // Same non-zero circuit - nothing happens.
            }
            else
            {
                // Both points unconnected - get a new ID and connect them.
                var id = map.GetNextCircuitId();
                map.SetCircuitId(a, id);
                map.SetCircuitId(b, id);
            }
        }
        else
        {
            // Different circuit IDs. Circuits should be merged.
            // Easy merges first: check either point is zero (both can't be zero in this branch)
            if (circuitA == 0)
            {
                map.SetCircuitId(a, circuitB);
            }
            else if (circuitB == 0)
            {
                map.SetCircuitId(b, circuitA);
            }
            else
            {
                // Found two non-zero circuits. Merge them by iterating all points in both circuits and moving them to a new circuit.
                var id = map.GetNextCircuitId();
                foreach (var point in map.Points)
                {
                    if (point.CircuitId == circuitA || point.CircuitId == circuitB)
                    {
                        map.SetCircuitId(point.Id, id);
                    }
                }
            }
        }
    }

    public static Map ParseMap(string input)
    {
        var lines = input.SplitLines();

        var map = new Map
        {
            Points = new Point3D[lines.Length]
        };

        for (var i = 0; i < lines.Length; ++i)
        {
            var parts = lines[i].Split(',').Select(int.Parse).ToArray();
            map.Points[i] = new Point3D { X = parts[0], Y = parts[1], Z = parts[2], Id = i };
        }

        return map;
    }

    /// <summary>
    /// Calculates the Euclidean distance between two Points.
    /// </summary>
    /// <param name="pointA"></param>
    /// <param name="pointB"></param>
    /// <returns></returns>
    public static double EuclideanDistance(Point3D pointA, Point3D pointB)
    {
        return Math.Sqrt(
            Math.Pow(pointB.X - pointA.X, 2)
            +
            Math.Pow(pointB.Y - pointA.Y, 2)
            +
            Math.Pow(pointB.Z - pointA.Z, 2)
        );
    }

    /// <summary>
    /// 3D point with a unique ID and an updatable Circuit ID.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public class Point3D
    {
        /// <summary>
        /// Unique ID of this Point.
        /// </summary>
        public int Id { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        /// <summary>
        /// ID of the Circuit this Point currently belongs in.
        /// Zero == not in any circuit.
        /// </summary>
        public int CircuitId { get; set; } = 0;

        public override string ToString() => string.Join(',', X, Y, Z);
    }

    public class Map
    {
        private int CircuitIdCounter { get; set; } = 0;

        public required Point3D[] Points { get; init; }

        /// <summary>
        /// Distance between each Point. PointA and PointB are IDs.
        /// </summary>
        public List<(int PointA, int PointB, double distance)> PointPairsByDistance { get; private set; } = new();

        /// <summary>
        /// Calculates a list of Point Pairs ordered by distance.
        /// </summary>
        public void CalculatePointPairs()
        {
            PointPairsByDistance.Clear();

            var pairs = new List<(int PointA, int PointB, double Distance)>();

            for (var i = 0; i < Points.Length; i++)
            {
                for (var j = i; j < Points.Length; ++j)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    pairs.Add((i, j, EuclideanDistance(Points[i], Points[j])));
                }
            }

            PointPairsByDistance = pairs.OrderBy(p => p.Distance).ToList();
        }

        /// <summary>
        /// Returns a new unique circuit counter.
        /// </summary>
        /// <returns></returns>
        public int GetNextCircuitId() => ++CircuitIdCounter;

        public int GetCircuitId(int pointId) => Points[pointId].CircuitId;

        public void SetCircuitId(int pointId, int circuitId) => Points[pointId].CircuitId = circuitId;

        /// <summary>
        /// Returns all Circuit sizes.
        /// </summary>
        /// <returns></returns>
        public List<int> GetCircuitSizes()
        {
            int singleCount = 0;
            Dictionary<int, int> groups = new();

            foreach (var point in Points)
            {
                if (point.CircuitId == 0)
                {
                    singleCount++;
                }
                else
                {
                    groups.TryAdd(point.CircuitId, 0);
                    groups[point.CircuitId] = groups[point.CircuitId] + 1;
                }
            }

            var circuitSizes = new List<int>();

            circuitSizes.AddRange(groups.Values.OrderByDescending(x => x));
            for (var i = 0; i < singleCount; ++i)
            {
                circuitSizes.Add(1);
            }

            return circuitSizes;
        }

        public int GetCircuitSize(int circuitId) => Points.Count(p => p.CircuitId == circuitId);
    }
}
