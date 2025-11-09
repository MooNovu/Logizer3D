using System;
using System.Collections.Generic;
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

    private List<RedactorPanel> _panels = new();

    private RedactorPanel _panelToDelete = null;

    private UiAnimator uiAnim => GetComponent<UiAnimator>();

    private void Start()
    {
        InitialCardCreate();
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

    public void OpenCurrentLevel(string levelName)
    {
        uiAnim.CloseAnimation();
        _redactorLevelPanel.OpenAnimation();
        
        _redactorLevelPanel.gameObject.GetComponent<EditorLevelCardMenu>().Set(levelName, LevelList.GetUserLevel(levelName).Description, StartDeletingLevel);
    }
    public void CloseCurrentLevel()
    {
        uiAnim.OpenAnimation();
        _redactorLevelPanel.CloseAnimation();
    }

    public void CreateNewLevel()
    {
        if (string.IsNullOrEmpty(_levelNameInputField.text))
        {
            Debug.Log("Empty Inputs");
            return;
        }
        string levelName = _levelNameInputField.text;
        string discription = string.Empty;
        SaveFileManager.SaveLevel(new GridSystem(_gridSize, _gridSize), levelName, discription);
        AddPanel(levelName);
        CloseNewLevelPanel();
    }

    private void InitialCardCreate()
    {
        foreach (string levelName in LevelList.GetAllUserLevelNames())
        {
            AddPanel(levelName);
        }
    }
    private void AddPanel(string levelName)
    {
        _panels.Add(new RedactorPanel(_uiRedactorPanelPrefab, _parent, levelName, OpenCurrentLevel));
    }
    public void StartDeletingLevel(string levelName)
    {
        _confirmPanel.OpenAnimation();
        _redactorLevelPanel.CloseAnimation();
        foreach (var panel in _panels)
        {
            if (panel.LevelName == levelName)
            {
                _panelToDelete = panel;
                return;
            }
        }

    }
    public void DeleteLevel()
    {
        SaveFileManager.DeleteLevel(_panelToDelete.LevelName);
        _panels.Remove(_panelToDelete);
        Destroy(_panelToDelete.PanelGO);
        _panelToDelete = null;
        _confirmPanel.CloseAnimation();
        uiAnim.OpenAnimation();
    }
    public void CancelDeleting()
    {
        _redactorLevelPanel.OpenAnimation();
        _panelToDelete = null;
    }

    private class RedactorPanel
    {
        public readonly GameObject PanelGO;
        public readonly string LevelName;
        private readonly TextMeshProUGUI _title;
        private readonly Button _moreButton;
        private readonly Action<string> _openCurrentCard;
        public RedactorPanel(GameObject prefab, Transform parent, string levelName, Action<string> openCurrentCard)
        {
            _openCurrentCard = openCurrentCard;
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
                        _openCurrentCard(LevelName);
                    }
                );
        }
    }

}
