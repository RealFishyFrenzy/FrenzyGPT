using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        string? apiKey = Environment.GetEnvironmentVariable(
            "OPENAI_API_KEY",
            EnvironmentVariableTarget.User
        );

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            Console.WriteLine("API key not found");
            return;
        }

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
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        IAIProvider ai = ProviderFactory.Create(client, settings);

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