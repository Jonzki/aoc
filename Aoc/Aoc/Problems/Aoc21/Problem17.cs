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
        var coords = ProblemInput.ParseNumberList(input, ",");
        var target = new Target(coords[0], coords[1], coords[2], coords[3]);

        // For part 1, we don't actually have to care about the X velocity.
        // We should be shooting so high that we can always hit the target in terms of horizontal positioning.
        // Iterate on the Y velocity.

        var yLimit = 5000;

        var maxHeight = 0;
        var maxHeightVelocityY = 0;

        var firstHit = false;
        var maxConsecutive = 100;
        var consecutiveMisses = 0;

        for (int vY = 1; vY < yLimit; ++vY)
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

                Console.WriteLine($"Velocity {vY} hits target and reaches {probe.MaxHeight}.");
                if (probe.MaxHeight > maxHeight)
                {
                    maxHeight = probe.MaxHeight;
                    maxHeightVelocityY = vY;
                }

                consecutiveMisses = 0;
            }
            else
            {
                consecutiveMisses++;
            }

            if (firstHit && consecutiveMisses > maxConsecutive)
            {
                Debug.WriteLine($"{consecutiveMisses} misses - stop looking.");
                break;
            }
        }

        Debug.WriteLine($"Max height after {yLimit} iterations: {maxHeight} on velocity {maxHeightVelocityY}");
        return maxHeight;
    }

    public object Solve2(string input)
    {
        throw new NotImplementedException();
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
