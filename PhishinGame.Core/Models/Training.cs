namespace PhishingGame.Core.Models;

public class Training : BaseModel
{
    public string Name { get; set; }
    public ICollection<Email> NormalMails { get; set; }
    public ICollection<Email> PhishingMails { get; set; }
}
