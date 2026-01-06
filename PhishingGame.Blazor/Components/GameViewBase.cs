using Microsoft.AspNetCore.Components;
using PhishingGame.Core;
using PhishingGame.Core.Models;

namespace PhishingGame.Blazor.Components;

public abstract class GameViewBase<TState> : ComponentBase, IGameView
    where TState : ILinkedState
{
    [Parameter]
    public Guid UserId { get; set; }

    [Parameter]
    public TState State { get; set; }

    [Parameter]
    public Team Team { get; set; }

    protected Session Session => State.Session;
}
