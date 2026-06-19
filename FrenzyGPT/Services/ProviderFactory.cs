using System.Net.Http;

public static class ProviderFactory
{
    public static IAIProvider Create(HttpClient client, UserSettings settings)
    {
        return settings.Provider.ToLower() switch
        {
            "openai" => new OpenAIProvider(client, settings.Model),
            "claude" => new ClaudeProvider(client, settings.Model),
            _ => new OpenAIProvider(client, settings.Model)
        };
    }
}