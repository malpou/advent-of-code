using AdventOfCode;

if (args.Length != 2)
{
  Console.WriteLine("Usage: dotnet run <year> <day>");
  Console.WriteLine("Example: dotnet run 2015 01");
  return;
}

if (!int.TryParse(args[0], out int year) || !int.TryParse(args[1], out int day))
{
  Console.WriteLine("Year and day must be numbers");
  return;
}

try
{
  var runner = new DayRunner();
  var result = await runner.RunDayAsync(year, day);
                
  Console.WriteLine($"Part 1: {result.part1}");
  if (result.part2.HasValue)
  {
    Console.WriteLine($"Part 2: {result.part2}");
  }
}
catch (Exception ex)
{
  Console.WriteLine($"Error: {ex.Message}");
}