namespace AdventOfCode.Core;

public abstract class Day
{
    public abstract (string part1, string part2) Solve(string[] inputLines, string inputText);
}

public class SolutionRunner(AdventOfCodeClient client, string inputsBasePath = "inputs")
{
    public async Task<(string part1, string? part2)> RunDayAsync(int year, int day)
    {
        var dayString = day.ToString("00");
        await EnsureSolutionFileExistsAsync(year, dayString);
        await EnsureInputFileExistsAsync(year, day);

        return await ExecuteSolutionAsync(year, dayString);
    }

    private async Task<(string part1, string? part2)> ExecuteSolutionAsync(int year, string dayString)
    {
        var className = $"AdventOfCode.Y{year}.Day{dayString}";
        var inputPath = Path.Combine(inputsBasePath, $"{year}_{dayString}");

        var inputLines = await File.ReadAllLinesAsync(inputPath);
        var inputText = await File.ReadAllTextAsync(inputPath);

        var type = Type.GetType(className) ??
                   throw new InvalidOperationException(
                       $"Solution class {className} not found. Ensure it exists and inherits from Day.");

        if (Activator.CreateInstance(type) is not Day instance)
        {
            throw new InvalidOperationException(
                $"Could not create instance of {className} or it doesn't inherit from Day.");
        }

        return instance.Solve(inputLines, inputText);
    }

    private async Task EnsureInputFileExistsAsync(int year, int day)
    {
        var inputPath = Path.Combine(inputsBasePath, $"{year}_{day:00}");
        if (File.Exists(inputPath))
        {
            return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(inputPath)!);
        var input = await client.GetInputAsync(year, day);
        await File.WriteAllTextAsync(inputPath, input);
    }

    private static async Task EnsureSolutionFileExistsAsync(int year, string dayString)
    {
        var directory = $"Y{year}";
        var filePath = Path.Combine(directory, $"Day{dayString}.cs");

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(filePath))
        {
            var template = $$"""
                             using AdventOfCode.Core;

                             namespace AdventOfCode.Y{{year}};

                             public class Day{{dayString}}: Day
                             {
                                 public override (string part1, string part2) Solve(string[] inputLines, string inputText)
                                 {
                                     
                                     return ("", "");
                                 }
                             }
                             """;

            await File.WriteAllTextAsync(filePath, template);
        }
    }
}