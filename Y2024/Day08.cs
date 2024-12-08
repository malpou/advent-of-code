using AdventOfCode.Core;
using AdventOfCode.Utils;

namespace AdventOfCode.Y2024;

public class Day08 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var (rows, cols) = (input.Length, input[0].Length);

        var antennaGroups = Enumerable.Range(0, rows)
            .SelectMany(_ => Enumerable.Range(0, cols),
                (r, c) => (Char: input[r][c], Point: new Point(r, c)))
            .Where(x => x.Char != '.')
            .GroupBy(x => x.Char)
            .Where(g => g.Count() > 1)
            .Select(g => g.Select(x => x.Point).ToList());

        var antiNodes = antennaGroups.Aggregate((First: new HashSet<Point>(), All: new HashSet<Point>()),
            (acc, antennas) =>
            {
                acc.All.UnionWith(antennas);

                antennas.SelectMany((pos1, i) => antennas.Skip(i + 1)
                        .Select(pos2 => (pos1, pos2)))
                    .ToList()
                    .ForEach(pair =>
                    {
                        var nodes = GetAntiNodes(pair.pos1, pair.pos2, rows, cols);
                        acc.First.UnionWith(nodes.Firsts);
                        acc.All.UnionWith(nodes.All);
                    });

                return acc;
            });

        return (antiNodes.First.Count.ToString(), antiNodes.All.Count.ToString());
    }

    private static (IEnumerable<Point> Firsts, IEnumerable<Point> All) GetAntiNodes
        (Point antenna1, Point antenna2, int rows, int cols)
    {
        var vector = antenna1.GetVector(antenna2);
        var antiNodes1 = GetPoints(antenna1, (-vector.Row, -vector.Col));
        var antiNodes2 = GetPoints(antenna2, vector);

        return (Firsts: [.. antiNodes1.Take(1), .. antiNodes2.Take(1)], All: [..antiNodes1, ..antiNodes2]);

        List<Point> GetPoints(Point antenna, (int Row, int Col) v)
        {
            var points = new List<Point>();
            var point = antenna.AddVector(v);

            while (point.IsInBounds(rows, cols))
            {
                points.Add(point);
                point = point.AddVector(v);
            }

            return points;
        }
    }
}