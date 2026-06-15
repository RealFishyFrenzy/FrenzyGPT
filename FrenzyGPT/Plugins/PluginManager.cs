using System.Collections.Generic;

public static class PluginManager
{
    public static List<ICommandPlugin> Plugins = new()
    {
        new CalculatorPlugin()
    };
}