﻿using AdventOfCode;

if (args.Length != 2)
{
  Console.WriteLine("Usage: dotnet run <year> <day>");
  Console.WriteLine("Example: dotnet run 2015 01");
  return;
}

if (!int.TryParse(args[0], out var year) || !int.TryParse(args[1], out var day))
{
  Console.WriteLine("Year and day must be numbers");
  return;
}

try
{
  var runner = new DayRunner();
  var result = await runner.RunDayAsync(year, day);

  Console.WriteLine($"Part 1: {result.part1}");
  if (!string.IsNullOrEmpty(result.part2))
  {
    Console.WriteLine($"Part 2: {result.part2}");
  }
}
catch (Exception ex)
{
  Console.WriteLine($"Error: {ex.Message}");
}