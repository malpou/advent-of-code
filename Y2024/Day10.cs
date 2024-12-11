using AdventOfCode.Core;
using AdventOfCode.Utils;

namespace AdventOfCode.Y2024;

public class Day10 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var height = input.Length;
        var width = input[0].Length;

        var (reachableNines, distinctPaths) = Enumerable.Range(0, height)
            .SelectMany(y => Enumerable.Range(0, width)
                .Where(x => input[y][x] == '0')
                .Select(x => new Point(x, y, width, height)))
            .Select(trailhead =>
            {
                var reachableNines = new HashSet<Point>();
                var distinctPaths = new HashSet<string>();
                var currentPath = new List<Point>();
                ExploreTrails(trailhead, input, currentPath, reachableNines, distinctPaths);
                return (reachableNines.Count, distinctPaths.Count);
            })
            .Aggregate(
                (ReachableNines: 0, DistinctPaths: 0),
                (acc, stats) => (
                    acc.ReachableNines + stats.Item1,
                    acc.DistinctPaths + stats.Item2
                )
            );

        return (reachableNines.ToString(), distinctPaths.ToString());
    }

    private static void ExploreTrails(
        Point current,
        string[] input,
        List<Point> currentPath,
        HashSet<Point> reachableNines,
        HashSet<string> distinctPaths)
    {
        currentPath.Add(current);

        if (input[current.Y][current.X] == '9')
        {
            reachableNines.Add(current);
            var pathString = string.Join(";", currentPath.Select(p => $"{p.X},{p.Y}"));
            distinctPaths.Add(pathString);
            currentPath.RemoveAt(currentPath.Count - 1);
            return;
        }

        var currentHeight = input[current.Y][current.X] - '0';

        foreach (var direction in Vector.CardinalDirections)
        {
            var next = current + direction;

            if (!next.IsInBounds() || currentPath.Contains(next))
            {
                continue;
            }

            var nextHeight = input[next.Y][next.X] - '0';

            if (nextHeight == currentHeight + 1)
            {
                ExploreTrails(next, input, currentPath, reachableNines, distinctPaths);
            }
        }

        currentPath.RemoveAt(currentPath.Count - 1);
    }
}