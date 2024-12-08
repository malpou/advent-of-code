using AdventOfCode.Core;

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
        var positiveVector = (
            Row: antenna2.Row - antenna1.Row,
            Col: antenna2.Col - antenna1.Col
        );

        return GetPoints(antenna1, (
            -positiveVector.Row,
            -positiveVector.Col
        )).Concat(GetPoints(antenna2, positiveVector));

        IEnumerable<Point> GetPoints(Point antenna, (int Row, int Col) v)
        {
            var point = new Point(antenna.Row + v.Row, antenna.Col + v.Col);
            while (IsInBounds(point, rows, cols))
            {
                yield return point;
                if (!allPoints)
                {
                    yield break;
                }

                point = new Point(point.Row + v.Row, point.Col + v.Col);
            }
        }
    }

    private static bool IsInBounds(Point p, int rows, int cols) =>
        p.Row >= 0 && p.Row < rows && p.Col >= 0 && p.Col < cols;

    private record Point(int Row, int Col)
    {
        public override int GetHashCode() => HashCode.Combine(Row, Col);
    }
}