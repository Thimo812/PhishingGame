using Microsoft.Extensions.DependencyInjection;

namespace PhishingGame.Core;

public class StateConfiguration
{
    private List<Type> _stateTypes = [];

    public StateConfiguration WithState<TState>() where TState : class, ILinkedState
    {
        _stateTypes.Add(typeof(TState));
        return this;
    }

    public ILinkedState GetLinkedState(IServiceProvider scopedProvider)
    {
        ILinkedState? first = null;
        ILinkedState? current = null; 

        foreach (var type in _stateTypes)
        {
            var state = (ILinkedState)ActivatorUtilities.CreateInstance(scopedProvider, type);
            first ??= state;

            if (current != null) current.NextState = state;

            current = state;
        }

        return first ?? throw new NullReferenceException();
    }
}
