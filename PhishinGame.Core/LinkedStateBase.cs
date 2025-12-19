using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace PhishingGame.Core;

public abstract class LinkedStateBase<THostView, TPlayerView> : ILinkedState
    where THostView : IGameView
    where TPlayerView : IGameView
{
    public Session Session { get; set; }
    public ILinkedState NextState { get; set; }
    public Type PlayerViewType => typeof(TPlayerView);
    public Type HostViewType => typeof(THostView);
    public IDictionary<string, object> Parameters { get; private set; }

    public virtual void InitializeState(Session session)
    {
        Session = session;
        Parameters = new Dictionary<string, object>
        {
            ["State"] = this
        };
    }

    public virtual void OnStateChanged() { }
    public virtual Task OnStateChangedAsync() => Task.CompletedTask;
}
