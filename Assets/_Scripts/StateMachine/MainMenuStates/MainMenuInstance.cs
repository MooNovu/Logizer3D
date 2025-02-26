using UnityEngine;

public class MainMenuInstance : MonoBehaviour
{
    private LevelStateMachine _levelStateMachine;
    private void Awake()
    {
        _levelStateMachine = new();
        _levelStateMachine.EnterIn<LoadingMainMenuState>();
    }
}
