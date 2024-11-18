namespace AdventOfCode.Core;

public class AdventOfCodeApp
{
    private readonly AdventOfCodeClient _client;
    private readonly SolutionRunner _solutionRunner;

    public AdventOfCodeApp()
    {
        _client = new AdventOfCodeClient();
        _solutionRunner = new SolutionRunner(_client);
    }

    public async Task RunAsync(string[] args)
    {
        try
        {
            await HandleCommandAsync(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private async Task HandleCommandAsync(string[] args)
    {
        switch (args.Length)
        {
            case 1 when args[0].ToLower() == "ok":
                await ValidateSessionAsync();
                return;
            case < 2 or > 3:
                ShowUsage();
                return;
        }

        if (!TryParseYearDay(args, out var year, out var day))
        {
            Console.WriteLine("Year and day must be numbers");
            return;
        }

        var shouldSubmit = args is [_, _, "-s"];
        await RunSolutionAsync(year, day, shouldSubmit);
    }

    private async Task ValidateSessionAsync()
    {
        var isValid = await _client.ValidateSessionTokenAsync();
        Console.WriteLine(isValid
            ? "Session token is valid!"
            : "Session token is invalid. Please check your secret file.");
    }

    private static void ShowUsage()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  dotnet run <year> <day>      Run specific day");
        Console.WriteLine("  dotnet run <year> <day> -s   Run and submit results");
        Console.WriteLine("  dotnet run ok                Validate session token");
        Console.WriteLine("\nExample: dotnet run 2015 01");
    }

    private static bool TryParseYearDay(string[] args, out int year, out int day)
    {
        year = 0;
        day = 0;
        return int.TryParse(args[0], out year) && int.TryParse(args[1], out day);
    }

    private async Task RunSolutionAsync(int year, int day, bool shouldSubmit)
    {
        var result = await _solutionRunner.RunDayAsync(year, day);

        if (shouldSubmit)
        {
            await SubmitAnswersAsync(year, day, result);
            return;
        }

        if (string.IsNullOrWhiteSpace(result.part1) && string.IsNullOrWhiteSpace(result.part2))
        {
            Console.WriteLine("No solution implemented yet!");
        }
        else
        {
            Console.WriteLine($"Part 1: {result.part1}");
            if (!string.IsNullOrEmpty(result.part2))
            {
                Console.WriteLine($"Part 2: {result.part2}");
            }
        }
    }

    private async Task SubmitAnswersAsync(int year, int day, (string part1, string? part2) result)
    {
        if (!string.IsNullOrWhiteSpace(result.part1))
        {
            var submitResult = await _client.SubmitAnswerAsync(year, day, 1, result.part1);
            Console.WriteLine($"\nSubmitting Part 1: {submitResult}");
        }
        else
        {
            Console.WriteLine("\nPart 1 is empty - skipping submission");
        }

        if (!string.IsNullOrWhiteSpace(result.part2))
        {
            var submitResult = await _client.SubmitAnswerAsync(year, day, 2, result.part2);
            Console.WriteLine($"Submitting Part 2: {submitResult}");
        }
        else if (result.part2 != null)
        {
            Console.WriteLine("Part 2 is empty - skipping submission");
        }
    }
}