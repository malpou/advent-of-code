namespace AdventOfCode.Utils;

public record Point(int Row, int Col)
{
    public override int GetHashCode() => HashCode.Combine(Row, Col);

    public bool IsInBounds(int rows, int cols) => Row >= 0 && Row < rows && Col >= 0 && Col < cols;

    public Point Add(Point other) => new(Row + other.Row, Col + other.Col);

    public Point TurnRight() => new(Col, -Row);

    public (int Row, int Col) GetVector(Point other) => (other.Row - Row, other.Col - Col);

    public Point AddVector((int Row, int Col) vector) => new(Row + vector.Row, Col + vector.Col);
}