using Microsoft.AspNetCore.Components;
using PhishingGame.Blazor.Components.Pages.StateViews.Client;
using PhishingGame.Blazor.Components.Pages.StateViews.Host;
using PhishingGame.Core;

namespace PhishingGame.Blazor.States;

public class StartMenuState(NavigationManager navigator) : LinkedStateBase<StartMenuHostView, StartMenuClientView>
{
    private NavigationManager _navigator = navigator;

    public string BuildUrl()
    {
        return $"{_navigator.BaseUri}/sessie/{Session.SessionId}";
    }

    public void StopSession()
    {
        Session.Stop();
        _navigator.NavigateTo("/");
    }

    public async Task StartSessionAsync()
    {
        await Session.StartAsync();
        await Session.NextStateAsync();
    }
}
