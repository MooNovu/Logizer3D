using DG.Tweening;
using System;
using UnityEngine;

public class UIPanel : MonoBehaviour, IUIPanel, IUINavigation
{
    private CanvasGroup CanvasGroup => GetComponent<CanvasGroup>();
    float _fadeDuration = 0.2f;
    public bool IsVisible => gameObject.activeSelf;

    public event Action<Type> OnPanelRequested;

    protected virtual void Awake()
    {
        //HideImmediate();
    }
    public virtual void Show()
    {
        gameObject.SetActive(true);
        CanvasGroup.alpha = 0;
        CanvasGroup.DOFade(1, _fadeDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => CanvasGroup.blocksRaycasts = true);
    }
    public virtual void Hide()
    {
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.DOFade(0, _fadeDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => gameObject.SetActive(false));
    }
    public void HideImmediate()
    {
        gameObject.SetActive(false);
    }
    protected void RequestPanel<T>() where T : IUIPanel
    {
        OnPanelRequested?.Invoke(typeof(T));
    }
}