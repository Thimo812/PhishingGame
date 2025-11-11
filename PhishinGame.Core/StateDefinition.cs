using Microsoft.Extensions.DependencyInjection;

namespace PhishingGame.Core;

public class StateDefinition(IServiceProvider services)
{
    private List<Type> _stateTypes = [];
    private IServiceProvider _services = services;

    public StateDefinition WithState<TState>() where TState : class, ILinkedState
    {
        _stateTypes.Add(typeof(TState));
        return this;
    }

    public ILinkedState GetLinkedState()
    {
        ILinkedState? first = null;
        ILinkedState? current = null; 

        foreach (var type in _stateTypes)
        {
            var state = (ILinkedState)ActivatorUtilities.CreateInstance(_services, _stateTypes[0]);
            first ??= state;

            if (current != null) current.NextState = state;

            current = state;
        }

        return first ?? throw new NullReferenceException();
    }
}
