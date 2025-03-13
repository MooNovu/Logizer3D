using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelSelectionPanel : UIPanel
{
    [Inject] private readonly ISceneSwitcher sceneSwitcher;
    [Inject] private readonly ILevelList levelList;
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
        List<LevelData> levels = levelList.LoadStage("Stage1");

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
        sceneSwitcher.LoadLevel();
    }
}
