using PhishingGame.Blazor.States;
using PhishingGame.Core;

namespace PhishingGame.Blazor;

public static class StateConfigurationExtensions
{
    public static IServiceCollection AddGameStates(this IServiceCollection services)
        => services.AddSessions(states => states
            .WithState<StartMenuState>()
            .WithState<TeamLayoutState>()
            .WithState<FlaggingRoundState>()
            .WithState<EmailCompositionState>()
            .WithState<FlaggingRoundState>());
}

