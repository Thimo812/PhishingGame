namespace PhishingGame.Core;

public class Team
{
    public IList<Player> Players { get; set; } = new List<Player>();
    public int Score {  get; set; }
    public string Name { get; set; } = string.Empty;
}
