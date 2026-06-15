using System;
using System.Collections.Generic;
using System.Text;

public static class StartupScreen
{
    public static void Show()
    {
        string folder = PathManager.Chats;

        Directory.CreateDirectory(folder);

        string[] chats = ChatStorage.GetChatFiles();

        ConsoleUI.Header();

        ConsoleUI.SystemMessage($"Welcome {Session.CurrentUser}!");

        Console.WriteLine($"Saved Chats: {chats.Length}");

        if (chats.Length > 0)
        {
            Console.WriteLine("\nRecent Chats: ");

            foreach (string chat in chats.Take(5))
            {
                string name = Path.GetFileNameWithoutExtension(chat);
                List<ChatMessage>? loaded = ChatStorage.Load(name);

                int messageCount = loaded?.Count ?? 0;

                Console.WriteLine($"- {name} ({messageCount} messages)");
            }
        }

        Console.WriteLine("\nType /help for commands.");
    }
}