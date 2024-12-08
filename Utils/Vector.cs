namespace AdventOfCode.Utils;

public record Vector(int X, int Y)
{
    // Cardinal directions - Grid coordinates (top-left is 0,0)
    public static Vector Up => new(0, -1);    // Move up (decrease Y)
    public static Vector Down => new(0, 1);    // Move down (increase Y)
    public static Vector Left => new(-1, 0);   // Move left (decrease X)
    public static Vector Right => new(1, 0);   // Move right (increase X)

    // Diagonal directions
    public static Vector UpLeft => new(-1, -1);    // Decrease both X and Y
    public static Vector UpRight => new(1, -1);    // Increase X, decrease Y
    public static Vector DownLeft => new(-1, 1);   // Decrease X, increase Y
    public static Vector DownRight => new(1, 1);   // Increase both X and Y

    public static Vector[] CardinalDirections =>
    [
        Up, Right, Down, Left
    ];

    public static Vector[] EightDirections =>
    [
        UpLeft, Up, UpRight,
        Left, Right,
        DownLeft, Down, DownRight
    ];

    public Vector OppositeDirection() => new(-X, -Y);

    public Vector TurnClockwise() => new(-Y, X);
    
    public Vector TurnCounterClockwise() => new(Y, -X);

    public override int GetHashCode() => HashCode.Combine(X, Y);
}