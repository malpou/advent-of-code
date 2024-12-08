using AdventOfCode.Core;
using AdventOfCode.Utils;

namespace AdventOfCode.Y2024;

public class Day08 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var (cols, rows) = (input[0].Length, input.Length);

        var antennaGroups =
            from col in Enumerable.Range(0, cols)
            from row in Enumerable.Range(0, rows)
            let point = new Point(col, row, cols, rows)
            let character = input[row][col]
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
                    var vector = antenna1 - antenna2;
                    var antiNodes1 = antenna1.GetPointsInDirection(vector);
                    var antiNodes2 = antenna2.GetPointsInDirection(-vector);
                    accumulator.FirstNodes.UnionWith([.. antiNodes1.Take(1), .. antiNodes2.Take(1)]);
                    accumulator.AllNodes.UnionWith([..antiNodes1, ..antiNodes2]);
                }

                return accumulator;
            });

        return (result.FirstNodes.Count.ToString(), result.AllNodes.Count.ToString());
    }
}