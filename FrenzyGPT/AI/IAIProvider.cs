using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAIProvider
{
    string Name { get; }

    TokenUsage Usage { get; }

    Task<string> SendMessage(List<ChatMessage> conversation);
}