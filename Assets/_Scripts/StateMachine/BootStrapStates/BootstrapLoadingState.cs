using UnityEngine;

public class BootstrapLoadingState : IState
{
    private readonly GameStateMachine _levelStateMachine;
    public BootstrapLoadingState(GameStateMachine levelStateMachine)
    {
        _levelStateMachine = levelStateMachine;
    }
    public void Enter()
    {
        //Загрузка из файлов и настройки
        _levelStateMachine.EnterIn<BootstrapInitializeState>();
    }

    public void Exit()
    {

    }
}
