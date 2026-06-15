using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public static class ChatStorage
{
    public static void Save(string chatName, List<ChatMessage> conversation)
    {
        chatName = CleanFileName(chatName);

        Directory.CreateDirectory(PathManager.Chats);

        string filePath = Path.Combine(PathManager.Chats, chatName + ".json");

        string json = JsonSerializer.Serialize(
            conversation,
            new JsonSerializerOptions { WriteIndented = true }
        );

        File.WriteAllText(filePath, json);

        ConsoleUI.SystemMessage($"Saved chat: {chatName}");
        Console.WriteLine(filePath);
    }

    public static List<ChatMessage>? Load(string chatName)
    {
        chatName = CleanFileName(chatName);

        string filePath = Path.Combine(PathManager.Chats, chatName + ".json");

        if (!File.Exists(filePath))
        {
            ConsoleUI.SystemMessage($"Chat not found: {chatName}");
            return null;
        }

        string json = File.ReadAllText(filePath);

        return JsonSerializer.Deserialize<List<ChatMessage>>(json);
    }

    public static void ShowChats()
    {
        Directory.CreateDirectory(PathManager.Chats);

        string[] files = Directory.GetFiles(PathManager.Chats, "*.json")
            .OrderByDescending(File.GetLastWriteTime)
            .ToArray();

        if (files.Length == 0)
        {
            ConsoleUI.SystemMessage("No saved chats found.");
            return;
        }

        ConsoleUI.SystemMessage("Saved Chats:");

        int count = 1;

        foreach (string file in files)
        {
            string name = Path.GetFileNameWithoutExtension(file);
            DateTime saved = File.GetLastWriteTime(file);

            Console.WriteLine($"{count}. {name} ({saved:g})");
            count++;
        }

        Console.WriteLine($"\nTotal Chats: {files.Length}");
    }

    private static string CleanFileName(string name)
    {
        name = name.Trim();

        foreach (char badChar in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(badChar, '_');
        }

        return name;
    }

    public static string[] GetChatFiles()
    {
        Directory.CreateDirectory(PathManager.Chats);

        return Directory.GetFiles(PathManager.Chats, "*.json")
            .OrderByDescending(File.GetLastWriteTime)
            .ToArray();
    }
}