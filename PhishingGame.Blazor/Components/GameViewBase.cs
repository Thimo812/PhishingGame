using Microsoft.AspNetCore.Components;
using PhishingGame.Core;

namespace PhishingGame.Blazor.Components;

public abstract class GameViewBase<TState> : ComponentBase, IGameView
    where TState : ILinkedState
{
    [Inject]
    public IUserService UserService { get; set; } = default!;

    [Parameter]
    public TState State { get; set; }
    protected Session Session => State.Session;
}
