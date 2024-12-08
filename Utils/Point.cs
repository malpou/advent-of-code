namespace AdventOfCode.Utils;

/// <summary>
/// Represents a point in a bounded 2D grid coordinate system with origin (0,0) at top-left.
/// X increases to the right, Y increases downward.
/// </summary>
/// <param name="X">The x-coordinate (horizontal position)</param>
/// <param name="Y">The y-coordinate (vertical position)</param> 
/// <param name="XMax">Maximum allowed x-coordinate (exclusive)</param>
/// <param name="YMax">Maximum allowed y-coordinate (exclusive)</param>
public record Point(int X, int Y, int XMax, int YMax)
{
   /// <summary>
   /// Checks if the point lies within the grid boundaries
   /// </summary>
   /// <returns>true if the point is within bounds, false otherwise</returns>
   public bool IsInBounds() => Y >= 0 && Y < YMax && X >= 0 && X < XMax;

   /// <summary>
   /// Calculates the vector from this point to another point
   /// </summary>
   /// <param name="other">The destination point</param>
   /// <returns>A vector representing the displacement from this point to the other point</returns>
   public Vector GetVector(Point other) => other - this;

   /// <summary>
   /// Adds a vector to a point, creating a new point at the resulting position
   /// </summary>
   /// <param name="point">The starting point</param>
   /// <param name="vector">The vector to add</param>
   /// <returns>A new point offset by the vector's components</returns>
   public static Point operator +(Point point, Vector vector) => 
       point with { X = point.X + vector.X, Y = point.Y + vector.Y };

   /// <summary>
   /// Subtracts a vector from a point, creating a new point at the resulting position
   /// </summary>
   /// <param name="point">The starting point</param>
   /// <param name="vector">The vector to subtract</param>
   /// <returns>A new point offset by the negated vector's components</returns>
   public static Point operator -(Point point, Vector vector) => 
       point with { X = point.X - vector.X, Y = point.Y - vector.Y };

   /// <summary>
   /// Calculates the vector between two points
   /// </summary>
   /// <param name="p1">The destination point</param>
   /// <param name="p2">The origin point</param>
   /// <returns>A vector representing the displacement from p2 to p1</returns>
   public static Vector operator -(Point p1, Point p2) => 
       new(p1.X - p2.X, p1.Y - p2.Y);

   /// <summary>
   /// Gets all points in a given direction from this point until reaching a grid boundary
   /// </summary>
   /// <param name="v">The direction vector</param>
   /// <returns>A list of points starting from the first point in the direction until the grid boundary</returns>
   public List<Point> GetPointsInDirection(Vector v)
   {
       var points = new List<Point>();
       var point = this + v;

       while (point.IsInBounds())
       {
           points.Add(point);
           point += v;
       }

       return points;
   }

   /// <summary>
   /// Returns a string representation of the point in the format (X,Y)
   /// </summary>
   /// <returns>A string in the format "(X,Y)"</returns>
   public override string ToString() => $"({X},{Y})";
   
   /// <summary>
   /// Generates a hash code for the point based on its X and Y coordinates
   /// </summary>
   /// <returns>A hash code that uniquely identifies the point's position</returns>
   public override int GetHashCode() => HashCode.Combine(X, Y);
}