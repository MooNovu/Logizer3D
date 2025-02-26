using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
public class SceneFade : UIPanel
{
    private CanvasGroup canvasGroup => GetComponent<CanvasGroup>();
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Show()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 1;
        canvasGroup.DOFade(0, 3f).SetEase(Ease.InOutQuad).OnComplete(() => RequestPanel<MainMenuPanel>());
    }
    public override void Hide()
    {
        gameObject.SetActive(false);
    }
}
