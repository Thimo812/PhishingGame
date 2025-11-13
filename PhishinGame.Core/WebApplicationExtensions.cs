using Microsoft.AspNetCore.Builder;

namespace PhishingGame.Core;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder UseAnonymousUserId(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            if (!context.TryGetUserId(out var userId))
            {
                context.AppendUserId();
            }

            await next();
        });
    }
}
