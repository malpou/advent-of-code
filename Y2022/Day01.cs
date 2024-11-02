namespace AdventOfCode.Y2022;

public class Day01 : Day
{
  public override (int part1, int? part2) Solve(string[] input)
  {
    var weightTotals = new List<int>();
    var currentWeight = 0;
    foreach (var s in input)
    {
      if (int.TryParse(s, out var number))
      {
        currentWeight += number;
      }
      else if (currentWeight > 0)
      {
        weightTotals.Add(currentWeight);
        currentWeight = 0;
      }
    }

    weightTotals.Sort((a, b) => b.CompareTo(a));
    return (weightTotals[0], null);
  }
}