using PhishingGame.Blazor.Components.Pages.StateViews.Client;
using PhishingGame.Blazor.Components.Pages.StateViews.Host;
using PhishingGame.Core;

namespace PhishingGame.Blazor.States;

public delegate void CountDownCallback();
public class FirstRoundState : LinkedStateBase<FirstRoundHostView, FirstRoundClientView>
{
    public event CountDownCallback CountdownElapsed;
    public event CountDownCallback CountdownUpdated;

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

    public FirstRoundState()
    {
        CountdownElapsed += OnCountdownElapsed;
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
        CountdownElapsed?.Invoke();
    }

    private async void OnCountdownElapsed()
    {
        await Session.NextStateAsync();
    }
}
