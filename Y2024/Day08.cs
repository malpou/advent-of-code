using AdventOfCode.Core;
using AdventOfCode.Utils;

namespace AdventOfCode.Y2024;

public class Day08 : Day
{
    private int _xMax;
    private int _yMax;

    public override (string part1, string part2) Solve(string[] input, string _)
    {
        (_xMax, _yMax) = (input[0].Length, input.Length);

        var antennaGroups =
            from x in Enumerable.Range(0, _xMax)
            from y in Enumerable.Range(0, _yMax)
            let point = new Point(x, y)
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

                var antennaPairs = GetUniquePairs(antennaGroup);
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

    private (IEnumerable<Point> Firsts, IEnumerable<Point> All) GetAntiNodes
        (Point antenna1, Point antenna2)
    {
        var vector = antenna1.GetVector(antenna2);
        var antiNodes1 = GetPointsInDirection(antenna1, vector.OppositeDirection());
        var antiNodes2 = GetPointsInDirection(antenna2, vector);
        return (Firsts: [.. antiNodes1.Take(1), .. antiNodes2.Take(1)], All: [..antiNodes1, ..antiNodes2]);
    }

    private List<Point> GetPointsInDirection(Point p, Vector v)
    {
        var points = new List<Point>();
        var point = p.AddVector(v);

        while (point.IsInBounds(_xMax, _yMax))
        {
            points.Add(point);
            point = point.AddVector(v);
        }

        return points;
    }

    private static IEnumerable<(Point, Point)> GetUniquePairs(IEnumerable<Point> points)
    {
        var pointsList = points.ToList();

        return from i in Enumerable.Range(0, pointsList.Count)
            from j in Enumerable.Range(i + 1, pointsList.Count - i - 1)
            select (pointsList[i], pointsList[j]);
    }
}