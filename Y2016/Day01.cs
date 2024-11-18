using AdventOfCode.Core;

namespace AdventOfCode.Y2016;

public class Day01 : Day
{
    public override (string part1, string part2) Solve(string[] _, string input)
    {
        var instructions = input.Split(", ");
        var visitedLocations = new HashSet<Position> { new(0, 0) };
        var firstDuplicate = new Position(0, 0);
        var foundDuplicate = false;

        var finalPosition = new Position(0, 0);
        foreach (var position in GetPath(instructions))
        {
            if (!foundDuplicate && !visitedLocations.Add(position))
            {
                firstDuplicate = position;
                foundDuplicate = true;
            }

            finalPosition = position;
        }

        return (
            finalPosition.ManhattanDistance.ToString(),
            foundDuplicate ? firstDuplicate.ManhattanDistance.ToString() : "No duplicate found"
        );
    }

    private static IEnumerable<Position> GetPath(string[] instructions)
    {
        var position = new Position(0, 0);
        var direction = Direction.North;

        foreach (var instruction in instructions)
        {
            direction = Turn(direction, instruction[0]);
            var blocks = int.Parse(instruction[1..]);

            for (var i = 0; i < blocks; i++)
            {
                position = position.Move(direction);
                yield return position;
            }
        }
    }

    private static Direction Turn(Direction current, char turn)
    {
        return (turn, current) switch
        {
            ('R', Direction.North) => Direction.East,
            ('R', Direction.East) => Direction.South,
            ('R', Direction.South) => Direction.West,
            ('R', Direction.West) => Direction.North,
            ('L', Direction.North) => Direction.West,
            ('L', Direction.West) => Direction.South,
            ('L', Direction.South) => Direction.East,
            ('L', Direction.East) => Direction.North,
            _ => throw new InvalidOperationException($"Invalid turn {turn} from direction {current}")
        };
    }

    private enum Direction { North, East, South, West }

    private readonly record struct Position(int X, int Y)
    {
        public int ManhattanDistance => Math.Abs(X) + Math.Abs(Y);

        public Position Move(Direction direction)
        {
            return direction switch
            {
                Direction.North => this with { Y = Y + 1 },
                Direction.South => this with { Y = Y - 1 },
                Direction.East => this with { X = X + 1 },
                Direction.West => this with { X = X - 1 },
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }
    }
}