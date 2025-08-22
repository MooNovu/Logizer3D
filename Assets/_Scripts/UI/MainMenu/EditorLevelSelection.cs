using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EditorLevelSelection : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _uiRedactorPanelPrefab;

    [Header("New Level Panel")]
    [SerializeField] private UiAnimator _newLevelPanel;
    [SerializeField] private TMP_InputField _levelNameInputField;
    [SerializeField] private int _gridSize;

    [Header("ConfirmPanel")]
    [SerializeField] private UiAnimator _confirmPanel;

    [Inject] private readonly ISceneSwitcher _sceneSwitcher;

    private RedactorPanel _panelToDelete = null;

    private void Start()
    {
        CreateButtons();
    }

    public void OpenNewLevelPanel()
    {
        _levelNameInputField.text = "";
        _newLevelPanel.OpenAnimation();
    }
    public void CloseNewLevelPanel()
    {
        _newLevelPanel.CloseAnimation();
    }
    public void CreateNewLevel()
    {
        if (string.IsNullOrEmpty(_levelNameInputField.text))
        {
            Debug.Log("Empty Inputs");
            return;
        }
        string levelName = _levelNameInputField.text;
        SaveFileManager.SaveLevel(new GridSystem(_gridSize, _gridSize), levelName);
        AddPanel(levelName);
        CloseNewLevelPanel();
    }

    private void CreateButtons()
    {
        foreach (string levelName in LevelList.GetAllUserLevelNames())
        {
            AddPanel(levelName);
        }
    }
    private void AddPanel(string levelName)
    {
        RedactorPanel _ = new(_uiRedactorPanelPrefab, _parent, levelName, SwitchScene, DeleteLevel);
    }

    private void SwitchScene(string sceneName)
    {
        _sceneSwitcher.SwitchScene(sceneName);
    }
    private void DeleteLevel(RedactorPanel panel)
    {
        _confirmPanel.OpenAnimation();
        _panelToDelete = panel;

    }
    public void DeleteLevel()
    {
        SaveFileManager.DeleteLevel(_panelToDelete.LevelName);
        Destroy(_panelToDelete.PanelGO);
        _panelToDelete = null;
        _confirmPanel.CloseAnimation();
    }
    public void CancelDeleting()
    {
        _panelToDelete = null;
    }

    private class RedactorPanel
    {
        public readonly GameObject PanelGO;
        public readonly string LevelName;
        private readonly TextMeshProUGUI _title;
        private readonly Button _editButton;
        private readonly Button _deleteButton;
        private readonly Button _playButton;
        private readonly Action<string> switchScene;
        private readonly Action<RedactorPanel> deleteAction;
        public RedactorPanel(GameObject prefab,
            Transform parent,
            string levelName,
            Action<string> switchScene,
            Action<RedactorPanel> deleteAction)
        {
            this.switchScene = switchScene;
            this.deleteAction = deleteAction;
            LevelName = levelName;
            PanelGO = GameObject.Instantiate(prefab, parent);

            _title = PanelGO.transform.Find("LevelName").GetComponent<TextMeshProUGUI>();
            _editButton = PanelGO.transform.Find("EditButton").GetComponent<Button>();
            _deleteButton = PanelGO.transform.Find("DeleteButton").GetComponent<Button>();
            _playButton = PanelGO.transform.Find("PlayButton").GetComponent<Button>();
            SetLevelName();
        }

        public void SetLevelName()
        {
            _title.text = LevelName;
            SetCallbacks();
        }
        private void SetCallbacks()
        {
            _editButton.onClick.RemoveAllListeners();
            _deleteButton.onClick.RemoveAllListeners();
            _playButton.onClick.RemoveAllListeners();
            _editButton.onClick.AddListener(
                () =>
                {
                    CurrentLevelHandler.SetLevel(LevelList.GetUserLevel(LevelName));
                    switchScene.Invoke("Redactor");
                }
                );
            _playButton.onClick.AddListener(
                () =>
                {
                    CurrentLevelHandler.SetLevel(LevelList.GetUserLevel(LevelName));
                    switchScene.Invoke("Game");
                }
                );
            _deleteButton.onClick.AddListener(
                () =>
                {
                    deleteAction.Invoke(this);
                }
                );
        }
    }

}
