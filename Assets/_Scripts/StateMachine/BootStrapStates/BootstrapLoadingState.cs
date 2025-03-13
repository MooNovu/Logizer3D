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
        GameObject canvas = GameObject.FindAnyObjectByType<Canvas>().gameObject;
        CanvasGroup logo = canvas.GetComponentInChildren<CanvasGroup>(true);
        logo.alpha = 0;
        logo.gameObject.SetActive(false);

        _levelStateMachine.EnterIn<BootstrapInitializeState>();
    }

    public void Exit()
    {

    }
}
