using UnityEngine;

public class LevelInstance : MonoBehaviour
{
    private LevelStateMachine _levelStateMachine;
    private void Awake()
    {
        _levelStateMachine = new();
        _levelStateMachine.EnterIn<LoadingLevelState>();
    }
}
