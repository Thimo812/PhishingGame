using PhishingGame.Blazor.Components.Pages.StateViews.Client;
using PhishingGame.Blazor.Components.Pages.StateViews.Host;
using PhishingGame.Core;
using PhishingGame.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhishingGame.Blazor.States;

public class HighscoreState : LinkedStateBase<HighscoreHostView, HighscoreClientView>
{
    public List<Team> RankedTeams { get; private set; } = new();

    public override void InitializeState(Session session)
    {
        base.InitializeState(session);
        RecalculateRanking();
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