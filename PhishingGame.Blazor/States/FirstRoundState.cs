using PhishingGame.Blazor.Components.Pages.StateViews.Client;
using PhishingGame.Blazor.Components.Pages.StateViews.Host;
using PhishingGame.Core;
using PhishingGame.Core.Models;
using System.Linq;

namespace PhishingGame.Blazor.States;

public delegate void EmailFlaggedCallback(Team team, Email mail);
public class FirstRoundState(Core.ITimer timer) : LinkedStateBase<FirstRoundHostView, FirstRoundClientView>
{
    public event EmailFlaggedCallback EmailFlagged;

    public Core.ITimer Timer { get; set; } = timer;
    public TimeSpan TotalTime => TimeSpan.FromMinutes(10);


    public Dictionary<Team, HashSet<Guid>> FlaggedMails { get; set; } = new();

    public override void InitializeState(Session session)
    {
        base.InitializeState(session);

        FlaggedMails.Clear();
        foreach (var team in Session.SessionData.Teams)
        {
            FlaggedMails.Add(team, new HashSet<Guid>());
        }

        Timer.CountdownElapsed += OnCountdownElapsedAsync;
    }

    public void StartCountDown(CancellationToken token)
    {
        Timer.StartCountdown(TotalTime, token);
    }

    public void NotifyEmailFlagged(Team team, Email mail)
    {
        if (team == null || mail == null) return;

        if (!FlaggedMails.TryGetValue(team, out var set))
        {
            set = new HashSet<Guid>();
            FlaggedMails[team] = set;
        }


        if (set.Contains(mail.Id))
            set.Remove(mail.Id);
        else
            set.Add(mail.Id);

        EmailFlagged?.Invoke(team, mail);
    }

    private async void OnCountdownElapsedAsync()
    {

        CalculateScores();
        await Session.NextStateAsync();
    }

    // score : score = round( (goodAnswers / totalEmails) * 100 )
    // goed antwoord: (mail.IsPhishing && flagged) || (!mail.IsPhishing && !flagged)
    private void CalculateScores()
    {
        if (Session?.SessionData?.Mails == null) return;


        foreach (var kv in FlaggedMails.ToList())
        {
            var team = kv.Key;
            var flaggedIdsSnapshot = new HashSet<Guid>(kv.Value ?? new HashSet<Guid>());

            if (!Session.SessionData.Mails.TryGetValue(team, out var allMails) || allMails == null)
            {
                team.Score = 0;
                continue;
            }

            int total = allMails.Count;
            if (total == 0)
            {
                team.Score = 0;
                continue;
            }

            int goodAnswers = 0;
            foreach (var mail in allMails)
            {
                bool isFlagged = flaggedIdsSnapshot.Contains(mail.Id);
                if (mail.IsPhishing == isFlagged) goodAnswers++;
            }

            double percent = (double)goodAnswers / total * 100.0;
            team.Score = (int)Math.Round(percent);


            Console.WriteLine($"[FirstRoundState] Team='{team.Name}' good={goodAnswers}/{total} score={team.Score}");
        }
    }
}