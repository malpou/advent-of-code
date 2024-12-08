namespace AdventOfCode.Utils;

/// <summary>
///     Represents a 2D vector in a grid coordinate system with origin (0,0) at top-left.
///     X increases to the right, Y increases downward.
/// </summary>
/// <param name="X">The x-coordinate (horizontal position)</param>
/// <param name="Y">The y-coordinate (vertical position)</param>
public record Vector(int X, int Y)
{
    // Cardinal directions (relative to top-left origin)
    /// <summary>Vector pointing upward (decreasing Y)</summary>
    public static Vector Up => new(0, -1);

    /// <summary>Vector pointing downward (increasing Y)</summary>
    public static Vector Down => new(0, 1);

    /// <summary>Vector pointing left (decreasing X)</summary>
    public static Vector Left => new(-1, 0);

    /// <summary>Vector pointing right (increasing X)</summary>
    public static Vector Right => new(1, 0);

    // Diagonal directions
    /// <summary>Vector pointing up and left (decreasing both X and Y)</summary>
    public static Vector UpLeft => new(-1, -1);

    /// <summary>Vector pointing up and right (increasing X, decreasing Y)</summary>
    public static Vector UpRight => new(1, -1);

    /// <summary>Vector pointing down and left (decreasing X, increasing Y)</summary>
    public static Vector DownLeft => new(-1, 1);

    /// <summary>Vector pointing down and right (increasing both X and Y)</summary>
    public static Vector DownRight => new(1, 1);

    /// <summary>Array of vectors representing the four cardinal directions: Up, Right, Down, Left</summary>
    public static Vector[] CardinalDirections =>
    [
        Up, Right, Down, Left
    ];

    /// <summary>Array of vectors representing all eight directions: cardinal and diagonal</summary>
    public static Vector[] EightDirections =>
    [
        UpLeft, Up, UpRight,
        Left, Right,
        DownLeft, Down, DownRight
    ];

    /// <summary>
    ///     Negates a vector, reversing its direction
    /// </summary>
    /// <param name="v">The vector to negate</param>
    /// <returns>A new vector pointing in the opposite direction</returns>
    public static Vector operator -(Vector v)
    {
        return new Vector(-v.X, -v.Y);
    }

    /// <summary>
    ///     Adds two vectors together
    /// </summary>
    /// <param name="v1">The first vector</param>
    /// <param name="v2">The second vector</param>
    /// <returns>A new vector representing the sum of the two vectors</returns>
    public static Vector operator +(Vector v1, Vector v2)
    {
        return new Vector(v1.X + v2.X, v1.Y + v2.Y);
    }

    /// <summary>
    ///     Subtracts one vector from another
    /// </summary>
    /// <param name="v1">The vector to subtract from</param>
    /// <param name="v2">The vector to subtract</param>
    /// <returns>A new vector representing the difference between the two vectors</returns>
    public static Vector operator -(Vector v1, Vector v2)
    {
        return new Vector(v1.X - v2.X, v1.Y - v2.Y);
    }

    /// <summary>
    ///     Multiplies a vector by a scalar value
    /// </summary>
    /// <param name="v">The vector to scale</param>
    /// <param name="scalar">The scaling factor</param>
    /// <returns>A new vector scaled by the given factor</returns>
    public static Vector operator *(Vector v, int scalar)
    {
        return new Vector(v.X * scalar, v.Y * scalar);
    }

    /// <summary>
    ///     Multiplies a scalar value by a vector
    /// </summary>
    /// <param name="scalar">The scaling factor</param>
    /// <param name="v">The vector to scale</param>
    /// <returns>A new vector scaled by the given factor</returns>
    public static Vector operator *(int scalar, Vector v)
    {
        return v * scalar;
    }

    /// <summary>
    ///     Rotates the vector 90 degrees clockwise
    /// </summary>
    /// <returns>A new vector rotated 90 degrees clockwise</returns>
    public Vector TurnClockwise() => new(-Y, X);

    /// <summary>
    ///     Rotates the vector 90 degrees counter-clockwise
    /// </summary>
    /// <returns>A new vector rotated 90 degrees counter-clockwise</returns>
    public Vector TurnCounterClockwise() => new(Y, -X);

    /// <summary>
    ///     Returns a string representation of the vector in the format <X, Y>
    /// </summary>
    /// <returns>A string in the format "<X, Y>"</returns>
    public override string ToString() => $"<{X},{Y}>";

    /// <summary>
    ///     '
    ///     Generates a hash code for the vector based on its X and Y coordinates
    /// </summary>
    /// <returns>A hash code that uniquely identifies the vector's position</returns>
    public override int GetHashCode() => HashCode.Combine(X, Y);
}