using PhishingGame.Core.Models;

namespace PhishingGame.Core;

public interface ISessionManager
{
    Session CreateSession(Training training, IServiceProvider scopedProvider);
    Session? GetSession(Guid sessionId);
}
