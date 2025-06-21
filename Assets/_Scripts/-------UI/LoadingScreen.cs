using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class LoadingScreen : MonoBehaviour, ILoadingScreen
{

    private void Awake()
    {
        //_root = _uiDocument.rootVisualElement;
        //_progressBar = _root.Q<VisualElement>("progress-bar");
        Hide();
    }

    public void Show()
    {
        //_root.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        //_root.style.display = DisplayStyle.None;
    }

    public void SetProgress(float progress)
    {
        //_progressBar.style.width = new StyleLength(Length.Percent(progress * 100));
    }

}

public interface ILoadingScreen
{
    public void Show();
    public void Hide();
    public void SetProgress(float progress);
}