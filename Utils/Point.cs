namespace AdventOfCode.Utils;

public record Point(int X, int Y, int XMax, int YMax)
{
    public bool IsInBounds() => Y >= 0 && Y < YMax && X >= 0 && X < XMax;

    public Vector GetVector(Point other) => new(other.X - X, other.Y - Y);
  
    public Point AddVector(Vector vector) => this with { X = X + vector.X, Y = Y + vector.Y };

    public List<Point> GetPointsInDirection(Vector v)
    {
        var points = new List<Point>();
        var point = AddVector(v);

        while (point.IsInBounds())
        {
            points.Add(point);
            point = point.AddVector(v);
        }

        return points;
    }

    public override int GetHashCode() => HashCode.Combine(X, Y);
}