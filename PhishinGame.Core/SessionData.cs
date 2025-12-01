using PhishingGame.Core.Models;

namespace PhishingGame.Core;

public class SessionData
{
    public Training Training { get; set; }
    public IList<Player> Players { get; set; } = [];
    public IList<Team> Teams { get; set; } = [];
    public Dictionary<Team, List<Email>> Mails { get; set; } = [];
}
