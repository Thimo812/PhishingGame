using Microsoft.Extensions.DependencyInjection;

namespace PhishingGame.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSessions(this IServiceCollection services, StateDefinitionBuilder stateBuilder)
        => services
            .AddSingleton<ISessionManager, SessionManager>()
            .AddScoped<IUserService, UserService>()
            .AddSingleton(services =>
            {
                var states = new StateDefinition(services);
                stateBuilder(states);

                return states;
            });
}
