using UnityEngine;
using DG.Tweening;
using System.Collections;
public class InitializeMainMenuState : ILevelState
{
    private readonly LevelStateMachine _levelStateMachine;

    public InitializeMainMenuState(LevelStateMachine levelStateMachine)
    {
        _levelStateMachine = levelStateMachine;
    }
    public void Enter()
    {
        CameraAppearence();
    }

    public void Exit()
    {

    }

    private void CameraAppearence()
    {
        GameObject camera = GameObject.FindAnyObjectByType<Camera>().gameObject;
        Vector3 targetPos = camera.transform.position;

        camera.transform.position += new Vector3(0, 5, 0);

        camera.transform.DOMove(targetPos, 3f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => camera.transform.position = targetPos);

    }
}