public interface ICommandPlugin
{
    string Name { get; }

    string Command { get; }

    string Description { get; }

    void Execute(string input);
}