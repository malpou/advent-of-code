using AdventOfCode.Core;
using AdventOfCode.Utils;

namespace AdventOfCode.Y2024;

public class Day08 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var (xMax, yMax) = (input[0].Length, input.Length);

        var antennaGroups =
            from x in Enumerable.Range(0, xMax)
            from y in Enumerable.Range(0, yMax)
            let point = new Point(x, y, xMax, yMax)
            let character = input[x][y]
            where character != '.'
            group point by character
            into g
            where g.Count() > 1
            select g;

        var result = antennaGroups.Aggregate(
            new { FirstNodes = new HashSet<Point>(), AllNodes = new HashSet<Point>() },
            (accumulator, antennaGroup) =>
            {
                accumulator.AllNodes.UnionWith(antennaGroup);

                var antennaPairs = antennaGroup.GetUniquePairs();
                foreach (var (antenna1, antenna2) in antennaPairs)
                {
                    var nodes = GetAntiNodes(antenna1, antenna2);
                    accumulator.FirstNodes.UnionWith(nodes.Firsts);
                    accumulator.AllNodes.UnionWith(nodes.All);
                }

                return accumulator;
            });

        return (result.FirstNodes.Count.ToString(), result.AllNodes.Count.ToString());
    }

    private static (IEnumerable<Point> Firsts, IEnumerable<Point> All) GetAntiNodes(Point antenna1, Point antenna2)
    {
        var vector = antenna1.GetVector(antenna2);
        var antiNodes1 = antenna1.GetPointsInDirection(vector.OppositeDirection());
        var antiNodes2 = antenna2.GetPointsInDirection(vector);
        return (Firsts: [.. antiNodes1.Take(1), .. antiNodes2.Take(1)], All: [..antiNodes1, ..antiNodes2]);
    }
}