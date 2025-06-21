using UnityEngine;
using Zenject;

public class LevelInstance : MonoBehaviour
{
    [Inject] private IStateMachine _stateMachine;
    private void Start()
    {
        _stateMachine.EnterIn<LoadingLevelState>();
    }
}
