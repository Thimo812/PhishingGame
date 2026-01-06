namespace PhishingGame.Core.Models;

public class Training : BaseModel
{
    public string Name { get; set; }
    public List<Email> Emails { get; set; } = [];
}
