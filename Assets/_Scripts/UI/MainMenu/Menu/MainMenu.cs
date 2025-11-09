using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private UiAnimator _mainPanel => GetComponent<UiAnimator>();

    [Header("Story Levels Panel")]
    [SerializeField] private UiAnimator _storyLevels;

    [Header("Redactor Levels Panel")]
    [SerializeField] private UiAnimator _redactorLevels;

    [Header("Online Levels Panel")]
    [SerializeField] private UiAnimator _onlineLevels;

    public void OpenMainMenu(Sequence seq) => StartCoroutine(OpenMain(seq));
    public void OpenStoryLevels() => StartCoroutine(OpenStoryLvls());
    public void OpenRedactorLevels() => StartCoroutine(OpenRedactorLvls());

    public void OpenOnlineLevels() => StartCoroutine(OpenOnlineLvls());


    private IEnumerator OpenMain(Sequence seq)
    {
        yield return seq.WaitForCompletion();
        _mainPanel.OpenAnimation();
    }
    private IEnumerator OpenStoryLvls()
    {
        yield return _mainPanel.CloseAnimation().WaitForCompletion();
        _storyLevels.OpenAnimation();
    }
    private IEnumerator OpenRedactorLvls()
    {
        yield return _mainPanel.CloseAnimation().WaitForCompletion();
        _redactorLevels.OpenAnimation();
    }
    private IEnumerator OpenOnlineLvls()
    {
        yield return _mainPanel.CloseAnimation().WaitForCompletion();
        _onlineLevels.OpenAnimation();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
