using DG.Tweening;
using UnityEngine;

public class MainButtonPanel : MonoBehaviour
{
    [Header("Main Panel")]
    [SerializeField] private GameObject _mainPanel;

    [Header("Level Selection Panel")]
    [SerializeField] private GameObject _levelSelectionPanel;

    [Header("Redactor Level Selection Panel")]
    [SerializeField] private GameObject _redactorLevelSelectionPanel;

    private void Start()
    {
        _mainPanel.SetActive(true);

        _levelSelectionPanel.SetActive(true);
        UIAnimationHandler.CloseAnimation(_levelSelectionPanel, true);

        _redactorLevelSelectionPanel.SetActive(true);
        UIAnimationHandler.CloseAnimation(_redactorLevelSelectionPanel, true);
    }

    public void OpenLevelSelection()
    {
        UIAnimationHandler.OpenAnimation(_levelSelectionPanel);
    }
    public void CloseLevelSelection()
    {
        UIAnimationHandler.CloseAnimation(_levelSelectionPanel);
    }
    public void OpenRedactorLevelSelection()
    {
        UIAnimationHandler.OpenAnimation(_redactorLevelSelectionPanel);
    }
    public void CloseRedactorLevelSelection()
    {
        UIAnimationHandler.CloseAnimation(_redactorLevelSelectionPanel);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
