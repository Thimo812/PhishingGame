using NUnit.Framework;
using PhishingGame.Core;
using System;
using System.Threading.Tasks;

namespace PhishingGame.Tests
{
    internal class TimerTests
    {
        [Test]
        public async Task Start_SetsRemainingTimeAndActive_And_CountdownElapses()
        {
            var timer = new PhishingGame.Core.Timer();
            TimeSpan elapsedRemaining = TimeSpan.Zero;
            bool elapsed = false;

            timer.CountdownUpdated += () => elapsedRemaining = timer.RemainingTime;
            timer.CountdownElapsed += () => elapsed = true;

            timer.Start(TimeSpan.FromSeconds(2));

            Assert.IsTrue(timer.Active);
            Assert.AreEqual(TimeSpan.FromSeconds(2), timer.RemainingTime);

            await Task.Delay(2600);

            Assert.IsTrue(elapsed);
            Assert.IsTrue(timer.RemainingTime.TotalSeconds <= 0);
        }

        [Test]
        public async Task Active_Toggle_ResetsOrCancels()
        {
            var timer = new PhishingGame.Core.Timer();
            timer.Start(TimeSpan.FromSeconds(3));
            Assert.IsTrue(timer.Active);

            timer.Active = false;
            Assert.IsFalse(timer.Active);

            timer.Active = true;
            Assert.IsTrue(timer.Active);

            timer.Active = false;
            await Task.Delay(100);
        }
    }
}
