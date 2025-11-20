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

    protected override void OnParametersSet()
    {
        Session.PlayerJoined += OnPlayerJoined;
    }

    protected virtual async void OnPlayerJoined(Session session)
    {
        await InvokeAsync(StateHasChanged);
    }
}
