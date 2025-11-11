namespace PhishingGame.Core;

public interface ILinkedState
{
    ILinkedState NextState { get; set; }
    IGameView PlayerView { get; }
    IGameView HostView { get; }
    void InitializeState(SessionData sessionData);
}
