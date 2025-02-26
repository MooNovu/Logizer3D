using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionPanel : UIPanel
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Transform _levelButtonsContainter;

    [SerializeField] private GameObject prefab;

    protected override void Awake()
    {
        base.Awake();
        _backButton.onClick.AddListener(RequestPanel<MainMenuPanel>);
        InitializeStageLevelsUI();
    }
    private void InitializeStageLevelsUI()
    {
        List<LevelData> levels = LevelListManager.Instance.LoadStage("Stage1");

        foreach (LevelData level in levels)
        {
            CreateLevelUi(level);
        }
    }

    private void CreateLevelUi(LevelData level)
    {
        var levelButton = Instantiate(prefab);
        levelButton.transform.SetParent(_levelButtonsContainter);
        levelButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{level.Name}";
        levelButton.GetComponent<Button>().onClick.AddListener(() => LoadLevel(level));
        // Сделать обертку и LevelManager для загрузки
    }

    private void LoadLevel(LevelData level)
    {
        LevelDataHolder.SetLevel(level);
        SceneSwitcher.Instance.LoadLevel();
        RequestPanel<LoadingScreenPanel>();
    }
}
