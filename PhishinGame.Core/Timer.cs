
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
            set
            {
                _remainingTime = value;
                CountdownUpdated?.Invoke();
            }
        }

        private bool _active = false;
        public bool Active
        {
            get => _active;
            set
            {
                if (_active == value) return;
                _active = value;

                if (_active && _tokenSource.TryReset())
                {
                    StartCountdown(_tokenSource.Token);
                    return;
                }

                _tokenSource.Cancel();
            }
        }

        private CancellationTokenSource _tokenSource = new(); 

        private async Task StartCountdown(CancellationToken token)
        {
            while (RemainingTime.TotalSeconds > 0 && !token.IsCancellationRequested)
            {
                await Task.Delay(1000);
                RemainingTime = RemainingTime.Subtract(TimeSpan.FromSeconds(1));
            }

            if (!token.IsCancellationRequested) CountdownElapsed?.Invoke();
        }

        public void Start(TimeSpan totalTime)
        {
            RemainingTime = totalTime;
            Active = true;
        }
    }
}
