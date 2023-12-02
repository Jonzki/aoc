namespace Aoc.Problems.Aoc18;

/// <summary>
/// https://adventofcode.com/2018/day/4
/// </summary>
public class Problem04 : IProblem
{
    // Part 1:
    // Find the guard that has the most minutes asleep.
    // What minute does that guard spend asleep the most?
    public object Solve1(string input)
    {
        var (events, guards) = ParseInput(input);

        // First event is a "begins shift" which we can ignore.
        for (var i = 1; i < events.Count; ++i)
        {
            var guard = guards[events[i].GuardId];
            if (events[i - 1].Type == EventType.FallsAsleep && events[i].Type == EventType.WakesUp)
            {
                // Found a sleep period.
                for (var m = events[i - 1].Timestamp.Minute; m < events[i].Timestamp.Minute; ++m)
                {
                    ++guard.TotalMinutesAsleep;
                    guard.SleepMinutes.TryAdd(m, 0);
                    guard.SleepMinutes[m] += 1;
                }
            }
        }

        var mostSleepyGuard = guards.Values.MaxBy(g => g.TotalMinutesAsleep);
        var mostSleptMinute = mostSleepyGuard.SleepMinutes.MaxBy(x => x.Value).Key;

        return mostSleepyGuard.Id * mostSleptMinute;
    }

    // Part 2:
    // Of all guards, which guard is most frequently asleep on the same minute?
    public object Solve2(string input)
    {
        var (events, guards) = ParseInput(input);

        // Run the same simulation.
        // First event is a "begins shift" which we can ignore.
        for (var i = 1; i < events.Count; ++i)
        {
            var guard = guards[events[i].GuardId];
            if (events[i - 1].Type == EventType.FallsAsleep && events[i].Type == EventType.WakesUp)
            {
                // Found a sleep period.
                for (var m = events[i - 1].Timestamp.Minute; m < events[i].Timestamp.Minute; ++m)
                {
                    ++guard.TotalMinutesAsleep;
                    guard.SleepMinutes.TryAdd(m, 0);
                    guard.SleepMinutes[m] += 1;
                }
            }
        }

        var mostCommonMinute = -1;
        var mostCommonCount = 0;
        var mostCommonGuardId = -1;

        foreach (var g in guards.Values)
        {
            foreach (var m in g.SleepMinutes)
            {
                if (m.Value > mostCommonCount)
                {
                    mostCommonGuardId = g.Id;
                    mostCommonMinute = m.Key;
                    mostCommonCount = m.Value;
                }
            }
        }

        return mostCommonGuardId * mostCommonMinute;
    }

    public static (List<Event> Events, Dictionary<int, Guard> Guards) ParseInput(string input)
    {
        var rawInput = new List<(DateTime Timestamp, string Text)>();

        foreach (var line in input.SplitLines())
        {
            var parts = line.TrimStart('[').Split(']');
            rawInput.Add((DateTime.Parse(parts[0]), parts[1].Trim()));
        }

        rawInput = rawInput.OrderBy(x => x.Timestamp).ToList();
        var events = new List<Event>();

        int guardId = 0;
        foreach (var (timestamp, text) in rawInput)
        {
            EventType eventType;
            // Check if a new guard begins shift.
            if (text.StartsWith("Guard #"))
            {
                eventType = EventType.BeginShift;
                guardId = int.Parse(text.Substring("Guard #".Length).Split(' ')[0]);
            }
            else if (text.EndsWith("falls asleep"))
            {
                eventType = EventType.FallsAsleep;
            }
            else if (text.EndsWith("wakes up"))
            {
                eventType = EventType.WakesUp;
            }
            else
            {
                throw new InvalidOperationException($"Unrecognized text: '{text}'");
            }

            if (guardId == 0)
            {
                throw new InvalidOperationException("GuardId was not resolved.");
            }

            events.Add(new Event(timestamp, guardId, eventType));
        }

        var guards = events
            .Select(e => e.GuardId)
            .Distinct()
            .ToDictionary(
                id => id,
                id => new Guard(id)
            );

        return (events, guards);
    }

    public enum EventType
    {
        BeginShift, FallsAsleep, WakesUp
    }

    public record struct Event
    (
        DateTime Timestamp,
        int GuardId,
        EventType Type
    );

    public class Guard
    {
        public Guard(int id)
        {
            Id = id;
            TotalMinutesAsleep = 0;
            SleepMinutes = new();
        }

        public int Id;
        public int TotalMinutesAsleep;
        public Dictionary<int, int> SleepMinutes;
    }
}
