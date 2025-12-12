namespace PhishingGame.Core;

public delegate void CountDownCallback();
public interface ITimer
{
    event CountDownCallback CountdownUpdated;
    event CountDownCallback CountdownElapsed;

    TimeSpan RemainingTime { get; }

    Task StartCountdown(TimeSpan totalTime, CancellationToken token);
}
