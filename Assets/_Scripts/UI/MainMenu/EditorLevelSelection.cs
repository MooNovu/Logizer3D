using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EditorLevelSelection : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _uiRedactorPanelPrefab;

    [Header("New Level Panel")]
    [SerializeField] private GameObject _newLevelPanel;
    [SerializeField] private TMP_InputField _levelNameInputField;
    [SerializeField] private TMP_InputField _gridSizeInputField;

    [Inject] private readonly ISceneSwitcher _sceneSwitcher;

    private List<RedactorPanel> _panelContainer = new();

    private void Start()
    {
        _newLevelPanel.SetActive(true);
        UIAnimationHandler.CloseAnimation(_newLevelPanel, true);
        CreateButtons();
    }

    public void OpenNewLevelPanel()
    {
        _levelNameInputField.text = "";
        _gridSizeInputField.text = "";
        UIAnimationHandler.OpenAnimation(_newLevelPanel);
    }
    public void CloseNewLevelPanel()
    {
        UIAnimationHandler.CloseAnimation(_newLevelPanel);
    }
    public void CreateNewLevel()
    {
        if (string.IsNullOrEmpty(_gridSizeInputField.text) || string.IsNullOrEmpty(_levelNameInputField.text))
        {
            Debug.Log("Empty Inputs");
            return;
        }
        if (int.TryParse(_gridSizeInputField.text, out int size))
        {
            string levelName = _levelNameInputField.text;
            SaveManager.SaveLevel(new GridSystem(size, size), levelName);
            AddPanel(levelName);
            CloseNewLevelPanel();
            return;
        }
        Debug.LogError("Not A number while trying to parse gridSize");
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
        RedactorPanel panel = new(_uiRedactorPanelPrefab, _parent, levelName, _sceneSwitcher);
        _panelContainer.Add(panel);
    }

    private class RedactorPanel
    {
        private readonly GameObject _panel;
        private readonly TextMeshProUGUI _title;
        private readonly Button _editButton;
        private readonly Button _playButton;
        private readonly ISceneSwitcher _sceneSwitcher;
        public RedactorPanel(GameObject prefab,
            Transform parent,
            string levelName,
            ISceneSwitcher sceneSwitcher)
        {
            _sceneSwitcher = sceneSwitcher;

            _panel = GameObject.Instantiate(prefab, parent);

            _title = _panel.transform.Find("LevelName").GetComponent<TextMeshProUGUI>();
            _editButton = _panel.transform.Find("EditButton").GetComponent<Button>();
            _playButton = _panel.transform.Find("PlayButton").GetComponent<Button>();
            SetLevelName(levelName);
        }

        public void SetLevelName(string levelName)
        {
            _title.text = levelName;
            SetCallbacks(levelName);
        }
        private void SetCallbacks(string levelName)
        {
            _editButton.onClick.RemoveAllListeners();
            _playButton.onClick.RemoveAllListeners();
            _editButton.onClick.AddListener(
                () =>
                {
                    CurrentLevelHandler.SetLevel(LevelList.GetUserLevel(levelName));
                    _sceneSwitcher.SwitchScene("Redactor");
                }
                );
            _playButton.onClick.AddListener(
                () =>
                {
                    CurrentLevelHandler.SetLevel(LevelList.GetUserLevel(levelName));
                    _sceneSwitcher.SwitchScene("Game");
                }
                );
        }
    }

}
