using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class OpenAIClient
{
    private readonly HttpClient _client;
    private readonly string _model = "gpt-5.4-mini";

    public TokenUsage Usage { get; private set; } = new TokenUsage();

    public OpenAIClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> SendMessage(List<ChatMessage> conversation)
    {
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

        HttpResponseMessage response = await _client.PostAsync(
            "https://api.openai.com/v1/responses",
            content
        );

        string responseText = await response.Content.ReadAsStringAsync();

        using JsonDocument usageDoc = JsonDocument.Parse(responseText);

        JsonElement usage = usageDoc.RootElement.GetProperty("usage");

        Usage.InputTokens = usage.GetProperty("input_tokens").GetInt32();
        Usage.OutputTokens = usage.GetProperty("output_tokens").GetInt32();
        Usage.TotalTokens = usage.GetProperty("total_tokens").GetInt32();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("\nAPI error:");
            Console.WriteLine(responseText);
            return "";
        }

        return ResponseParser.ParseResponse(responseText);
    }
}