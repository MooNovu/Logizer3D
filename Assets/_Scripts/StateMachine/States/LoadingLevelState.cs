using UnityEngine;

public class LoadingLevelState : ILevelState
{
    private readonly LevelStateMachine _levelStateMachine;
    public LoadingLevelState(LevelStateMachine levelStateMachine)
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
