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

        Timer.CountdownElapsed += async () => await ContinueAsync();

        RemovePhishingMails();
    }

    public async Task ContinueAsync()
    {
        ShuffleMails();

        await Session.NextStateAsync();
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
        List<Email> firstValue = null;

        var mailDict = Session.SessionData.Mails;

        for (int i = 0; i < mailDict.Count; i++)
        {
            if (i == 0)
            {
                firstValue = mailDict.ElementAt(i).Value;
            }


        }
    }
}
