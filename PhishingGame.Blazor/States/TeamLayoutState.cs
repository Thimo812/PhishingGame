using PhishingGame.Blazor.Components.Pages.StateViews.Client;
using PhishingGame.Blazor.Components.Pages.StateViews.Host;
using PhishingGame.Core;

namespace PhishingGame.Blazor.States;

public class TeamLayoutState : LinkedStateBase<TeamLayoutHostView, TeamLayoutClientView>
{
    public Team GetTeam(Guid playerId)
    {
        return Session.SessionData.Teams.FirstOrDefault(team => team.Players.Any(player => player.Id == playerId))
            ?? throw new InvalidDataException("Player not found");
    }
}
