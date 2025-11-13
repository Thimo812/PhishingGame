using Microsoft.AspNetCore.Http;

namespace PhishingGame.Core;

public static class HttpContextExtensions
{
    public const string IdKey = "UserId";
    public static Guid AppendUserId(this HttpContext context)
    {
        var id = Guid.NewGuid();
        context.Response.Cookies.Append(IdKey, id.ToString(), new()
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTimeOffset.UtcNow.AddDays(1)
        });

        return id;
    }

    public static bool TryGetUserId(this HttpContext context, out Guid userId)
    {
        userId = Guid.Empty;
        return context.Request.Cookies.TryGetValue(IdKey, out var guidString) &&
            Guid.TryParse(guidString, out userId);
    }
}
