using System;
using UnityEngine;
using Zenject;

public class BootstrapInstance : MonoBehaviour
{
    [Inject] private IStateMachine _stateMachine;
    private void Start()
    {
        int refreshRate = (int)Math.Round(Screen.currentResolution.refreshRateRatio.value);
        if (refreshRate < 60) refreshRate = 60;
        Application.targetFrameRate = refreshRate;
        QualitySettings.vSyncCount = 0;
        _stateMachine.EnterIn<BootstrapLoadingState>();
    }
}
