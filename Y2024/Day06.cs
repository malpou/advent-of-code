using AdventOfCode.Core;
using AdventOfCode.Utils;

namespace AdventOfCode.Y2024;

public class Day06 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var (cols, rows) = (input[0].Length, input.Length);

        var walls = (from row in Enumerable.Range(0, rows)
            from col in Enumerable.Range(0, cols)
            where input[row][col] == '#'
            select new Point(col, row, cols, rows)).ToHashSet();

        var start = (from row in Enumerable.Range(0, rows)
            from col in Enumerable.Range(0, cols)
            where input[row][col] == '^'
            select new Point(X: col, Y: row, cols, rows)).Single();

        var guard = new Guard(start, Vector.Up, walls);
        while (guard.Step()) { }

        var successfullyBlockedLocations = guard.VisitedPositions
            .Where(p => !p.Equals(start))
            .Count(pos =>
            {
                var testGuard = new Guard(start, Vector.Up, [..walls, pos]);
                var seen = new HashSet<(Point pos, Vector dir)>();
                while (testGuard.Step())
                {
                    if (!seen.Add((testGuard.Position, testGuard.Direction)))
                    {
                        return true;
                    }
                }

                return false;
            });

        return (guard.VisitedPositions.Count.ToString(), successfullyBlockedLocations.ToString());
    }

    private class Guard(Point startPos, Vector startDir, HashSet<Point> walls)
    {
        public Point Position { get; private set; } = startPos;
        public Vector Direction { get; private set; } = startDir;
        public HashSet<Point> VisitedPositions { get; } = [startPos];

        public bool Step()
        {
            var nextPosition = Position.AddVector(Direction);

            if (!nextPosition.IsInBounds() || walls.Contains(nextPosition))
            {
                Direction = Direction.TurnClockwise();
                return true;
            }

            Position = nextPosition;
            VisitedPositions.Add(Position);
            return Position.AddVector(Direction).IsInBounds();
        }
    }
}