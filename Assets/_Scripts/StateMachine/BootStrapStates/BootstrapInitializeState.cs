using DG.Tweening;
using UnityEngine;
using Zenject;

public class BootstrapInitializeState : IState
{
    private readonly GameStateMachine _levelStateMachine;
    public BootstrapInitializeState(GameStateMachine levelStateMachine)
    {
        _levelStateMachine = levelStateMachine;
    }
    public void Enter()
    {
        LogoAnimation lg = GameObject.FindFirstObjectByType<LogoAnimation>();
        lg.StartAnimation();
    }

    public void Exit()
    {

    }
}