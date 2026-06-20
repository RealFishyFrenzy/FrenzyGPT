using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Runtime.CompilerServices;

public static class CommandHandler
{
    public static bool HandleCommand(
    string userInput,
    List<ChatMessage> conversation,
    IAIProvider client)
    {
        if (!userInput.StartsWith("/"))
            return false;

        switch (userInput.ToLower())
        {
            case string command when command.StartsWith("/help"):
                HelpManager.Show(userInput);
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

            case "/ai":
                ShowAIStatus(client);
                return true;

            case string command when command.StartsWith("/provider"):
                ProviderCommand(userInput);
                return true;

            case string command when command.StartsWith("/model"):
                ModelCommand(userInput);
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

    private static void ShowAIStatus(IAIProvider client)
    {
        UserSettings settings = SettingsService.Load();

        ConsoleUI.SystemMessage("AI Configuration");

        Console.WriteLine($"Provider : {settings.Provider}");
        Console.WriteLine($"Model    : {settings.Model}");
        Console.WriteLine($"API Key  : {(client.IsAvailable() ? "Loaded" : "Missing")}");
    }



    private static void ProviderCommand(string userInput)
    {
        UserSettings settings = SettingsService.Load();

        string[] parts = userInput.Split(' ', 2);

        if (parts.Length < 2)
        {
            ConsoleUI.SystemMessage("Current Provider");
            Console.WriteLine(settings.Provider);
            Console.WriteLine("\nOptions:");
            Console.WriteLine("OpenAI");
            Console.WriteLine("Claude");
            Console.WriteLine("\nUse: /provider Claude");
            return;
        }

        string provider = parts[1].Trim();

        if (provider.ToLower() != "openai" && provider.ToLower() != "claude")
        {
            ConsoleUI.Error("Unknown provider. Use OpenAI or Claude.");
            return;
        }

        settings.Provider = provider;
        settings.Model = SettingsService.GetDefaultModel(provider);
        SettingsService.Save(settings);

        ConsoleUI.SystemMessage($"Provider changed to: {provider}");
        ConsoleUI.SystemMessage("Restart FrenzyGPT for this to take effect.");
    }


    private static void ModelCommand(string userInput)
    {
        UserSettings settings = SettingsService.Load();
        List<AIModel> models = SettingsService.GetAvailableModels(settings.Provider);

        string[] parts = userInput.Split(' ', 2);

        if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
        {
            ConsoleUI.SystemMessage("AI Models");

            for (int i = 0; i < models.Count; i++)
            {
                string selected = models[i].Id == settings.Model ? " <-- current" : "";

                Console.WriteLine($"{i + 1}. {models[i].DisplayName}{selected}");
                Console.WriteLine($"   {models[i].Description}");
            }

            Console.WriteLine("\nUse: /model <number>");
            Console.WriteLine("Or:  /model <display-name>");
            return;
        }

        string choice = parts[1].Trim();

        if (int.TryParse(choice, out int index))
        {
            if (index < 1 || index > models.Count)
            {
                ConsoleUI.Error("Invalid model number.");
                return;
            }

            settings.Model = models[index - 1].Id;
            SettingsService.Save(settings);

            ConsoleUI.SystemMessage($"Model changed to: {models[index - 1].DisplayName}");
            return;
        }

        AIModel? selectedModel = models.FirstOrDefault(
            m => m.DisplayName.Equals(choice, StringComparison.OrdinalIgnoreCase)
        );

        if (selectedModel == null)
        {
            ConsoleUI.Error("Unknown model.");
            return;
        }

        settings.Model = selectedModel.Id;
        SettingsService.Save(settings);

        ConsoleUI.SystemMessage($"Model changed to: {selectedModel.DisplayName}");
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