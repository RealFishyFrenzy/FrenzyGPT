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

        StartupScreen.Show();

        List<ChatMessage> conversation = new();

        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        OpenAIClient openAI = new OpenAIClient(client);

        while (true)
        {
            ConsoleUI.UserPrompt();
            string? userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("You typed nothing.");
                continue;
            }

            if (CommandHandler.HandleCommand(userInput, conversation, openAI))
                continue;

            conversation.Add(new ChatMessage
            {
                role = "user",
                content = userInput
            });

            string aiText = await openAI.SendMessage(conversation);

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