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

        var (antiNodes, resonantAntiNodes) = antennaGroups
            .Aggregate((new HashSet<Point>(), new HashSet<Point>()), (acc, antennas) =>
            {
                acc.Item2.UnionWith(antennas);

                antennas.SelectMany((pos1, i) => antennas.Skip(i + 1)
                        .Select(pos2 => (pos1, pos2)))
                    .ToList()
                    .ForEach(pair =>
                    {
                        acc.Item1.UnionWith(GetVectorPoints(pair.pos1, pair.pos2, rows, cols, false));
                        acc.Item2.UnionWith(GetVectorPoints(pair.pos1, pair.pos2, rows, cols, true));
                    });

                return acc;
            });

        return (antiNodes.Count.ToString(), resonantAntiNodes.Count.ToString());
    }

    private static IEnumerable<Point> GetVectorPoints(Point antenna1, Point antenna2, int rows, int cols,
        bool allPoints)
    {
        var vector = antenna1.GetVector(antenna2);
        return GetPoints(antenna1, (-vector.Row, -vector.Col)).Concat(GetPoints(antenna2, vector));

        IEnumerable<Point> GetPoints(Point antenna, (int Row, int Col) v)
        {
            var point = antenna.AddVector(v);
            while (point.IsInBounds(rows, cols))
            {
                yield return point;
                if (!allPoints) yield break;
                point = point.AddVector(v);
            }
        }
    }
}