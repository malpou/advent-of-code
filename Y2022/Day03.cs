namespace AdventOfCode.Y2022;

public class Day03 : Day
{
  public override (int part1, int? part2) Solve(string[] input, string _)
  {
    var part1 = input.Select(s => s[..(s.Length / 2)]
        .Intersect(s[(s.Length / 2)..])
        .Sum(GetPriority))
      .Sum();

    var part2 = input
      .Chunk(3)
      .Select(group => group[0]
        .Intersect(group[1])
        .Intersect(group[2])
        .Sum(GetPriority))
      .Sum();

    return (part1, part2);
  }

  private static int GetPriority(char c)
  {
    return c - (char.IsLower(c) ? 96 : 38);
  }
}