namespace PhishingGame.Core;

public class Team
{
    public IList<Player> Players { get; set; } = [];
    public int score {  get; set; }
    public string Name { get; set; }
}
