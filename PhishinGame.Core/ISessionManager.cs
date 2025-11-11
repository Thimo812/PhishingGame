using PhishingGame.Core.Models;

namespace PhishingGame.Core;

public interface ISessionManager
{
    Session CreateSession(Training training);
    Session? GetSession(Guid sessionId);
}
