namespace PhishingGame.Core.Models;

public class Team
{
    public IList<Player> Players { get; set; } = [];
    public int Score { get; set; }
    public string Name { get; set; }
}
