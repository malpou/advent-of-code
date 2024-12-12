using AdventOfCode.Core;
using AdventOfCode.Utils;

namespace AdventOfCode.Y2024;

public class Day12: Day
{
    private readonly HashSet<Point> _knownPoints = [];
    private char[][] _grid = [];
    
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        _grid = input.ToCharGrid();
        var height = input.Length;
        var width = input[0].Length;

        List<List<Point>> regions = [];
        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            var point = new Point(x, y, width, height);
            if (!_knownPoints.Contains(point))
            {
                regions.Add(GetRegion(point, input[y][x]));
            }
        }

        var sum = regions.Select(CalculateRegionPrice).Sum();

        return (sum.ToString(), "");
    }

    private List<Point> GetRegion(Point start, char plantType)
    {
        var region = new List<Point>();
        var queue = new Queue<Point>();
    
        queue.Enqueue(start);
        _knownPoints.Add(start);
        region.Add(start);
    
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
        
            foreach (var direction in Vector.CardinalDirections)
            {
                var neighbor = current + direction;
            
                if (!neighbor.IsInBounds() || _knownPoints.Contains(neighbor))
                    continue;
                
                var neighborType = _grid[neighbor.Y][neighbor.X];

                if (neighborType != plantType)
                {
                    continue;
                }

                queue.Enqueue(neighbor);
                _knownPoints.Add(neighbor);
                region.Add(neighbor);
            }
        }
    
        return region;
    }

    private static int CalculateRegionPrice(List<Point> region)
    {
        var area = region.Count;
    
        var perimeter = (
            from point in region 
            from direction in Vector.CardinalDirections 
            select point + direction)
            .Count(neighbor => !neighbor.IsInBounds() || !region.Contains(neighbor));

        return perimeter * area;
    }
}