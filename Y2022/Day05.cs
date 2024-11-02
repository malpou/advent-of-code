namespace AdventOfCode.Y2022;

public class Day05 : Day
{
  public override (string part1, string part2) Solve(string[] input, string _)
  {
    var configurationLines = input.TakeWhile(line => !string.IsNullOrWhiteSpace(line)).ToList();
    var moveLines = input.Skip(configurationLines.Count + 1)
      .Select(ParseMoveCommand)
      .ToList();

    var stacks = ParseStacks(configurationLines);

    foreach (var (count, source, dest) in moveLines)
    {
      for (var i = 0; i < count; i++)
      {
        var crate = stacks[source][^1];
        stacks[source].RemoveAt(stacks[source].Count - 1);
        stacks[dest].Add(crate);
      }
    }

    var result = string.Concat(stacks.Select(s => s.Value.Last()));

    return (result, "");
  }

  private static Dictionary<int, List<char>> ParseStacks(List<string> configurationLines)
  {
    var stacks = configurationLines.Last()
      .Split(' ', StringSplitOptions.RemoveEmptyEntries)
      .Select(int.Parse)
      .ToDictionary(key => key, _ => new List<char>());

    for (var row = configurationLines.Count - 2; row >= 0; row--)
    {
      for (int col = 1, pos = 1; pos < configurationLines[row].Length; col++, pos += 4)
      {
        var crate = configurationLines[row][pos];
        if (crate != ' ')
        {
          stacks[col].Add(crate);
        }
      }
    }

    return stacks;
  }

  private static (int count, int source, int dest) ParseMoveCommand(string command)
  {
    var numbers = command.Split(' ')
      .Where(s => int.TryParse(s, out _))
      .Select(int.Parse)
      .ToList();

    return (numbers[0], numbers[1], numbers[2]);
  }
}