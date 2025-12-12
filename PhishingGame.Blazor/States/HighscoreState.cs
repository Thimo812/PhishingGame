using PhishingGame.Blazor.Components.Pages.StateViews.Client;
using PhishingGame.Blazor.Components.Pages.StateViews.Host;
using PhishingGame.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using PhishingGame.Core.Models;

namespace PhishingGame.Blazor.States;

public class HighscoreState : LinkedStateBase<HighscoreHostView, HighscoreClientView>
{
    public List<Team> RankedTeams { get; private set; } = new();

    public override void InitializeState(Session session)
    {
        base.InitializeState(session);

        if (session?.SessionData?.Teams != null)
        {
            RankedTeams = session.SessionData.Teams
                .OrderByDescending(t => t.Score)
                .ThenBy(t => t.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
        else
        {
            RankedTeams = new List<Team>();
        }
    }
}