namespace PhishinGame.Core.Models;

public class Email : BaseModel
{
    public string Sender { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public bool IsPhishingMail { get; set; }
}
