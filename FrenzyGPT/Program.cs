using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        if (!LoginScreen.Show())
        {
            ConsoleUI.Error("Access denied.");
            return;
        }

        UserSettings settings = SettingsService.Load();

        if (settings.ShowStartup)
        {
            StartupScreen.Show();
        }

        List<ChatMessage> conversation = new();

        using HttpClient client = new HttpClient();
        

        IAIProvider ai = ProviderFactory.Create(client, settings);

        bool offlineMode = !ai.IsAvailable();

        if(offlineMode)
        {
            ConsoleUI.OfflineBanner();
        }

        while (true)
        {
            ConsoleUI.UserPrompt();

            string userInput = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(userInput))
                continue;

            if (userInput.StartsWith("/"))
            {
                CommandHandler.HandleCommand(userInput, conversation, ai);
                continue;
            }

            if (userInput.StartsWith("."))
            {
                PluginManager.HandlePlugin(userInput);
                continue;
            }

            if (offlineMode)
            {
                ConsoleUI.Error("AI chat is unavailable in Offline mode.");
                continue;
            }

            // AI message

            conversation.Add(new ChatMessage
            {
                role = "user",
                content = userInput
            });

            string aiText = await ai.SendMessage(conversation);

            if (!string.IsNullOrWhiteSpace(aiText))
            {
                ConsoleUI.AiMessage(aiText);

                conversation.Add(new ChatMessage
                {
                    role = "assistant",
                    content = aiText
                });
            }
        }
    }
}
