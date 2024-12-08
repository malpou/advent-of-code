using AdventOfCode.Core;
using AdventOfCode.Utils;

namespace AdventOfCode.Y2024;

public class Day06 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var rows = input.Length;
        var cols = input[0].Length;

        var walls = (from row in Enumerable.Range(0, rows)
            from col in Enumerable.Range(0, cols)
            where input[row][col] == '#'
            select new Point(col, row)).ToHashSet();

        var start = (from row in Enumerable.Range(0, rows)
            from col in Enumerable.Range(0, cols)
            where input[row][col] == '^'
            select new Point(col, row)).Single();

        var guard = new Guard(start, new Point(0, -1), walls, rows, cols);
        while (guard.Step()) { }

        var successfullyBlockedLocations = guard.VisitedPositions
            .Where(p => !p.Equals(start))
            .Count(pos =>
            {
                var testGuard = new Guard(start, new Point(0, -1), [..walls, pos], rows, cols);
                var seen = new HashSet<(Point pos, Point dir)>();
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


    private class Guard(Point startPos, Point startDir, HashSet<Point> walls, int rows, int cols)
    {
        public Point Position { get; private set; } = startPos;
        public Point Direction { get; private set; } = startDir;
        public HashSet<Point> VisitedPositions { get; } = [startPos];

        public bool Step()
        {
            var nextPosition = Position.Add(Direction);

            if (!nextPosition.IsInBounds(cols, rows) || walls.Contains(nextPosition))
            {
                Direction = Direction.TurnRight();
                return true;
            }

            Position = nextPosition;
            VisitedPositions.Add(Position);
            return Position.Add(Direction).IsInBounds(cols, rows);
        }
    }
}