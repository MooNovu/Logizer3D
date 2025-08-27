using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameCanvasManager : MonoBehaviour
{
    [SerializeField] private UiAnimator _pauseMenu;
    [SerializeField] private UiAnimator _resaultMenu;

    [Inject] private ISceneSwitcher _sceneSwitcher;

    private void Start()
    {
        UIEvents.OnResaulMenu += ShowResaultMenu;
    }
    private void OnDestroy()
    {
        UIEvents.OnResaulMenu -= ShowResaultMenu;
    }
    public void NextLevelButton()
    {
        HideResaultMenu();
        GameEvents.LoadNextLevel();
    }
    public void ReloadLevel()
    {
        HidePauseMenu();
        GameEvents.ReloadLevel();
    }
    public void ShowPauseMenu()
    {
        _pauseMenu.OpenAnimation();
    }
    public void HidePauseMenu()
    {
        _pauseMenu.CloseAnimation();
    }
    public void ShowResaultMenu()
    {
        int levelId = 1;
        //_resaultMenu.GetComponentInChildren<TextMeshProUGUI>().text = $"Level {levelId} Complete!";
        _resaultMenu.OpenAnimation();
    }
    public void HideResaultMenu()
    {
        _resaultMenu.CloseAnimation();
    }

    public void ToMainMenu()
    {
        _sceneSwitcher.SwitchScene("MainMenu");
    }
}
