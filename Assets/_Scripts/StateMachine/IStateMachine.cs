public interface IStateMachine
{
    void EnterIn<TState>() where TState : IState;
}
