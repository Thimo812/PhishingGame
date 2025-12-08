namespace PhishingGame.Core.Exceptions;

public class InvalidPlayerCountException : Exception
{
    public InvalidPlayerCountException(string? message) : base(message)
    {   
    }

    public InvalidPlayerCountException() : base()
    {
    }
}
