namespace AdventOfCode.Utils;

public record Point(int X, int Y)
{
    public override int GetHashCode() => HashCode.Combine(X, Y);

    public bool IsInBounds(int xMax, int yMax) => Y >= 0 && Y < yMax && X >= 0 && X < xMax;

    public Point Add(Point other) => new(X + other.X, Y + other.Y);

    public Point TurnRight() => new(-Y, X);

    public Vector GetVector(Point other) => new(other.X - X, other.Y - Y);

    public Point AddVector(Vector vector) => new(X + vector.X, Y + vector.Y);
}