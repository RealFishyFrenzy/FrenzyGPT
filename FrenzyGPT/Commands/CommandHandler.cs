using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

public static class CommandHandler
{
    public static bool HandleCommand(
    string userInput,
    List<ChatMessage> conversation,
    OpenAIClient client)
    {
        if (!userInput.StartsWith("/"))
            return false;

        

        switch (userInput.ToLower())
        {
            case "/help":
                ShowHelpMain();
                return true;

            case "/help chat":
                ShowHelpChat();
                return true;

            case "/help account":
                ShowHelpAccount();
                return true;

            case "/help system":
                ShowHelpSystem();
                return true;

            case "/help plugins":
                PluginManager.ShowPlugins();
                return true;

            case "/clear":
                conversation.Clear();
                Console.WriteLine("\nChat memory cleared.");
                return true;

            case "/cls":
                Console.Clear();
                ConsoleUI.Header();

                return true;

            case "/stats":
                ShowStats(conversation);
                return true;

            case "/tokens":

                ConsoleUI.SystemMessage("Last API Call");

                Console.WriteLine($"Input : {client.Usage.InputTokens}");
                Console.WriteLine($"Output: {client.Usage.OutputTokens}");
                Console.WriteLine($"Total : {client.Usage.TotalTokens}");

                return true;

            case string command when command.StartsWith("/save"):
                SaveCommand(userInput, conversation);
                return true;

            case string command when command.StartsWith("/load"):
                LoadCommand(userInput, conversation);
                return true;

            case "/chats":
                ChatStorage.ShowChats();
                return true;

            case "/logout":
                Logout(conversation);
                return true;

            case "/exit":
                Console.WriteLine("\nGoodbye.");
                Environment.Exit(0);
                return true;

            case "/whoami":
                ConsoleUI.SystemMessage("Current User:");
                Console.WriteLine(Session.CurrentUser);
                return true;

            case "/settings":

                ShowSettings();
                return true;

            case "/plugins":

                PluginManager.ShowPlugins();

                return true;

            default:
                Console.WriteLine("\nUnknown command. Type /help.");
                return true;
        }
    }

    private static void ShowHelpMain()
    {
        ConsoleUI.SystemMessage("Help");

        Console.WriteLine("/help chat      - Chat commands");
        Console.WriteLine("/help account   - Account commands");
        Console.WriteLine("/help system    - System commands");
        Console.WriteLine("/help plugins   - Plugin commands");
    }

    private static void ShowHelpChat()
    {
        ConsoleUI.SystemMessage("Chat Commands");

        Console.WriteLine("/save <name>    - Save current chat");
        Console.WriteLine("/load <name>    - Load a chat");
        Console.WriteLine("/chats          - Show saved chats");
        Console.WriteLine("/clear          - Clear chat memory");
    }

    private static void ShowHelpAccount()
    {
        ConsoleUI.SystemMessage("Account Commands");

        Console.WriteLine("/whoami         - Current user");
        Console.WriteLine("/logout         - Logout");
    }

    private static void ShowHelpSystem()
    {
        ConsoleUI.SystemMessage("System Commands");

        Console.WriteLine("/settings       - View settings");
        Console.WriteLine("/stats          - Chat statistics");
        Console.WriteLine("/tokens         - Last API token usage");
        Console.WriteLine("/cls            - Clear console");
        Console.WriteLine("/exit           - Exit FrenzyGPT");
    }

    private static void Logout(List<ChatMessage> conversation)
    {
        conversation.Clear();
        Session.CurrentUser = "";

        ConsoleUI.SystemMessage("Logged out.");

        if (!LoginScreen.Show())
        {
            ConsoleUI.Error("Access denied.");
            Environment.Exit(0);
        }

        StartupScreen.Show();
    }

    private static void ShowStats(List<ChatMessage> conversation)
    {
        int userMessages = 0;
        int aiMessages = 0;

        foreach (ChatMessage message in conversation)
        {
            if (message.role == "user")
                userMessages++;

            if (message.role == "assistant")
                aiMessages++;
        }

        Console.WriteLine("Stats:");
        Console.WriteLine($"Messages in memory: {conversation.Count}");
        Console.WriteLine($"User messages: {userMessages}");
        Console.WriteLine($"AI messages: {aiMessages}");
    }

    private static void ShowSettings()
    {
        UserSettings settings = SettingsService.Load();

        ConsoleUI.SystemMessage("Settings");

        Console.WriteLine($"Model           : {settings.Model}");
        Console.WriteLine($"Theme           : {settings.Theme}");
        Console.WriteLine($"ShowStartup     : {settings.ShowStartup}");
    }

    

    private static void SaveCommand(string userInput, List<ChatMessage> conversation)
    {
        string[] parts = userInput.Split(' ', 2);

        if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
        {
            ConsoleUI.SystemMessage("Usage: /save chatName");
            return;
        }

        ChatStorage.Save(parts[1], conversation);
    }

    private static void LoadCommand(string userInput, List<ChatMessage> conversation)
    {
        string[] parts = userInput.Split(' ', 2);

        if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
        {
            ConsoleUI.SystemMessage("Usage: /load chatName");
            return;
        }

        List<ChatMessage>? loadedChat = ChatStorage.Load(parts[1]);

        if (loadedChat == null)
            return;

        conversation.Clear();
        conversation.AddRange(loadedChat);

        ConsoleUI.SystemMessage($"Loaded chat: {parts[1].Trim()}");
        Console.WriteLine($"Messages loaded: {conversation.Count}");
    }

}