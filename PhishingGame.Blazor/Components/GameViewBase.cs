using Microsoft.AspNetCore.Components;
using PhishingGame.Core;

namespace PhishingGame.Blazor.Components;

public abstract class GameViewBase<TState> : ComponentBase, IGameView
    where TState : ILinkedState
{
    [Parameter]
    public Guid UserId { get; set; }

    [Parameter]
    public TState State { get; set; }

    protected Session Session => State.Session;
}
