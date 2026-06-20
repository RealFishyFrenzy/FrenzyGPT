using System;

public static class HelpManager
{
    public static void Show(string userInput)
    {
        string[] parts = userInput.Split(' ', 2);

        if (parts.Length < 2 )
        {
            ShowMain();
            return;
        }

        switch (parts[1].Trim().ToLower())
        {
            case "chat":
                ShowChat();
                break;

            case "account":
                ShowAccount();
                break;

            case "system":
                ShowSystem();
                break;

            case "plugins":
                PluginManager.ShowPlugins();
                break;

            default:
                ConsoleUI.Error("Unknown help category.");
                ShowMain();
                break;
        }
    }

    private static void ShowMain()
    {
        ConsoleUI.SystemMessage("Help");

        Console.WriteLine("/help chat      - Chat commands");
        Console.WriteLine("/help account   - Account commands");
        Console.WriteLine("/help system    - System commands");
        Console.WriteLine("/help plugins   - Plugin commands");
    }

    private static void ShowChat()
    {
        ConsoleUI.SystemMessage("Chat Commands");

        Console.WriteLine("/save <name>    - Save current chat");
        Console.WriteLine("/load <name>    - Load saved chat");
        Console.WriteLine("/chats          - Show saved chats");
        Console.WriteLine("/clear          - Clear chat memory");
    }

    private static void ShowAccount()
    {
        ConsoleUI.SystemMessage("Account Commands");

        Console.WriteLine("/whoami         - Show current user");
        Console.WriteLine("/logout         - Log out current user");
    }

    private static void ShowSystem()
    {
        ConsoleUI.SystemMessage("System Commands");

        Console.WriteLine("/ai             - Show AI status");
        Console.WriteLine("/provider       - Show/change provider");
        Console.WriteLine("/model          - Show/change model");
        Console.WriteLine("/settings       - View settings");
        Console.WriteLine("/stats          - Show chat stats");
        Console.WriteLine("/tokens         - Show token usage");
        Console.WriteLine("/cls            - Clear screen");
        Console.WriteLine("/exit           - Exit FrenzyGPT");
    }
}
