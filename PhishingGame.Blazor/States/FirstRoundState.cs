using PhishingGame.Blazor.Components.Pages.StateViews.Client;
using PhishingGame.Blazor.Components.Pages.StateViews.Host;
using PhishingGame.Core;
using PhishingGame.Core.Models;
using System.Net.Mail;

namespace PhishingGame.Blazor.States;

public delegate void CountDownCallback();
public delegate void EmailFlaggedCallback(Team team, Email mail);
public class FirstRoundState : LinkedStateBase<FirstRoundHostView, FirstRoundClientView>
{
    public event CountDownCallback CountdownUpdated;
    public event EmailFlaggedCallback EmailFlagged;

    public TimeSpan TotalTime => TimeSpan.FromMinutes(10);

    private TimeSpan _remainingTime;
    public TimeSpan RemainingTime
    {
        get => _remainingTime;
        set
        {
            _remainingTime = value;
            CountdownUpdated?.Invoke();
        }
    }

    public Dictionary<Team, List<Email>> FlaggedMails { get; set; } = new();

    public override void InitializeState(Session session)
    {
        base.InitializeState(session);

        foreach (var team in Session.SessionData.Teams)
        {
            FlaggedMails.Add(team, new List<Email>());
        }
    }

    public void StartCountDown(CancellationToken token)
    {
        CountDown(token);
    }

    public async Task CountDown(CancellationToken token)
    {
        RemainingTime = TotalTime;
        while(RemainingTime.TotalSeconds > 0 && !token.IsCancellationRequested)
        {
            await Task.Delay(1000);
            RemainingTime = RemainingTime.Subtract(TimeSpan.FromSeconds(1));
        }
        OnCountdownElapsed();
    }

    public void OnEmailFlagged(Team team, Email mail)
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
