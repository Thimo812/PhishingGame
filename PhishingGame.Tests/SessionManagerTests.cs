using NUnit.Framework;
using PhishingGame.Core;
using PhishingGame.Core.Models;
using System;
using System.Threading.Tasks;

namespace PhishingGame.Tests
{
    internal class SessionManagerTests
    {
        private class DummyUserService : IUserService
        {
            private Guid _id;
            public DummyUserService(Guid id) => _id = id;
            public Guid GetUserId() => _id;
        }

        private class SimpleServiceProvider : IServiceProvider
        {
            private readonly object _service;
            public SimpleServiceProvider(object service) => _service = service;
            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(IUserService)) return _service;
                return null!;
            }
        }

        [Test]
        public void CreateSession_AddsToWaitingSessionsAndSetsHostId()
        {
            var hostId = Guid.NewGuid();
            var userService = new DummyUserService(hostId);
            var provider = new SimpleServiceProvider(userService);

            var config = new StateConfiguration();
            config.WithState<TestState>();

            var manager = new SessionManager(config);

            var training = new Training { Name = "x" };

            var session = manager.CreateSession(training, provider);

            Assert.IsNotNull(session);
            Assert.AreEqual(hostId, session.HostId);
            Assert.IsNotNull(manager.GetSession(session.SessionId));
        }

        [Test]
        public void GetSession_ReturnsNullForUnknown()
        {
            var config = new StateConfiguration();
            var manager = new SessionManager(config);

            var s = manager.GetSession(Guid.NewGuid());
            Assert.IsNull(s);
        }

        [Test]
        public void SessionStarted_MovesFromWaitingToActive()
        {
            var userService = new DummyUserService(Guid.NewGuid());
            var provider = new SimpleServiceProvider(userService);

            var config = new StateConfiguration();
            config.WithState<TestState>();

            var manager = new SessionManager(config);
            var training = new Training { Name = "x" };
            var session = manager.CreateSession(training, provider);

            // add two players so StartAsync succeeds
            session.AddPlayer(Guid.NewGuid(), "p1");
            session.AddPlayer(Guid.NewGuid(), "p2");

            session.StartAsync().Wait();

            var found = manager.GetSession(session.SessionId);
            Assert.IsNotNull(found);
        }
    }
}
