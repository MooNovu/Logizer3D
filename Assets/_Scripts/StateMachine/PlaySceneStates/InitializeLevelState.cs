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
        Debug.Log("Initializing Level Started");
    }

    public void Exit()
    {
        Debug.Log("Initializing Level Finished");
    }
}
