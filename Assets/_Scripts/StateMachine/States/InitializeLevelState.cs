using UnityEngine;

public class InitializeLevelState : ILevelState
{
    private readonly LevelStateMachine _levelStateMachine;
    public InitializeLevelState(LevelStateMachine levelStateMachine)
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
