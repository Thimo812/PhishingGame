using System.Collections.Immutable;

namespace PhishingGame.Core;

public interface ILinkedState
{
    Session Session { get; set; }
    ILinkedState NextState { get; set; }
    Type PlayerViewType { get; }
    Type HostViewType { get; }
    IDictionary<string, object?> Parameters { get; }
    void InitializeState(Session session);
}
