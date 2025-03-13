using UnityEngine;

public class LoadingLevelState : IState
{
    private readonly GameStateMachine _levelStateMachine;
    public LoadingLevelState(GameStateMachine levelStateMachine)
    {
        _levelStateMachine = levelStateMachine;
    }
    public void Enter()
    {
        Debug.Log("Loading Level Started");

        _levelStateMachine.EnterIn<InitializeLevelState>();
    }

    public void Exit()
    {
        Debug.Log("Loading Level Finished");
    }
}
