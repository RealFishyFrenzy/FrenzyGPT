public class CalculatorPlugin : ICommandPlugin
{
    public string Name => "Calculator";
    public bool CanHandle(string input)
    {
        return input.StartsWith("calc");
    }

    public void Execute(string input)
    {
        Console.WriteLine("Calculator plugin Executed!");
    }
}