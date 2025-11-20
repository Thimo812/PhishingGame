using Microsoft.Extensions.DependencyInjection;

namespace PhishingGame.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSessions(this IServiceCollection services, StateDefinitionBuilder stateBuilder)
        => services
            .AddSingleton<ISessionManager, SessionManager>(services =>
            {
                var stateConfig = new StateConfiguration();
                stateBuilder(stateConfig);

                return new SessionManager(stateConfig);
            })
            .AddScoped<IUserService, UserService>();
}
