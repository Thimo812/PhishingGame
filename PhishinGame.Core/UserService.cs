
using Microsoft.AspNetCore.Http;
using System.Xml.Schema;

namespace PhishingGame.Core;

internal class UserService(IHttpContextAccessor contextAccessor) : IUserService
{
    private const string _idString = "UserId";

    private IHttpContextAccessor _contextAccessor = contextAccessor;

    public Guid GetUserId()
    {
        var context = _contextAccessor.HttpContext
            ?? throw new InvalidOperationException("No Http context available");

        return context.TryGetUserId(out var id) ? id : context.AppendUserId();
    }
}
