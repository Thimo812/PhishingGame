using System.Collections.ObjectModel;

namespace PhishingGame.Core;

public abstract class LinkedStateBase<THostView, TPlayerView> : ILinkedState
    where THostView : IGameView
    where TPlayerView : IGameView
{
    protected SessionData SessionData { get; set; }
    public ILinkedState NextState { get; set; }
    public Type PlayerViewType => typeof(TPlayerView);
    public Type HostViewType => typeof(THostView);
    public IDictionary<string, object?> Parameters { get; private set; }

    public void InitializeState(SessionData sessionData)
    {
        SessionData = sessionData;
        Parameters = new Dictionary<string, object?>
        {
            ["State"] = this
        };
    }
}
