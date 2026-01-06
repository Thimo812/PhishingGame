using PhishingGame.Core;

namespace PhishingGame.Tests;

internal class TestState : LinkedStateBase<IGameView, IGameView>
{
    public bool InitializeCalled { get; private set; }
    public int OnStateChangedCallCount { get; private set; }
    public int OnStateChangedAsyncCallCount { get; private set; }

    public override void InitializeState(Session session)
    {
        base.InitializeState(session);

        Session = session;
        InitializeCalled = true;
    }

    public override void OnStateChanged() => OnStateChangedCallCount++;

    public override Task OnStateChangedAsync()
    {
        OnStateChangedAsyncCallCount++;
        return Task.CompletedTask;
    }
}
