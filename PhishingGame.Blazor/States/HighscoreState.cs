using PhishingGame.Blazor.Components.Pages.StateViews.Client;
using PhishingGame.Blazor.Components.Pages.StateViews.Host;
using PhishingGame.Core;

public class HighscoreState : LinkedStateBase<HighscoreHostView, HighscoreClientView>
{
    public List<Team> RankedTeams { get; private set; } = new();

    public event Action? Changed;

    public override void InitializeState(Session session)
    {
        base.InitializeState(session);

        Session.SessionDataChanged += OnSessionDataChanged;

        RecalculateRanking();
        Changed?.Invoke();
    }

    private void OnSessionDataChanged()
    {
        RecalculateRanking();
        Changed?.Invoke();
    }

    public void RecalculateRanking()
    {
        if (Session?.SessionData?.Teams == null)
        {
            RankedTeams = new List<Team>();
            return;
        }

        RankedTeams = Session.SessionData.Teams
            .OrderByDescending(t => t.Score)
            .ThenBy(t => t.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}
