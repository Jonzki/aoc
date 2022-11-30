using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aoc.Utils;

namespace Aoc.Problems.Aoc21;

public class Problem17 : IProblem
{
    public object Solve1(string input)
    {
        var coords = InputReader.ParseNumberList(input, ",");
        var target = new Target(coords[0], coords[1], coords[2], coords[3]);

        // For part 1, we don't actually have to care about the X velocity.
        // We should be shooting so high that we can always hit the target in terms of horizontal positioning.
        // Iterate on the Y velocity.

        var verticalVelos = ResolveVerticalVelocities(target);

        var maxHeight = verticalVelos.Max(x => x.maxHeight);

        Debug.WriteLine($"Max height after iterations: {maxHeight}.");
        return maxHeight;
    }

    public object Solve2(string input)
    {
        var coords = InputReader.ParseNumberList(input, ",");
        var target = new Target(coords[0], coords[1], coords[2], coords[3]);

        // For part 2, first resolve all Y velocities that hit the target, similar to part 1.
        // Then, fill in X velocities that can hit.

        var verticalVelos = ResolveVerticalVelocities(target, 250);

        Debug.WriteLine($"Found {verticalVelos.Count} Y velocities that hit.");

        // Then, count all X velocities that hit.
        int hitCount = 0;

        Debug.WriteLine($"Testing {verticalVelos.Count * target.MaxX} trajectories..");
        foreach (var y in verticalVelos)
        {
            // Generous limits: X velocity must be between 1 and target.MinX to have any chance of hitting.
            // This definitely scans too high horizontal velocities.
            for (var vX = 1; vX <= target.MaxX; ++vX)
            {
                // Simulate the probe.
                var probe = new Probe
                {
                    Position = new Point2D(0, 0),
                    Velocity = new Point2D(vX, y.vY)
                };
                var hit = Simulate(probe, target);
                if (hit)
                {
                    // Store the velocity in the output.
                    ++hitCount;

                    //Debug.WriteLine($"Hit on {new Point2D(vX, y.vY)}, up to {hitCount}.");
                }
            }
        }

        return hitCount;
    }

    public List<(int vY, int maxHeight)> ResolveVerticalVelocities(Target target, int maxConsecutiveMisses = 100)
    {
        var output = new List<(int vY, int maxHeight)>();

        var yLimit = 5000;

        var firstHit = false;
        var consecutiveMisses = 0;

        // Start from target.MinY - we might shoot directly at the target.
        for (int vY = target.MinY; vY <= yLimit; ++vY)
        {
            // Generate a probe and simulate it's flight.
            // Put X within the target and X-velocity to zero.
            var probe = new Probe
            {
                Position = new Point2D(target.MinX + 1, 0),
                Velocity = new Point2D(0, vY)
            };
            var didHit = Simulate(probe, target);

            if (didHit)
            {
                // Mark first hit as true.
                firstHit = true;

                //Debug.WriteLine($"Velocity {vY} hits target and reaches {probe.MaxHeight}.");
                output.Add((vY, probe.MaxHeight));

                // Reset miss counter.
                consecutiveMisses = 0;
            }
            else
            {
                consecutiveMisses++;
            }

            if (firstHit && consecutiveMisses > maxConsecutiveMisses)
            {
                //Debug.WriteLine($"{consecutiveMisses} misses - stop looking.");
                break;
            }
        }

        return output;
    }

    public bool Simulate(Probe probe, Target target)
    {
        (bool Hit, bool CanStillHit) CheckForHit()
        {
            var hit = probe.Position.X.BetweenInclusive(target.MinX, target.MaxX)
                   && probe.Position.Y.BetweenInclusive(target.MinY, target.MaxY);

            if (hit)
            {
                // We have hit the target.
                return (true, true);
            }

            // Resolve whether we can still hit the target.
            // We have missed if X has gone past maxX, or Y has gone below minY.
            var canStillHit = probe.Position.X < target.MaxX && probe.Position.Y > target.MinY;

            return (hit, canStillHit);
        }

        // Put a high limit on the simulation.
        const int MaxIterations = 1_000_000;
        for (var i = 0; i < MaxIterations; ++i)
        {
            // Update the probe.
            probe.Update();

            // Check if we hit, and whether we still can.
            var (hit, canHit) = CheckForHit();

            if (hit) { return true; }
            if (!canHit) { return false; }
        }

        throw new InvalidOperationException("Reached maximum iterations.");
    }

    public class Probe
    {
        public Point2D Velocity { get; set; }

        public Point2D Position { get; set; }

        public int MaxHeight { get; set; } = 0;

        public void Update()
        {
            // Update position by current velocity.
            Position += Velocity;
            // Decrease horizontal velocity by 1 towards 0.
            if (Velocity.X > 0) Velocity.X--;
            // Decrease Y velocity by 1.
            Velocity.Y--;

            // Track max height.
            if (Position.Y > MaxHeight) MaxHeight = Position.Y;
        }
    }

    public class Target
    {
        public Target(int minX, int maxX, int minY, int maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }

        public int MinX { get; }
        public int MaxX { get; }
        public int MinY { get; }
        public int MaxY { get; }
    }
}
