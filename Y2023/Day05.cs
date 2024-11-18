using AdventOfCode.Core;

namespace AdventOfCode.Y2023;

public class Day05 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var seeds = ParseSeeds(input[0]);
        var maps = ParseMaps(input);

        var part1 = seeds.Min(seed => GetLocation(seed, maps));
        var part2 = GetMinLocationFromRanges(seeds, maps);

        return (part1.ToString(), part2.ToString());
    }

    private static List<long> ParseSeeds(string line)
    {
        return line.Split(':')[1].Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();
    }

    private static List<List<MapRange>> ParseMaps(string[] input)
    {
        var maps = new List<List<MapRange>>();
        var currentMap = new List<MapRange>();

        foreach (var line in input.Skip(2))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.EndsWith("map:"))
            {
                if (currentMap.Any())
                {
                    maps.Add(currentMap);
                }

                currentMap = [];
                continue;
            }

            var parts = line.Split(' ').Select(long.Parse).ToArray();
            currentMap.Add(new MapRange(parts[1], parts[0], parts[2]));
        }

        if (currentMap.Any())
        {
            maps.Add(currentMap);
        }

        return maps;
    }

    private static long GetLocation(long seed, List<List<MapRange>> maps)
    {
        var value = seed;
        foreach (var map in maps)
        {
            foreach (var range in map)
            {
                if (value < range.Source || value >= range.Source + range.Length)
                {
                    continue;
                }

                value = range.Dest + (value - range.Source);
                break;
            }
        }

        return value;
    }

    private static long GetMinLocationFromRanges(List<long> seeds, List<List<MapRange>> maps)
    {
        var ranges = new List<SeedRange>();
        for (var i = 0; i < seeds.Count; i += 2)
        {
            ranges.Add(new SeedRange(seeds[i], seeds[i + 1]));
        }

        foreach (var map in maps)
        {
            var newRanges = new List<SeedRange>();
            foreach (var range in ranges)
            {
                var current = range.Start;
                var remaining = range.Length;

                while (remaining > 0)
                {
                    var (start, length) = GetNextRange(current, map);
                    length = Math.Min(remaining, length);
                    newRanges.Add(new SeedRange(start, length));
                    current += length;
                    remaining -= length;
                }
            }

            ranges = newRanges;
        }

        return ranges.Min(r => r.Start);
    }

    private static (long Start, long Length) GetNextRange(long value, List<MapRange> ranges)
    {
        foreach (var range in ranges.OrderBy(r => r.Source))
        {
            if (value >= range.Source && value < range.Source + range.Length)
            {
                return (range.Dest + (value - range.Source), range.Source + range.Length - value);
            }

            if (value < range.Source)
            {
                return (value, range.Source - value);
            }
        }

        return (value, long.MaxValue);
    }

    private record struct SeedRange(long Start, long Length);

    private record struct MapRange(long Source, long Dest, long Length);
}