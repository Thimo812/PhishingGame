using Microsoft.AspNetCore.Components;
using PhishingGame.Blazor.Components.Pages.StateViews.Client;
using PhishingGame.Blazor.Components.Pages.StateViews.Host;
using PhishingGame.Core;

namespace PhishingGame.Blazor.States;

public class StartMenuState(NavigationManager navigator, IUserService userService) : LinkedStateBase<StartMenuHostView, StartMenuClientView>
{
    private NavigationManager _navigator = navigator;
    private IUserService _userService = userService;

    public string BuildUrl()
    {
        return $"{_navigator.BaseUri}/sessie/{Session.SessionId}";
    }

    public void StopSession()
    {
        Session.Stop();
        _navigator.NavigateTo("/");
    }

    public string GetUserName()
    {
        var userId = _userService.GetUserId();
        if (!Session.TryGetPlayer(userId, out var player))
        {
            throw new InvalidDataException("Player is not present");
        }
        return player.Name;
    }
}
