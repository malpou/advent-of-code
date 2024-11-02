namespace AdventOfCode.Y2022;

public class Day05 : Day
{
  public override (string part1, string part2) Solve(string[] input, string _)
  {
    var configurationLines = input.TakeWhile(line => !string.IsNullOrWhiteSpace(line)).ToList();
    var moveLines = input.Skip(configurationLines.Count + 1)
      .Select(ParseMoveCommand)
      .ToList();

    var stacks1 = ParseStacks(configurationLines);
    var stacks2 = ParseStacks(configurationLines);

    foreach (var (count, source, dest) in moveLines)
    {
      for (var i = 0; i < count; i++)
      {
        var crate = stacks1[source][^1];
        stacks1[source].RemoveAt(stacks1[source].Count - 1);
        stacks1[dest].Add(crate);
      }
    }

    foreach (var (count, source, dest) in moveLines)
    {
      var cratesToMove = stacks2[source].TakeLast(count).ToList();
      stacks2[source].RemoveRange(stacks2[source].Count - count, count);
      stacks2[dest].AddRange(cratesToMove);
    }

    var result1 = string.Concat(stacks1.Select(s => s.Value.Last()));
    var result2 = string.Concat(stacks2.Select(s => s.Value.Last()));

    return (result1, result2);
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