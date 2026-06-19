using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class ClaudeProvider : IAIProvider
{
    public string Name => "Claude";

    private readonly HttpClient _client;
    private readonly string _model;

    public TokenUsage Usage { get; private set; } = new TokenUsage();

    public ClaudeProvider(HttpClient client, string model)
    {
        _client = client;
        _model = model;
    }

    public async Task<string> SendMessage(List<ChatMessage> conversation)
    {
        string? apiKey = Environment.GetEnvironmentVariable(
            "ANTHROPIC_API_KEY",
            EnvironmentVariableTarget.User
        );

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            ConsoleUI.Error("Claude API key not found.");
            return "";
        }

        var messages = conversation
            .Where(m => m.role == "user" || m.role == "assistant")
            .Select(m => new
            {
                role = m.role,
                content = m.content
            })
            .ToList();

        var requestBody = new
        {
            model = _model,
            max_tokens = 1024,
            messages = messages
        };

        string json = JsonSerializer.Serialize(requestBody);

        using HttpRequestMessage request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://api.anthropic.com/v1/messages"
        );

        request.Headers.Add("x-api-key", apiKey);
        request.Headers.Add("anthropic-version", "2023-06-01");

        request.Content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json"
        );

        HttpResponseMessage response = await _client.SendAsync(request);

        string responseText = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            ConsoleUI.Error("Claude API error:");
            Console.WriteLine(responseText);
            return "";
        }

        using JsonDocument doc = JsonDocument.Parse(responseText);

        string aiText = doc.RootElement
            .GetProperty("content")[0]
            .GetProperty("text")
            .GetString() ?? "";

        JsonElement usage = doc.RootElement.GetProperty("usage");

        Usage.InputTokens = usage.GetProperty("input_tokens").GetInt32();
        Usage.OutputTokens = usage.GetProperty("output_tokens").GetInt32();
        Usage.TotalTokens = Usage.InputTokens + Usage.OutputTokens;

        return aiText;
    }

    public bool IsAvailable()
    {
        string? apiKey = Environment.GetEnvironmentVariable(
            "ANTHROPIC_API_KEY",
            EnvironmentVariableTarget.User
        );

        return !string.IsNullOrWhiteSpace(apiKey);
    }
}