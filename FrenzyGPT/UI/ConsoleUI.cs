using System;

public static class ConsoleUI
{
    public static void Header()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("===================");
        Console.WriteLine("FrenzyGPT v0.1");
        Console.WriteLine("===================");
        Console.ResetColor();
    }

    public static void UserPrompt()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("\nYou > ");
        Console.ResetColor();
    }

    public static void AiMessage(string text)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nAI > " + text);
        Console.ResetColor();
    }

    public static void SystemMessage(string text)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n" + text);
        Console.ResetColor();
    }

    public static void Error(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n" + text);
        Console.ResetColor();
    }
}