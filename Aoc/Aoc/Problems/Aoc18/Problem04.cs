using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aoc.Utils;

namespace Aoc.Problems.Aoc18;

public class Problem04 : IProblem
{
    public object Solve1(string input)
    {
        var guards = CalculateGuardStates(input);

        // Find out who slept the most?
        var mostAsleep = guards.OrderByDescending(g => g.TotalMinutesAsleep).First();

        // Multiply the guard ID by the most slept minute.
        return mostAsleep.Id * mostAsleep.GetMostSleptMinute().Minute;
    }

    public object Solve2(string input)
    {
        // For part 2, run the same calculations.
        var guards = CalculateGuardStates(input);

        // Now we are looking for the guard that sleeps most frequently on the same minute.
        var mostFrequent = guards.OrderByDescending(g => g.GetMostSleptMinute().Count).First();

        // For this guard, return the ID multiplied by the minute.
        return mostFrequent.Id * mostFrequent.GetMostSleptMinute().Minute;
    }

    class Guard
    {
        public Guard(int id)
        {
            Id = id;
            TotalMinutesAsleep = 0;
            SleepMinutes = new int[60];
        }

        public int Id { get; set; }

        public int TotalMinutesAsleep { get; set; }

        public int[] SleepMinutes { get; set; }

        /// <summary>
        /// Finds the most slept minute for the Guard.
        /// </summary>
        /// <returns></returns>
        public (int Minute, int Count) GetMostSleptMinute()
        {
            int maxMinute = 0, maxCount = 0;
            for (var i = 0; i < SleepMinutes.Length; ++i)
            {
                if (SleepMinutes[i] > maxCount)
                {
                    maxCount = SleepMinutes[i];
                    maxMinute = i;
                }
            }
            return (maxMinute, maxCount);
        }
    }

    private IEnumerable<Guard> CalculateGuardStates(string input)
    {
        // Parse the events.
        var events = ParseEvents(input);


        var guards = new Dictionary<int, Guard>();

        // Run through the inputs. First item is always a "begins shift" so we can skip it.
        for (var i = 1; i < events.Count; ++i)
        {
            // Make sure the guard exists.
            guards.TryAdd(events[i].GuardId, new Guard(events[i].GuardId));

            // Load up the guard.
            var guard = guards[events[i].GuardId];

            if (events[i - 1].EventType == EventType.FallsAsleep && events[i].EventType == EventType.WakesUp)
            {
                // Found sleep period. Calculate the duration.
                guard.TotalMinutesAsleep += (int)(events[i].Timestamp - events[i - 1].Timestamp).TotalMinutes;

                // Assign the minute division.
                for (int m = events[i - 1].Timestamp.Minute; m < events[i].Timestamp.Minute; ++m)
                {
                    guard.SleepMinutes[m]++;
                }
            }
        }

        return guards.Values;
    }

    /// <summary>
    /// Parses guard events from the input text.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private List<Event> ParseEvents(string input)
    {
        var events = new List<Event>();
        foreach (var line in input.SplitLines())
        {
            // Split timestamp and the rest.
            var parts = line.TrimStart('[').Split("] ");

            var e = new Event
            {
                GuardId = -1,
                Timestamp = DateTime.Parse(parts[0])
            };

            if (parts[1].EndsWith("begins shift"))
            {
                e.EventType = EventType.BeginsShift;

                // Parse the guard ID. This can be hacked by clever string removals.
                e.GuardId = parts[1].RemoveStrings("Guard #", " begins shift").ParseInt();
            }
            else if (parts[1] == "falls asleep")
            {
                e.EventType = EventType.FallsAsleep;
            }
            else if (parts[1] == "wakes up")
            {
                e.EventType = EventType.WakesUp;
            }
            else
            {
                throw new ArgumentException($"Input line cannot be parsed: '{line}'");
            }

            events.Add(e);
        }

        // Sort by timestamp.
        events = events.OrderBy(e => e.Timestamp).ToList();

        // Fix Guard IDs for each event.
        int guardId = -1;
        foreach (var e in events)
        {
            if (e.EventType == EventType.BeginsShift)
            {
                guardId = e.GuardId;
            }
            if (e.GuardId == -1)
            {
                e.GuardId = guardId;
            }
        }

        return events;
    }

    class Event
    {
        public DateTime Timestamp { get; set; }
        public int GuardId { get; set; }
        public EventType EventType { get; set; }
    }

    enum EventType
    {
        BeginsShift,
        FallsAsleep,
        WakesUp
    }

}
