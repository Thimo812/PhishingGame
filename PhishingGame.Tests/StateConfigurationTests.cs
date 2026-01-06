using NUnit.Framework;
using PhishingGame.Core;
using System;
using System.Threading.Tasks;

namespace PhishingGame.Tests
{
    internal class StateConfigurationTests
    {
        private class DummyProvider : IServiceProvider
        {
            public object GetService(Type serviceType) => null!;
        }

        private class DummyState : ILinkedState
        {
            public Session Session { get; set; }
            public ILinkedState NextState { get; set; }
            public Type PlayerViewType => typeof(object);
            public Type HostViewType => typeof(object);
            public IDictionary<string, object?> Parameters { get; } = new System.Collections.Generic.Dictionary<string, object?>();
            public void InitializeState(Session session) => Session = session;
            public void OnStateChanged() { }
            public Task OnStateChangedAsync() => Task.CompletedTask;
        }

        [Test]
        public void WithState_AddsStateAndGetLinkedState_CreatesChain()
        {
            var config = new StateConfiguration();
            config.WithState<DummyState>().WithState<DummyState>();

            var provider = new DummyProvider();

            var linked = config.GetLinkedState(provider);

            Assert.IsNotNull(linked);
            Assert.IsNotNull(linked.NextState);
        }

        [Test]
        public void GetLinkedState_ThrowsWhenEmpty()
        {
            var config = new StateConfiguration();
            var provider = new DummyProvider();

            Assert.Throws<NullReferenceException>(() => config.GetLinkedState(provider));
        }
    }
}
