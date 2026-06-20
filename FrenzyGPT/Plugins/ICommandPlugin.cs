public interface ICommandPlugin
{
    string Name { get; }
    string Version { get; }
    string Author { get; }
    string Command { get; }
    string Description { get; }

    void Execute(string input);
}