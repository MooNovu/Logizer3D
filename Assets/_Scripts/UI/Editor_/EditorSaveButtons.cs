using UnityEngine;
using Zenject;

public class EditorSaveButtons : MonoBehaviour
{
    [Inject] ISceneSwitcher _sceneSwitcher;
    [SerializeField] private GameObject _panel;

    private void Start()
    {
        _panel.SetActive(true);
        UIAnimationHandler.CloseAnimation(_panel, true);
    }

    public void OpenMenu()
    {
        UIAnimationHandler.OpenAnimation(_panel);
    }
    public void CloseMenu()
    {
        UIAnimationHandler.CloseAnimation(_panel);
    }

    public void QuitAndSaveLevel()
    {
        
        LevelConverter converter = new(CurrentLevelHandler.LevelData.width, CurrentLevelHandler.LevelData.height);
        GridSystem gridSystem = converter.LoadLevelFromSceneObject();

        SaveManager.SaveLevel(gridSystem, CurrentLevelHandler.LevelData.Name);

        _sceneSwitcher.SwitchScene("MainMenu");
    }
    public void QuitWoSaving()
    {
        _sceneSwitcher.SwitchScene("MainMenu");
    }
}
