using AdventOfCode.Core;

namespace AdventOfCode.Y2024;

public class Day01 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var locationPairs = input.Select(line =>
        {
            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return (Left: int.Parse(parts[0]), Right: int.Parse(parts[1]));
        }).ToList();

        var leftLocations = locationPairs.Select(pair => pair.Left).OrderBy(id => id).ToList();
        var rightLocations = locationPairs.Select(pair => pair.Right).OrderBy(id => id).ToList();

        var totalDistance = leftLocations.Select((t, index) => Math.Abs(t - rightLocations[index])).Sum();

        var rightLocationFrequency = rightLocations
            .GroupBy(location => location)
            .ToDictionary(group => group.Key, group => group.Count());
        var similarityScore = leftLocations.Sum(location =>
            location * rightLocationFrequency.GetValueOrDefault(location, 0));

        return (totalDistance.ToString(), similarityScore.ToString());
    }
}