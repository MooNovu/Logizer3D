using DG.Tweening;
using UnityEngine;
using Zenject;

public class BootstrapInitializeState : IState
{
    private readonly GameStateMachine _levelStateMachine;
    private readonly ISceneSwitcher sceneSwitcher;
    public BootstrapInitializeState(GameStateMachine levelStateMachine, ISceneSwitcher _sceneSwitcher)
    {
        _levelStateMachine = levelStateMachine;
        sceneSwitcher = _sceneSwitcher;
    }
    public void Enter()
    {
        LogoAnimation();
    }

    public void Exit()
    {

    }

    private void LogoAnimation()
    {
        GameObject canvas = GameObject.FindAnyObjectByType<Canvas>().gameObject;
        CanvasGroup logo = canvas.GetComponentInChildren<CanvasGroup>(true);
        logo.gameObject.SetActive(true);
        DOTween.Sequence()
            .Append(logo.DOFade(1, 1f).SetEase(Ease.OutQuad))
            .AppendInterval(1f)
            .Append(logo.DOFade(0, 1f).SetEase(Ease.OutQuad))
            .AppendCallback(SwitchScene);

    }
    private void SwitchScene()
    {
        sceneSwitcher.SwitchScene("GameScene");
    }
}