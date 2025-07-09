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
        GameEvents.LoadLevel(CurrentLevelHandler.LevelData);
    }

    public void Exit()
    {

    }
}
