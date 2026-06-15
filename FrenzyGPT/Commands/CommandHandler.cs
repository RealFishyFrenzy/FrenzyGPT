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
                Console.WriteLine("\nCommands:");
                Console.WriteLine("/help  - Show commands");
                Console.WriteLine("/clear - Clear chat memory");
                Console.WriteLine("/stats - Shows the amount of message in current memory");
                Console.WriteLine("/tokens - Shows total amount tokens used");
                Console.WriteLine("/save 'name' - Save current chat");
                Console.WriteLine("/load 'name' - Load saved chat");
                Console.WriteLine("/chats     - Show saved chats");
                Console.WriteLine("/exit  - Close FrenzyGPT");

                return true;

            case "/clear":
                conversation.Clear();
                Console.WriteLine("\nChat memory cleared.");
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

            case "/exit":
                Console.WriteLine("\nGoodbye.");
                Environment.Exit(0);
                return true;

            default:
                Console.WriteLine("\nUnknown command. Type /help.");
                return true;
        }
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