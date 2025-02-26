using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenPanel : UIPanel
{
    private Slider _slider;

    protected override void Awake()
    {
        base.Awake();
        _slider = GetComponentInChildren<Slider>();
    }
    private void Update()
    {
        _slider.value = SceneSwitcher.Instance.LoadingProgress;
    }

}
