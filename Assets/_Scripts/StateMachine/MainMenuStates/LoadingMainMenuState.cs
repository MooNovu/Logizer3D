using UnityEngine;

public class LoadingMainMenuState : ILevelState
{
    private readonly LevelStateMachine _levelStateMachine;
    public LoadingMainMenuState(LevelStateMachine levelStateMachine)
    {
        _levelStateMachine = levelStateMachine;
    }
    public void Enter()
    {

        _levelStateMachine.EnterIn<InitializeMainMenuState>();
    }

    public void Exit()
    {

    }
}
