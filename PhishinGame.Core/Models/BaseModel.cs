using System.Runtime.CompilerServices;

namespace PhishingGame.Core.Models;

public class BaseModel
{
    private Guid _id;
    public Guid Id
    {
        get
        {
            if (_id == Guid.Empty) _id = Guid.NewGuid(); 
            return _id;
        }
        set => _id = value;
    }
}
