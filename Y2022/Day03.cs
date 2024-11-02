namespace AdventOfCode.Y2022;

public class Day03 : Day
{
  public override (int part1, int? part2) Solve(string[] input, string _)
  {
    var result = (from s in input
      let itemsPrCompartment = s.Length / 2
      select s[..itemsPrCompartment]
        .Intersect(s[itemsPrCompartment..])
        .Select(c => c - (char.IsLower(c) ? 96 : 38))
        .Sum())
      .Sum();

    return (result, null);
  }
}