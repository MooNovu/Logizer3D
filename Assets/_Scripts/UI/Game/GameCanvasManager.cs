using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _resaultMenu;

    [Inject] private ISceneSwitcher _sceneSwitcher;

    private void Start()
    {
        UIEvents.OnResaulMenu += ShowResaultMenu;

        UIAnimationHandler.CloseAnimation(_pauseMenu, true);
        UIAnimationHandler.CloseAnimation(_resaultMenu, true);
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
        UIAnimationHandler.OpenAnimation(_pauseMenu);
    }
    public void HidePauseMenu()
    {
        UIAnimationHandler.CloseAnimation(_pauseMenu);
    }
    public void ShowResaultMenu()
    {
        int levelId = 1;
        _resaultMenu.GetComponentInChildren<TextMeshProUGUI>().text = $"Level {levelId} Complete!";
        UIAnimationHandler.OpenAnimation(_resaultMenu);
    }
    public void HideResaultMenu()
    {
        UIAnimationHandler.CloseAnimation(_resaultMenu);
    }

    public void ToMainMenu()
    {
        _sceneSwitcher.SwitchScene("MainMenu");
    }
}
