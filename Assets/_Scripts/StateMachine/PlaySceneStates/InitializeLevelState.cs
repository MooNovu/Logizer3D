using UnityEngine;

public class InitializeLevelState : IState
{
    private readonly GameStateMachine _levelStateMachine;
    public InitializeLevelState(GameStateMachine levelStateMachine)
    {
        _levelStateMachine = levelStateMachine;
    }
    public void Enter()
    {

    }

    public void Exit()
    {

    }
}
