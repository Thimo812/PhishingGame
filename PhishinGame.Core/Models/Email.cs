namespace PhishingGame.Core.Models;

public class Email : BaseModel
{
    public string Sender { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public bool IsPhishing { get; set; }

    public List<Training> Trainings { get; set; } = new();
}
