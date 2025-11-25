using PhishingGame.Blazor.Components.Pages.StateViews.Client;
using PhishingGame.Blazor.Components.Pages.StateViews.Host;
using PhishingGame.Core;
using System.Diagnostics;

namespace PhishingGame.Blazor.States;

public delegate void CountDownCallback();
public class FirstRoundState : LinkedStateBase<FirstRoundHostView, FirstRoundClientView>
{
    public event CountDownCallback CountdownElapsed;

    public TimeSpan TotalTime => TimeSpan.FromSeconds(10);
    public TimeSpan RemainingTime { get; set; }
    
    public void StartCountDown()
    {
        CountDown();
    }

    public async Task CountDown()
    {
        while(RemainingTime.Seconds > 0)
        {
            await Task.Delay(1000);
            RemainingTime = RemainingTime.Subtract(TimeSpan.FromSeconds(1));
        }

        await Session.NextStateAsync();
    }
}
