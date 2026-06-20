using System.IO;
using System.Text.Json;
using System.Xml.Linq;

public static class SettingsService
{
    private static string SettingsFile => Path.Combine(PathManager.Config, "settings.json");

    public static UserSettings Load()
    {
        Directory.CreateDirectory(PathManager.Config);

        if (!File.Exists(SettingsFile))
        {
            UserSettings defaultSettings = new UserSettings();
            Save(defaultSettings);
            return defaultSettings;
        }

        string json = File.ReadAllText(SettingsFile);

        return JsonSerializer.Deserialize<UserSettings>(json)
            ?? new UserSettings();
    }

    public static void Save(UserSettings settings)
    {
        Directory.CreateDirectory(PathManager.Config);

        string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(SettingsFile, json);
    }

    public static List<AIModel> GetAvailableModels(string provider)
    {
        return provider.ToLower() switch
        {
            "openai" => new List<AIModel>
        {
            new AIModel
            {
                DisplayName = "GPT-5.4 Mini",
                Id = "gpt-5.4-mini",
                Description = "Fast and cheaper OpenAI model."
            },
            new AIModel
            {
                DisplayName = "GPT-5.5",
                Id = "gpt-5.5",
                Description = "Stronger OpenAI model."
            }
        },

            "claude" => new List<AIModel>
        {
            new AIModel
            {
                DisplayName = "Claude Haiku 4.5",
                Id = "claude-haiku-4-5-20251001",
                Description = "Fastest and cheapest Claude model."
            },
            new AIModel
            {
                DisplayName = "Claude Sonnet 4.5",
                Id = "claude-sonnet-4-5",
                Description = "Balanced Claude model."
            },
            new AIModel
            {
                DisplayName = "Claude Opus 4.1",
                Id = "claude-opus-4-1",
                Description = "Most capable Claude model."
            }
        },

            _ => new List<AIModel>()
        };
    }

    public static string GetDefaultModel(string provider)
    {
        List<AIModel> models = GetAvailableModels(provider);

        if (models.Count == 0)
            return "";

        return models[0].Id;
    }
}