using System.Collections.Generic;

public static class PluginManager
{
    public static List<ICommandPlugin> Plugins = new()
    {
        new CalculatorPlugin()
    };

    public static bool HandlePlugin(string userInput)
    {
        foreach (ICommandPlugin plugin in Plugins)
        {
            if (userInput.StartsWith(plugin.Command))
            {
                plugin.Execute(userInput);
                return true;
            }
        }

        ConsoleUI.Error("Unknown plugin.");

        return true;
    }

    public static void ShowPlugins()
    {
        ConsoleUI.SystemMessage("Loaded Plugins");

        foreach (ICommandPlugin plugin in Plugins)
        {
            Console.WriteLine($"{plugin.Name} v{plugin.Version}");
            Console.WriteLine($"Author      : {plugin.Author}");
            Console.WriteLine($"Command     : {plugin.Command}");
            Console.WriteLine($"Description : {plugin.Description}");
            Console.WriteLine();
        }

        Console.WriteLine($"Total: {Plugins.Count}");
    }
}

