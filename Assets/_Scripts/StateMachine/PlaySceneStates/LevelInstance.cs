using UnityEngine;
using Zenject;

public class LevelInstance : MonoBehaviour
{
    [Inject] private IStateMachine _stateMachine;
    private void Awake()
    {
        _stateMachine.EnterIn<LoadingLevelState>();
    }
}
