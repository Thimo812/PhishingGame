namespace PhishingGame.Core;

public delegate void CountDownCallback();
public interface ITimer
{
    event CountDownCallback CountdownUpdated;
    event CountDownCallback CountdownElapsed;

    TimeSpan RemainingTime { get; set; }
    bool Active { get; set; }
    void Start(TimeSpan totalTime);
}
