
using PhishingGame.Core.Models;

namespace PhishingGame.Core;

public delegate void StateDefinitionBuilder(StateDefinition states);
internal class SessionManager(StateDefinition states) : ISessionManager
{
    private Dictionary<Guid, Session> _waitingSessions = [];
    private Dictionary<Guid, Session> _activeSessions = [];
    private StateDefinition _stateDefinition = states;

    public Session CreateSession(Training training)
    {
        var session = new Session(
            _stateDefinition.GetLinkedState(),
            training);

        _waitingSessions.Add(session.SessionId, session);

        AttachEvents(session);

        return session;
    }

    public Session? GetSession(Guid sessionId)
    {
        return 
            _activeSessions.TryGetValue(sessionId, out var session) ? session :
            _waitingSessions.TryGetValue(sessionId, out session) ? session :
            null;
    }

    private void AttachEvents(Session session)
    {
        session.SessionStarted += OnSessionStarted;
        session.SessionEnded += OnSessionEnded;
    }

    private void DetachEvents(Session session)
    {
        session.SessionStarted -= OnSessionStarted;
        session.SessionEnded -= OnSessionEnded;
    }

    private void OnSessionStarted(Session session)
    {
        _waitingSessions.Remove(session.SessionId);

        if (!_activeSessions.ContainsKey(session.SessionId))
        {
            _activeSessions.Add(session.SessionId, session);
        }
    }

    private void OnSessionEnded(Session session)
    {
        _activeSessions.Remove(session.SessionId);
        DetachEvents(session);
    }
}
