using System;

public static class ConsoleUI
{
    public static void Header()
    {
        Console.ForegroundColor = ThemeManager.Current.HeaderColor;
        Console.WriteLine("===================");
        Console.WriteLine("FrenzyGPT v0.3");
        Console.WriteLine("===================");
        Console.ResetColor();
    }

    public static void UserPrompt()
    {
        Console.ForegroundColor = ThemeManager.Current.UserColor;
        Console.Write("\nYou > ");
        Console.ResetColor();
    }

    public static void AiMessage(string text)
    {
        Console.ForegroundColor = ThemeManager.Current.AiColor;
        Console.WriteLine("\nAI > " + text);
        Console.ResetColor();
    }

    public static void SystemMessage(string text)
    {
        Console.ForegroundColor = ThemeManager.Current.SystemColor;
        Console.WriteLine("\n" + text);
        Console.ResetColor();
    }

    public static void Error(string text)
    {
        Console.ForegroundColor = ThemeManager.Current.ErrorColor;
        Console.WriteLine("\n" + text);
        Console.ResetColor();
    }

    public static void OfflineBanner()
    {
        SystemMessage("Offline Mode");

        Console.WriteLine("AI Provider unavailable.");
        Console.WriteLine("Cloud AI chat is disabled.\n");

        Console.WriteLine("Available:");
        Console.WriteLine("  /help");
        Console.WriteLine("  /settings");
        Console.WriteLine("  /plugins");
        Console.WriteLine("  /save");
        Console.WriteLine("  /load");
        Console.WriteLine("  .plugins");
        Console.WriteLine();
    }
}