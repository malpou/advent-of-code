using AdventOfCode.Core;

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
            select new Point(row, col)).ToHashSet();

        var start = (from row in Enumerable.Range(0, rows)
            from col in Enumerable.Range(0, cols)
            where input[row][col] == '^'
            select new Point(row, col)).Single();

        var guard = new Guard(start, new Point(-1, 0), walls, rows, cols);
        while (guard.Step()) { }

        var successfullyBlockedLocations = guard.VisitedPositions
            .Where(p => !p.Equals(start))
            .Count(pos =>
            {
                var testGuard = new Guard(start, new Point(-1, 0), [..walls, pos], rows, cols);
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

    private record Point(int Row, int Col)
    {
        public override int GetHashCode() => HashCode.Combine(Row, Col);

        public Point Add(Point other) => new(Row + other.Row, Col + other.Col);

        public Point TurnRight() => new(Col, -Row);
    }

    private class Guard(Point startPos, Point startDir, HashSet<Point> walls, int rows, int cols)
    {
        public Point Position { get; private set; } = startPos;
        public Point Direction { get; private set; } = startDir;
        public HashSet<Point> VisitedPositions { get; } = [startPos];

        private bool IsInBounds(Point p) =>
            p.Row >= 0 && p.Row < rows && p.Col >= 0 && p.Col < cols;

        public bool Step()
        {
            var nextPosition = Position.Add(Direction);

            if (!IsInBounds(nextPosition) || walls.Contains(nextPosition))
            {
                Direction = Direction.TurnRight();
                return true;
            }

            Position = nextPosition;
            VisitedPositions.Add(Position);
            return IsInBounds(Position.Add(Direction));
        }
    }
}