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
}