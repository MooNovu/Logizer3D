using UnityEngine;
using Zenject;

public class EditorSaveButtons : MonoBehaviour
{
    [Inject] ISceneSwitcher _sceneSwitcher;
    private UiAnimator _panel;

    private void Start()
    {
        _panel = GetComponent<UiAnimator>();
    }

    public void OpenMenu()
    {
        _panel.OpenAnimation();
    }

    public void CloseMenu()
    {

    }

    public void QuitAndSaveLevel()
    {
        
        LevelConverter converter = new(CurrentLevelHandler.LevelData.width, CurrentLevelHandler.LevelData.height);
        GridSystem gridSystem = converter.LoadLevelFromSceneObject();

        SaveFileManager.SaveLevel(gridSystem, CurrentLevelHandler.LevelData.Name, CurrentLevelHandler.LevelData.Description);

        _sceneSwitcher.SwitchScene("MainMenu");
    }
    public void QuitWoSaving()
    {
        _sceneSwitcher.SwitchScene("MainMenu");
    }
}
