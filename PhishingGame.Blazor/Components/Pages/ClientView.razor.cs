using Microsoft.AspNetCore.Components;
using PhishingGame.Core;

namespace PhishingGame.Blazor.Components.Pages;

public partial class ClientView
{
    [Inject]
    private ISessionManager _sessionManager { get; set; } = default!;

    [Inject]
    private IUserService _userService { get; set; } = default!;

    [Parameter]
    public Guid SessionId { get; set; }

    [Parameter]
    public Session? Session { get; set; }
    public ILinkedState? CurrentState => Session?.CurrentState;
    public Guid UserId {  get; set; }
    public bool ContainsPlayer { get; set; }

    private void RegisterPlayer(string name)
    {
        if (Session?.CanJoin == true)
        {
            Session.AddPlayer(UserId, name);
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        Session = _sessionManager.GetSession(SessionId);
        UserId = _userService.GetUserId();
        ContainsPlayer = Session?.ContainsPlayer(UserId) ?? false;
    }
}
