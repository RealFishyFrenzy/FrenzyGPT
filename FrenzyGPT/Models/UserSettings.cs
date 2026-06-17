
public class UserSettings
{
    public int Version { get; set; } = 1;
    public string Provider { get; set; } = "OpenAI";
    public string Model { get; set; } = "gpt-5.4-mini";
    public string Theme { get; set; } = "Dark";
    public bool ShowStartup { get; set; } = true;
}