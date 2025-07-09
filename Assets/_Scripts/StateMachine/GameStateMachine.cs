using System;
using System.Collections.Generic;
using Zenject;

public class GameStateMachine :  IStateMachine
{
    private Dictionary<Type, IState> _states;
    private IState _currentState;
    public GameStateMachine()
    {
        _states = new Dictionary<Type, IState>()
        {
            [typeof(LoadingLevelState)] = new LoadingLevelState(this),
            [typeof(InitializeLevelState)] = new InitializeLevelState(this),

            [typeof(BootstrapLoadingState)] = new BootstrapLoadingState(this),
            [typeof(BootstrapInitializeState)] = new BootstrapInitializeState(this)
        };
    }

    public void EnterIn<TState>() where TState : IState
    {
        if (_states.TryGetValue(typeof(TState), out IState state))
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState?.Enter();
        }
    }
}

