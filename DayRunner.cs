﻿namespace AdventOfCode;

public interface IDay
{
  (string part1, string part2) Solve(string[] inputLines, string inputText);
}

public abstract class Day : IDay
{
  public abstract (string part1, string part2) Solve(string[] inputLines, string inputText);
}

public class DayRunner(string inputsBasePath = "inputs")
{
  public async Task<(string part1, string? part2)> RunDayAsync(int year, int day)
  {
    var dayString = day.ToString("00");
    var className = $"AdventOfCode.Y{year}.Day{dayString}";
    var inputPath = Path.Combine(inputsBasePath, $"{year}_{dayString}");

    if (!File.Exists(inputPath))
    {
      throw new FileNotFoundException($"Input file not found: {inputPath}");
    }

    var inputLines = await File.ReadAllLinesAsync(inputPath);
    var inputText = await File.ReadAllTextAsync(inputPath);

    var type = Type.GetType(className);
    if (type == null)
    {
      throw new InvalidOperationException(
        $"Solution class {className} not found. Ensure it exists and inherits from Day.");
    }

    if (Activator.CreateInstance(type) is not IDay instance)
    {
      throw new InvalidOperationException(
        $"Could not create instance of {className} or it doesn't implement IDay.");
    }

    return instance.Solve(inputLines, inputText);
  }
}