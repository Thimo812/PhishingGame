namespace PhishingGame.Core;

public interface ILinkedState
{
    ILinkedState NextState { get; set; }
    Type PlayerViewType { get; }
    Type HostViewType { get; }
    IDictionary<string, object?> Parameters { get; }
    void InitializeState(SessionData sessionData);
}
