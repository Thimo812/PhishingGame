using PhishingGame.Blazor.Components.Pages.StateViews.Client;
using PhishingGame.Blazor.Components.Pages.StateViews.Host;
using PhishingGame.Core;
using PhishingGame.Core.Models;

namespace PhishingGame.Blazor.States;

public delegate void EmailFlaggedCallback(Team team, Email mail);
public class FlaggingRoundState(Core.ITimer timer) : LinkedStateBase<FlaggingRoundHostView, FlaggingRoundClientView>
{
    public event EmailFlaggedCallback EmailFlagged;

    public Core.ITimer Timer { get; set; } = timer;
    public TimeSpan TotalTime => TimeSpan.FromMinutes(10);
    public Dictionary<Team, List<Email>> FlaggedMails { get; set; } = new();

    public override void InitializeState(Session session)
    {
        base.InitializeState(session);

        foreach (var team in Session.SessionData.Teams)
        {
            FlaggedMails.Add(team, new List<Email>());
        }

        Timer.CountdownElapsed += async () => await Session.NextStateAsync();
    }

    public void StartCountDown()
    {
        Timer.Start(TotalTime);
    }

    public void NotifyEmailFlagged(Team team, Email mail)
    {
        EmailFlagged?.Invoke(team, mail);
    }

    public override void OnStateChanged()
    {
        CalculateScores();
    }

    private void CalculateScores()
    {
        foreach ((Team team, List<Email> flaggedMails) in FlaggedMails)
        {
            var allMails = Session.SessionData.Mails[team];
            int mistakes = MistakeCount(allMails, flaggedMails);

            team.Score += 100 - (100 * mistakes / allMails.Count);
        }
    }

    private int MistakeCount(List<Email> allMails, List<Email> flagged)
    {
        return allMails.Count(mail => !mail.IsPhishing ^ flagged.Contains(mail));
    }
}
