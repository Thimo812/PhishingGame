using PhishingGame.Blazor.Components.Pages.StateViews.Client;
using PhishingGame.Blazor.Components.Pages.StateViews.Host;
using PhishingGame.Core;
using PhishingGame.Core.Models;

namespace PhishingGame.Blazor.States;

public delegate void EmailsUpdatedCallback(Team team, List<Email> mails);
public class EmailCompositionState : LinkedStateBase<EmailCompositionHostView, EmailCompositionClientView>
{
    public event EmailsUpdatedCallback EmailsUpdated;

    public override void InitializeState(Session session)
    {
        base.InitializeState(session);

        RemovePhishingMails();
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
}
