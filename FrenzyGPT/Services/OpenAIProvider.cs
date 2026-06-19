using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class OpenAIProvider : IAIProvider
{
    public string Name => "OpenAI";

    private readonly HttpClient _client;
    private readonly string _model;

    public TokenUsage Usage { get; private set; } = new TokenUsage();

    public OpenAIProvider(HttpClient client, string model)
    {
        _client = client;
        _model = model;
    }

    public async Task<string> SendMessage(List<ChatMessage> conversation)
    {

        string? apiKey = Environment.GetEnvironmentVariable(
            "OPENAI_API_KEY",
            EnvironmentVariableTarget.User
        );

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            ConsoleUI.Error("OpenAI API key not found.");
            return "";
        }
        var requestBody = new
        {
            model = _model,
            input = conversation
        };

        string json = JsonSerializer.Serialize(requestBody);

        using StringContent content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json"
        );

        using HttpRequestMessage request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://api.openai.com/v1/responses"
        );

        request.Headers.Add("Authorization", $"Bearer {apiKey}");
        request.Content = content;

        HttpResponseMessage response = await _client.SendAsync(request);

        string responseText = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("\nAPI error:");
            Console.WriteLine(responseText);
            return "";
        }

        using JsonDocument usageDoc = JsonDocument.Parse(responseText);

        JsonElement usage = usageDoc.RootElement.GetProperty("usage");

        Usage.InputTokens = usage.GetProperty("input_tokens").GetInt32();
        Usage.OutputTokens = usage.GetProperty("output_tokens").GetInt32();
        Usage.TotalTokens = usage.GetProperty("total_tokens").GetInt32();

        return ResponseParser.ParseResponse(responseText);
    }

    public bool IsAvailable()
    {
        string? apiKey = Environment.GetEnvironmentVariable(
        "OPENAI_API_KEY",
        EnvironmentVariableTarget.User
    );

        return !string.IsNullOrWhiteSpace(apiKey);
    }
}