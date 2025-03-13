using UnityEngine;
using UnityEngine.UI;

public class PauseButtonPanel : UIPanel
{
    [SerializeField] private Button _pauseButton;

    protected override void Awake()
    {
        base.Awake();
        _pauseButton.onClick.AddListener(RequestPanel<PausePanel>);
    }
}
