using System;
using System.Data;

public class CalculatorPlugin : ICommandPlugin
{
    public string Name => "Calculator";
    public string Version => "1.0";
    public string Author => "FishyFrenzy";
    public string Command => ".calc";
    public string Description => "Basic calculator plugin.";

    public void Execute(string input)
    {
        string expression = input.Substring(Command.Length).Trim();

        if (string.IsNullOrWhiteSpace(expression))
        {
            ConsoleUI.Error("Usage: .calc 5+5");
            return;
        }

        try
        {
            object result = new DataTable().Compute(expression, null);

            ConsoleUI.SystemMessage("Calculator");
            Console.WriteLine($"{expression} = {result}");
        }
        catch
        {
            ConsoleUI.Error("Invalid expression.");
        }
    }
}