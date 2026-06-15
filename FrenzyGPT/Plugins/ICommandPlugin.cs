public interface ICommandPlugin
{
    string Name { get; }

    bool CanHandle(string input);

    void Execute(string input);
}