using Microsoft.AspNetCore.Components;
using PhishingGame.Core;
using System.Runtime.CompilerServices;

namespace PhishingGame.Blazor.Components.Pages;

public partial class GameView
{
    [Inject]
    private ISessionManager _sessionManager { get; set; } = default!;

    [Inject]
    private IUserService _userService { get; set; } = default!;

    [Parameter]
    public Guid SessionId { get; set; }

    private Session? _session;
    [Parameter]
    public Session? Session
    {
        get => _session;
        set
        {
            if (_session != null) 
                _session.StateUpdated -= OnStateUpdated;

            _session = value;
            _session.StateUpdated += OnStateUpdated;
        }
    }
    public ILinkedState? CurrentState => Session?.CurrentState;
    public Guid UserId {  get; set; }
    public bool IsHost {  get; set; }
    public Type? ViewType => IsHost ? CurrentState?.HostViewType : CurrentState?.PlayerViewType;
    public bool ContainsPlayer => Session?.ContainsPlayer(UserId) ?? false;

    private void RegisterPlayer(string name)
    {
        if (Session?.CanJoin == true)
        {
            Session.AddPlayer(UserId, name);
        }
        InvokeAsync(StateHasChanged);
    }

    private async void OnStateUpdated(Session session)
    {
        await InvokeAsync(StateHasChanged);
    }

    protected override async Task OnParametersSetAsync()
    {
        Session = _sessionManager.GetSession(SessionId);
        UserId = _userService.GetUserId();
        IsHost = Session.HostId == UserId;
    }
}
