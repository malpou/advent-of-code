namespace AdventOfCode.Y2022;

internal class Day06 : Day
{
  private const int MarkerLength = 4;

  public override (string part1, string part2) Solve(string[] _, string input)
  {
    var result = -1;
    for (var i = 0; i < input.Length; i++)
    {
      if (input.Substring(i, MarkerLength).ToHashSet().Count != MarkerLength)
      {
        continue;
      }

      result = i + MarkerLength;
      break;
    }

    return (result.ToString(), "");
  }
}