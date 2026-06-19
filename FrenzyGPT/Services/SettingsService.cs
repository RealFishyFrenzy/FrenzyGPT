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

    public static List<string> GetAvailableModels(string provider)
    {
        return provider.ToLower() switch
        {
            "openai" => new List<string>
            {
                "gpt-5.4-mini",
                "gpt-5.5"
            },

            "claude" => new List<string>
            {
                "claude-sonnet-4-5",
                "claude-opus-4-1"
            },

            _ => new List<string>()
        };
    }

    public static string GetDefaultModel(string provider)
    {
        return provider.ToLower() switch
        {
            "openai" => "gpt-5.4-mini",
            "claude" => "claude-sonnet-4-5",
            _ => "gpt-5.4-mini"
        };
    }
}