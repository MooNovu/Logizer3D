using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PausePanel : UIPanel
{
    [Inject] private readonly ISceneSwitcher _sceneSwitcher;

    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _resumeButton;

    protected override void Awake()
    {
        base.Awake();
        _menuButton.onClick.AddListener(() => _sceneSwitcher.LoadMainMenu());
        _resumeButton.onClick.AddListener(() => Hide());
    }
}
