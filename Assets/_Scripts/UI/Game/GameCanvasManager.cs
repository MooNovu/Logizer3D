using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameCanvasManager : MonoBehaviour
{
    [SerializeField] private Image _inputField;

    [SerializeField] private CanvasGroup _allUi;
    [SerializeField] private GameObject _inGamePauseBtn;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _resaultMenu;

    [Inject] private ISceneSwitcher _sceneSwitcher;

    private CanvasGroup _pauseMenuCanvasGroup;
    private CanvasGroup _resaultMenuCanvasGroup;
    
    private Transform _pauseMenuTransform;
    private Transform _resaultMenuTransform;

    private bool _isPauseMenuOpen = false;
    //private bool _isResaultMenuOpen = false;

    private void Awake()
    {
        UIEvents.OnResaulMenu += ShowResaultMenu;
        UIEvents.OnBlockUI += BlockUi;
        UIEvents.OnUnblockUI += UnblockUi;
    }
    private void OnDestroy()
    {
        UIEvents.OnResaulMenu -= ShowResaultMenu;
        UIEvents.OnBlockUI -= BlockUi;
        UIEvents.OnUnblockUI -= UnblockUi;
    }

    private void Start()
    {
        _inGamePauseBtn.SetActive(true);
        _pauseMenu.SetActive(true);
        _resaultMenu.SetActive(true);
        _inputField.raycastTarget = true;
        _pauseMenuCanvasGroup = _pauseMenu.GetComponent<CanvasGroup>();
        _resaultMenuCanvasGroup = _resaultMenu.GetComponent<CanvasGroup>();
        _pauseMenuTransform = _pauseMenu.GetComponent<Transform>();
        _resaultMenuTransform = _resaultMenu.GetComponent<Transform>();

        _pauseMenuTransform.DOScale(new Vector3(0, 0, 0), 0);
        _resaultMenuTransform.DOScale(new Vector3(0, 0, 0), 0);
        _pauseMenuCanvasGroup.alpha = 0;
        _resaultMenuCanvasGroup.alpha = 0;
    }
    public void NextLevelButton()
    {
        HideResaultMenu();
        GameEvents.LoadNextLevel();
    }
    public void ReloadLevel()
    {
        TogglePauseMenu();
        GameEvents.OnLevelReload();
    }

    public void TogglePauseMenu()
    {
        if (_isPauseMenuOpen)
        {
            HidePauseMenu();
        }
        else
        {
            ShowPauseMenu();
        }
    }
    public void ShowPauseMenu()
    {
        _isPauseMenuOpen = true;
        _pauseMenuTransform.DOScale(new Vector3(1, 1, 1), 0.1f).SetEase(Ease.InQuad);
        _pauseMenuCanvasGroup.DOFade(1f, 0.1f).SetEase(Ease.InQuad);
        _pauseMenuCanvasGroup.blocksRaycasts = true;
        _inputField.raycastTarget = false;
    }
    public void HidePauseMenu()
    {
        _isPauseMenuOpen = false;
        _pauseMenuTransform.DOScale(new Vector3(0, 0, 0), 0.1f).SetEase(Ease.InQuad);
        _pauseMenuCanvasGroup.DOFade(0f, 0.1f).SetEase(Ease.InQuad);
        _pauseMenuCanvasGroup.blocksRaycasts = false;
        _inputField.raycastTarget = true;
    }
    public void ShowResaultMenu()
    {
        int levelId = 1;
        _resaultMenu.GetComponentInChildren<TextMeshProUGUI>().text = $"Level {levelId} Complete!";
        _resaultMenuTransform.DOScale(new Vector3(1, 1, 1), 0.1f).SetEase(Ease.InQuad);
        _resaultMenuCanvasGroup.DOFade(1f, 0.1f).SetEase(Ease.InQuad);
        _resaultMenuCanvasGroup.blocksRaycasts = true;
        _inputField.raycastTarget = false;
    }
    public void HideResaultMenu()
    {
        _resaultMenuTransform.DOScale(new Vector3(0, 0, 0), 0.1f).SetEase(Ease.InQuad);
        _resaultMenuCanvasGroup.DOFade(0f, 0.1f).SetEase(Ease.InQuad);
        _resaultMenuCanvasGroup.blocksRaycasts = false;
        _inputField.raycastTarget = true;
    }

    public void ToMainMenu()
    {
        _sceneSwitcher.SwitchScene("MainMenu");
    }

    public void BlockUi()
    {
        _allUi.interactable = false;
        _allUi.blocksRaycasts = false;
    }
    public void UnblockUi()
    {
        _allUi.interactable = true;
        _allUi.blocksRaycasts = true;
    }
}
