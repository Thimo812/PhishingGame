using PhishingGame.Blazor.Components.Pages.StateViews.Client;
using PhishingGame.Blazor.Components.Pages.StateViews.Host;
using PhishingGame.Core;
using PhishingGame.Core.Models;

namespace PhishingGame.Blazor.States;

public delegate void EmailsUpdatedCallback(Team team, List<Email> mails);
public class EmailCompositionState(Core.ITimer timer) : LinkedStateBase<EmailCompositionHostView, EmailCompositionClientView>
{
    public event EmailsUpdatedCallback EmailsUpdated;

    public Core.ITimer Timer { get; set; } = timer;

    public TimeSpan TotalTime => TimeSpan.FromMinutes(10);

    public override void InitializeState(Session session)
    {
        base.InitializeState(session);

        Timer.CountdownElapsed += async () => await Session.NextStateAsync();

        RemovePhishingMails();
    }

    public override void OnStateChanged()
    {
        ShuffleMails();
    }

    public void StartCountDown(CancellationToken token)
    {
        Timer.StartCountdown(TotalTime, token);
    }

    public void AddEmail(Team team, Email email)
    {
        var mails = Session.SessionData.Mails[team];
        mails.Add(email);
        NotifyEmailChanged(team);
    }

    public void NotifyEmailChanged(Team team)
    {
        var mails = Session.SessionData.Mails[team];
        EmailsUpdated?.Invoke(team, mails);
    }

    private void RemovePhishingMails()
    {
        foreach (var mails in Session.SessionData.Mails.Values)
        {
            mails.RemoveAll(email => email.IsPhishing);
        }
    }

    private void ShuffleMails()
    {
        var mailDict = Session.SessionData.Mails;
        var last = mailDict.Last().Value;

        for (int i = 0; i < mailDict.Count; i++)
        {
            var team = mailDict.ElementAt(i).Key;
            var temp = mailDict.ElementAt(i).Value;

            mailDict[team] = last;
            last = temp;
        }
    }
}
