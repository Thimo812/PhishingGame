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
        return $"{_navigator.BaseUri}session/{Session.SessionId}";
    }

    public void StopSession()
    {
        Session.Stop();
        _navigator.NavigateTo("/");
    }

    public override async Task OnStateChangedAsync()
    {
        await Session.StartAsync();
    }
}
