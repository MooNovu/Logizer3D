using UnityEngine;
using DG.Tweening;

public class LoadingScreen : MonoBehaviour, ILoadingScreen
{
    private CanvasGroup _canvasGroup;
    private float _animDuration = 0.5f;
    private void Awake()
    {
        _canvasGroup = GetComponentInChildren<CanvasGroup>(true);
    }

    public void FadeIn()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.DOFade(0, _animDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() => _canvasGroup.blocksRaycasts = false);
    }

    public void FadeOut()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.DOFade(1, _animDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() => _canvasGroup.blocksRaycasts = true);
    }

}

public interface ILoadingScreen
{
    public void FadeIn();
    public void FadeOut();
}