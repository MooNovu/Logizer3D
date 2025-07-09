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
        //�������� �� ������ � ���������
        _levelStateMachine.EnterIn<BootstrapInitializeState>();
    }

    public void Exit()
    {

    }
}
