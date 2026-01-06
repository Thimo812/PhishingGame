using PhishingGame.Core;
using PhishingGame.Core.Models;
using PhishingGame.Core.Exceptions;

namespace PhishingGame.Tests;

[TestFixture]
public class SessionTests
{
    private Training MakeTraining(int normalCount, int phishingCount)
    {
        var t = new Training { Name = "t" };
        for (int i = 0; i < normalCount; i++)
            t.Emails.Add(new Email { Sender = $"n{i}", Subject = "s", Message = "m", IsPhishing = false });
        for (int i = 0; i < phishingCount; i++)
            t.Emails.Add(new Email { Sender = $"p{i}", Subject = "s", Message = "m", IsPhishing = true });
        return t;
    }

    [Test]
    public void AddPlayer_AddsPlayerAndRaisesEvent()
    {
        var state = new TestState();
        var training = MakeTraining(0,0);
        var session = new Session(state, training);

        bool invoked = false;
        session.PlayerJoined += s => invoked = true;

        var id = Guid.NewGuid();
        session.AddPlayer(id, "bob");

        Assert.IsTrue(invoked);
        Assert.IsTrue(session.ContainsPlayer(id));
        Assert.AreEqual(1, session.SessionData.Players.Count);
        Assert.AreEqual("bob", session.SessionData.Players[0].Name);
    }

    [Test]
    public void Initialize_CallsStateInitialize()
    {
        var state = new TestState();
        var training = MakeTraining(0,0);
        var session = new Session(state, training);

        session.Initialize();

        Assert.IsTrue(state.InitializeCalled);
        Assert.AreSame(session, state.Session);
    }

    [Test]
    public void StartAsync_ThrowsWhenLessThanTwoPlayers()
    {
        var state = new TestState();
        var training = MakeTraining(0,0);
        var session = new Session(state, training);

        session.AddPlayer(Guid.NewGuid(), "one");

        Assert.ThrowsAsync<InvalidPlayerCountException>(async () => await session.StartAsync());
    }

    [Test]
    public async Task StartAsync_CreatesTeams_DispatchesMails_And_raises_event()
    {
        var state = new TestState();
        var training = MakeTraining(2,2);
        var session = new Session(state, training);

        var ids = Enumerable.Range(0,4).Select(_ => Guid.NewGuid()).ToList();
        foreach (var id in ids) session.AddPlayer(id, id.ToString());

        bool started = false;
        session.SessionStarted += s => started = true;

        await session.StartAsync();

        Assert.IsTrue(started);
        Assert.IsFalse(session.CanJoin);

        Assert.IsTrue(session.SessionData.Teams.Count >= 1);

        int totalPlayersInTeams = session.SessionData.Teams.Sum(t => t.Players.Count);
        Assert.AreEqual(4, totalPlayersInTeams);

        Assert.IsTrue(session.SessionData.Mails.Count == session.SessionData.Teams.Count);
        var allAssigned = session.SessionData.Mails.SelectMany(kv => kv.Value).ToList();
        Assert.AreEqual(4, allAssigned.Count);
        Assert.AreEqual(2, allAssigned.Count(e => e.IsPhishing));
        Assert.AreEqual(2, allAssigned.Count(e => !e.IsPhishing));
    }

    [Test]
    public async Task NextStateAsync_CallsStateChangeAndTransitionsAndRaisesEvent()
    {
        var first = new TestState();
        var second = new TestState();
        first.NextState = second;
        var training = MakeTraining(0,0);
        var session = new Session(first, training);

        bool updated = false;
        session.StateUpdated += s => updated = true;

        await session.NextStateAsync();

        Assert.AreEqual(1, first.OnStateChangedCallCount);
        Assert.AreEqual(1, first.OnStateChangedAsyncCallCount);

        Assert.AreSame(second, session.CurrentState);
        Assert.IsTrue(second.InitializeCalled);
        Assert.IsTrue(updated);
    }

    [Test]
    public void Stop_SetsCurrentStateNull_And_RaisesEvent()
    {
        var state = new TestState();
        var training = MakeTraining(0,0);
        var session = new Session(state, training);

        bool ended = false;
        session.SessionEnded += s => ended = true;

        session.Stop();

        Assert.IsNull(session.CurrentState);
        Assert.IsTrue(ended);
    }

    [Test]
    public void TryGetPlayer_ReturnsExpected()
    {
        var state = new TestState();
        var training = MakeTraining(0,0);
        var session = new Session(state, training);

        var id = Guid.NewGuid();
        session.AddPlayer(id, "x");

        Assert.IsTrue(session.TryGetPlayer(id, out var player));
        Assert.AreEqual("x", player.Name);

        Assert.IsFalse(session.TryGetPlayer(Guid.NewGuid(), out _));
    }

    [Test]
    public void ContainsPlayer_Overloads_Work()
    {
        var state = new TestState();
        var training = MakeTraining(0,0);
        var session = new Session(state, training);

        var id = Guid.NewGuid();
        session.AddPlayer(id, "x");

        var p = session.SessionData.Players.First();
        Assert.IsTrue(session.ContainsPlayer(p));
        Assert.IsTrue(session.ContainsPlayer(p.Id));
        Assert.IsFalse(session.ContainsPlayer(Guid.NewGuid()));
    }

    [Test]
    public async Task CreateTeams_DistributesPlayers_RoundRobin_WhenNotEven()
    {
        var state = new TestState();
        var training = MakeTraining(0,0);
        var session = new Session(state, training);

        var ids = Enumerable.Range(0,5).Select(_ => Guid.NewGuid()).ToList();
        foreach (var id in ids) session.AddPlayer(id, id.ToString());

        await session.StartAsync();

        int totalPlayersInTeams = session.SessionData.Teams.Sum(t => t.Players.Count);
        Assert.AreEqual(5, totalPlayersInTeams);

        var counts = session.SessionData.Teams.Select(t => t.Players.Count).ToList();
        Assert.IsTrue(counts.Max() - counts.Min() <= 1);
    }
}
