using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EditorLevelSelectionMenu : MonoBehaviour
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

    [Header("Redactor Level Panel")]
    [SerializeField] private UiAnimator _redactorLevelPanel;

    private RedactorPanel _currentPanel = null;

    private UiAnimator uiAnim => GetComponent<UiAnimator>();

    private void Start()
    {
        CreateButtons();
    }

    public void OpenNewLevelPanel()
    {
        uiAnim.CloseAnimation();
        _levelNameInputField.text = "";
        _newLevelPanel.OpenAnimation();
    }
    public void CloseNewLevelPanel()
    {
        _newLevelPanel.CloseAnimation();
        uiAnim.OpenAnimation();
    }

    public void OpenCurrentLevel(LevelData level)
    {
        uiAnim.CloseAnimation();
        _redactorLevelPanel.OpenAnimation();
        _redactorLevelPanel.gameObject.GetComponent<EditorLevelCardMenu>();
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
        RedactorPanel _ = new(_uiRedactorPanelPrefab, _parent, levelName, DeleteLevel);
    }
    private void DeleteLevel(RedactorPanel panel)
    {
        _confirmPanel.OpenAnimation();
        _currentPanel = panel;

    }
    public void DeleteLevel()
    {
        SaveFileManager.DeleteLevel(_currentPanel.LevelName);
        Destroy(_currentPanel.PanelGO);
        _currentPanel = null;
        _confirmPanel.CloseAnimation();
    }
    public void CancelDeleting()
    {
        _currentPanel = null;
    }

    private class RedactorPanel
    {
        public readonly GameObject PanelGO;
        public readonly string LevelName;
        private readonly TextMeshProUGUI _title;
        private readonly Button _moreButton;
        private readonly Action<RedactorPanel> deleteAction;
        public RedactorPanel(GameObject prefab,
            Transform parent,
            string levelName,
            Action<RedactorPanel> deleteAction)
        {
            this.deleteAction = deleteAction;
            LevelName = levelName;
            PanelGO = GameObject.Instantiate(prefab, parent);

            _title = PanelGO.transform.Find("LevelName").GetComponent<TextMeshProUGUI>();
            _moreButton = PanelGO.transform.Find("MoreButton").GetComponent<Button>();
            SetLevelName();
        }

        public void SetLevelName()
        {
            _title.text = LevelName;
            SetCallbacks();
        }
        private void SetCallbacks()
        {
            _moreButton.onClick.RemoveAllListeners();
            _moreButton.onClick.AddListener(
                () =>
                    {
                        CurrentLevelHandler.SetLevel(LevelList.GetUserLevel(LevelName));
                    }
                );
        }
    }

}
