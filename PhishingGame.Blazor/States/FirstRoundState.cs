using PhishingGame.Blazor.Components.Pages.StateViews.Client;
using PhishingGame.Blazor.Components.Pages.StateViews.Host;
using PhishingGame.Core;
using PhishingGame.Core.Models;

namespace PhishingGame.Blazor.States;

public delegate void EmailFlaggedCallback(Team team, Email mail);
public class FirstRoundState(Core.ITimer timer) : LinkedStateBase<FirstRoundHostView, FirstRoundClientView>
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

        Timer.CountdownElapsed += OnCountdownElapsed;
    }

    public void StartCountDown(CancellationToken token)
    {
        Timer.StartCountdown(TotalTime, token);
    }

    public void NotifyEmailFlagged(Team team, Email mail)
    {
        EmailFlagged?.Invoke(team, mail);
    }

    private async void OnCountdownElapsed()
    {
        CalculateScores();
        await Session.NextStateAsync();
    }

    private void CalculateScores()
    {
        foreach ((Team team, List<Email> flaggedMails) in FlaggedMails)
        {
            var allMails = Session.SessionData.Mails[team];
            int mistakes = MistakeCount(allMails, flaggedMails);
            int phishingMailCount = allMails.Count(mail => mail.IsPhishing);

            team.score = (phishingMailCount - mistakes) * 100 / phishingMailCount;
        }
    }

    private int MistakeCount(List<Email> allMails, List<Email> flagged)
    {
        return allMails.Count(mail => !mail.IsPhishing ^ flagged.Contains(mail));
    }
}
