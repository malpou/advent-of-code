namespace AdventOfCode.Utils;

public record Vector(int X, int Y)
{
    public Vector OppositeDirection() => new(-X, -Y);
}