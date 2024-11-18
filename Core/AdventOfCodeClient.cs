using System.Text.RegularExpressions;

namespace AdventOfCode.Core;

public class AdventOfCodeClient
{
    private const string BaseUrl = "https://adventofcode.com";
    private readonly HttpClient _httpClient;
    private readonly string _sessionToken;

    public AdventOfCodeClient()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };

        try
        {
            _sessionToken = File.ReadAllText(".sec").Trim();
        }
        catch (Exception ex)
        {
            throw new FileNotFoundException("Session token file 'secret' not found or invalid", ex);
        }
    }

    public async Task<bool> ValidateSessionTokenAsync()
    {
        using var request = CreateRequest(HttpMethod.Get, "2015/day/1/input");
        try
        {
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string> GetInputAsync(int year, int day)
    {
        using var request = CreateRequest(HttpMethod.Get, $"{year}/day/{day}/input");
        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to fetch input: {response.StatusCode}");
        }

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> SubmitAnswerAsync(int year, int day, int part, string answer)
    {
        if (string.IsNullOrWhiteSpace(answer))
        {
            return "Cannot submit empty answer";
        }

        var response = await PostAnswerAsync(year, day, part, answer);
        if (!response.IsSuccessStatusCode)
        {
            return $"Failed to submit answer: {response.StatusCode}";
        }

        var responseText = await response.Content.ReadAsStringAsync();
        return ParseSubmissionResponse(responseText);
    }

    private HttpRequestMessage CreateRequest(HttpMethod method, string path)
    {
        var request = new HttpRequestMessage(method, path);
        request.Headers.Add("Cookie", $"session={_sessionToken}");
        return request;
    }

    private async Task<HttpResponseMessage> PostAnswerAsync(int year, int day, int part, string answer)
    {
        using var content = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("level", part.ToString()),
            new KeyValuePair<string, string>("answer", answer.Trim())
        ]);

        var request = CreateRequest(HttpMethod.Post, $"{year}/day/{day}/answer");
        request.Content = content;
        return await _httpClient.SendAsync(request);
    }

    private static string ParseSubmissionResponse(string responseText)
    {
        if (responseText.Contains("That's the right answer"))
        {
            return "Correct! ⭐";
        }

        if (responseText.Contains("You gave an answer too recently"))
        {
            return ParseWaitTime(responseText);
        }

        if (responseText.Contains("That's not the right answer"))
        {
            return ParseIncorrectAnswer(responseText);
        }

        if (responseText.Contains("You don't seem to be solving the right level"))
        {
            return "You've already completed this level";
        }

        return "Unknown response from AoC server";
    }

    private static string ParseWaitTime(string responseText)
    {
        var waitMatch = Regex.Match(responseText, @"You have (?:(\d+)m )?(\d+)s left to wait");
        if (!waitMatch.Success)
        {
            return "Too many attempts. Please wait before trying again.";
        }

        var minutes = waitMatch.Groups[1].Success ? int.Parse(waitMatch.Groups[1].Value) : 0;
        var seconds = int.Parse(waitMatch.Groups[2].Value);
        return $"Too many attempts. Please wait {minutes}m {seconds}s before trying again.";
    }

    private static string ParseIncorrectAnswer(string responseText)
    {
        if (responseText.Contains("too high"))
        {
            return "Incorrect - your answer is too high";
        }

        if (responseText.Contains("too low"))
        {
            return "Incorrect - your answer is too low";
        }

        return "Incorrect answer";
    }
}