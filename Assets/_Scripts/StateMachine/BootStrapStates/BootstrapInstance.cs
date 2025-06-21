using UnityEngine;
using Zenject;

public class BootstrapInstance : MonoBehaviour
{
    [Inject] private IStateMachine _stateMachine;
    private void Start()
    {
        Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 0;
        _stateMachine.EnterIn<BootstrapLoadingState>();
    }
}
