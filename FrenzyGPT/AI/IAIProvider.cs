using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAIProvider
{
    string Name { get; }

    TokenUsage Usage { get; }

    bool IsAvailable();

    Task<string> SendMessage(List<ChatMessage> conversation);
}