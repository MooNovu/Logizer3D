using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MainButtonPanel : MonoBehaviour
{
    [Header("Main Panel")]
    [SerializeField] private UiAnimator _mainPanel;

    [Header("Level Selection Panel")]
    [SerializeField] private UiAnimator _levelSelectionPanel;

    [Header("Redactor Level Selection Panel")]
    [SerializeField] private UiAnimator _redactorLevelSelectionPanel;
    public void OpenMainMenu(Sequence seq) => StartCoroutine(OpenMain(seq));
    public void OpenLevelSelection() => StartCoroutine(OpenLevelSel());
    public void OpenRedactorLevelSelection() => StartCoroutine(OpenRedactorLevelSel());




    private IEnumerator OpenMain(Sequence seq)
    {
        yield return seq.WaitForCompletion();
        _mainPanel.OpenAnimation();
    }
    private IEnumerator OpenLevelSel()
    {
        yield return _mainPanel.CloseAnimation().WaitForCompletion();
        _levelSelectionPanel.OpenAnimation();
    }
    private IEnumerator OpenRedactorLevelSel()
    {
        yield return _mainPanel.CloseAnimation().WaitForCompletion();
        _redactorLevelSelectionPanel.OpenAnimation();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
