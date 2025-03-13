using UnityEngine;
using Zenject;

public class BootstrapInstance : MonoBehaviour
{
    [Inject] private IStateMachine _stateMachine;
    private void Start()
    {
        _stateMachine.EnterIn<BootstrapLoadingState>();
    }
}
