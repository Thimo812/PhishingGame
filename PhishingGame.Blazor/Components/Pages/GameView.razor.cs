using Microsoft.AspNetCore.Components;
using PhishingGame.Core;

namespace PhishingGame.Blazor.Components.Pages;

public partial class GameView
{
    [Inject]
    private ISessionManager _sessionManager { get; set; } = default!;

    [Inject]
    private IUserService _userService { get; set; } = default!;

    [Inject]
    private NavigationManager _navigator {  get; set; } = default!;

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
            if (_session == null) return;
            _session.StateUpdated += OnStateUpdated;
        }
    }
    public ILinkedState? CurrentState => Session?.CurrentState;
    public Guid UserId {  get; set; }
    public Team? Team {  get; set; }
    public bool IsHost {  get; set; }
    public Type? ViewType => IsHost ? CurrentState?.HostViewType : CurrentState?.PlayerViewType;
    public IDictionary<string, object> ViewParameters { get; set; }
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
        UpdateViewParameters();
        await InvokeAsync(StateHasChanged);
    }

    protected override async Task OnParametersSetAsync()
    {
        Session = _sessionManager.GetSession(SessionId);

        if (Session == null)
        {
            _navigator.NavigateTo("/notfound");
            return;
        }

        UserId = _userService.GetUserId();
        Team = GetTeam();
        IsHost = Session?.HostId == UserId;
        UpdateViewParameters();
    }

    private void UpdateViewParameters()
    {
        Team ??= GetTeam();

        ViewParameters = new Dictionary<string, object>(CurrentState.Parameters)
        {
            ["UserId"] = UserId,
            ["Team"] = Team
        }; 
    }

    private Team? GetTeam()
        => Session?.SessionData.Teams.FirstOrDefault(team => team.Players.Any(player => player.Id == UserId));
}
