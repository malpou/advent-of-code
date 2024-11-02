namespace AdventOfCode.Y2022;

public class Day04 : Day
{
  public override (int part1, int? part2) Solve(string[] input, string _)
  {
    var overlaps = 0;

    foreach (var s in input)
    {
      var (first, second) = ParseLine(s);
      if (first.FullyContains(second) || second.FullyContains(first))
      {
        overlaps++;
      }
    }

    return (overlaps, null);
  }

  private static (Assignment first, Assignment second) ParseLine(string line)
  {
    var assignments = line.Split(',')
      .Select(ParseAssignment)
      .ToArray();

    return (assignments[0], assignments[1]);
  }

  private static Assignment ParseAssignment(string range)
  {
    var numbers = range.Split('-')
      .Select(int.Parse)
      .ToArray();

    return new Assignment(numbers[0], numbers[1]);
  }

  private class Assignment(int min, int max)
  {
    private int Min { get; } = min;
    private int Max { get; } = max;

    public bool FullyContains(Assignment other)
    {
      return Min <= other.Min && Max >= other.Max;
    }
  }
}