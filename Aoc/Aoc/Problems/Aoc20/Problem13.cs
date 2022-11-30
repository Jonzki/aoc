using Aoc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc.Problems.Aoc20
{
    public class Problem13 : IProblem
    {
        public object Solve1(string input)
        {
            var lines = input.SplitLines();

            // The first line is your estimate of the earliest timestamp you could depart on a bus. 
            // Parse into a timestamp.
            var timestamp = int.Parse(lines[0]);

            // The second line lists the bus IDs that are in service according to the shuttle company; entries that show x must be out of service, so you decide to ignore them.
            var busIds = lines[1].Split(',').Where(id => id != "x").Select(int.Parse).ToArray();

            var referencePoint = 0;

            var minBusId = -1;
            var minBusDiff = int.MaxValue;

            foreach (var busId in busIds)
            {
                var nextDeparture = GetNextDeparture(busId, timestamp, referencePoint);
                if (nextDeparture - timestamp < minBusDiff)
                {
                    minBusDiff = nextDeparture - timestamp;
                    minBusId = busId;
                }
            }

            // What is the ID of the earliest bus you can take to the airport 
            // multiplied by the number of minutes you'll need to wait for that bus?
            return minBusId * minBusDiff;
        }

        public object Solve2(string input)
        {
            // The 2nd part runs into significant performance issues.
            throw new NotImplementedException();

            input = "asd\n7,13,x,x,59,x,31,19";

            // The first line in your input is no longer relevant.
            // The second line lists the bus IDs that are in service according to the shuttle company.
            // Keep the 'x' lines for now.
            var parts = input.SplitLines()[1].Split(',');

            // Parse buses.
            var buses = new List<Bus>();
            for (int i = 0; i < parts.Length; ++i)
            {
                if (parts[i] != "x")
                {
                    buses.Add(new Bus
                    {
                        Id = int.Parse(parts[i]),
                        Offset = i,
                        Timestamp = 0
                    });
                }
            }

            for (long t = 0; t < 1_000_000_000; ++t)
            {
                // Wind all buses forward past current timestamp.
                foreach (var bus in buses)
                {
                    while (bus.Timestamp < t)
                    {
                        bus.Timestamp += bus.Id;
                    }
                }

                if (t == 1068781)
                {

                }

                // Check for offsets.
                bool allMatch = true;
                for (int i = 1; i < buses.Count; ++i)
                {
                    if (buses[i].Timestamp - buses[0].Timestamp != buses[i].Offset)
                    {
                        allMatch = false;
                        break;
                    }
                }

                if (allMatch)
                {
                    Console.WriteLine("Found matching timestamp: " + t);
                    //return t;
                }
            }

            return -1;
        }

        public static int GetNextDeparture(int busId, int timestamp, int reference)
        {
            // Find the next departure time for the bus.
            int nextDeparture = reference;
            while (nextDeparture < timestamp)
            {
                nextDeparture += busId;
            }

            return nextDeparture;
        }

        record Bus
        {
            public int Id { get; set; }

            public int Offset { get; set; }

            public long Timestamp { get; set; }
        }
    }
}
