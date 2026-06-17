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
            Console.WriteLine($"{plugin.Command} - {plugin.Name}");
            Console.WriteLine($"    {plugin.Description}");
        }

        Console.WriteLine($"\nTotal: {Plugins.Count}");
    }
}

