using System.Net.Http;

public static class ProviderFactory
{
    public static IAIProvider Create(HttpClient client, UserSettings settings)
    {
        return settings.Provider.ToLower() switch
        {
            "openai" => new OpenAIProvider(client),
            "claude" => new ClaudeProvider(client),
            _ => new OpenAIProvider(client)
        };
    }
}