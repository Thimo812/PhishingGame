
namespace PhishingGame.Core
{
    internal class Timer : ITimer
    {
        public event CountDownCallback CountdownUpdated;
        public event CountDownCallback CountdownElapsed;

        private TimeSpan _remainingTime;
        public TimeSpan RemainingTime
        {
            get => _remainingTime;
            private set
            {
                _remainingTime = value;
                CountdownUpdated?.Invoke();
            }
        }

        public async Task StartCountdown(TimeSpan totalTime, CancellationToken token)
        {
            RemainingTime = totalTime;
            while (RemainingTime.TotalSeconds > 0 && !token.IsCancellationRequested)
            {
                await Task.Delay(1000);
                RemainingTime = RemainingTime.Subtract(TimeSpan.FromSeconds(1));
            }
            CountdownElapsed?.Invoke();
        }
    }
}
