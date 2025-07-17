using UnityEngine;
using DG.Tweening;

public static class UIAnimationHandler
{
    private readonly static float Duration = 0.2f;
    public static void OpenAnimation(GameObject uiElement, bool removeAnimation = false)
    {
        uiElement.SetActive(true);

        if (uiElement.TryGetComponent(out CloseUiPanel closePanel))
        {
            closePanel.CreateBackgroundPanel();
        }

        float dur = !removeAnimation ? Duration : 0f;

        uiElement.transform.DOScale(Vector3.one, dur).SetEase(Ease.OutExpo).From(Vector3.zero);
    }
    public static void CloseAnimation(GameObject uiElement, bool removeAnimation = false)
    {
        if (uiElement.TryGetComponent(out CloseUiPanel closePanel))
        {
            closePanel.DestroyPanel();
        }

        float dur = !removeAnimation ? Duration : 0f;

        uiElement.transform.DOScale(Vector3.zero, dur).SetEase(Ease.InExpo).OnComplete(() => uiElement.SetActive(false));
    }
}
