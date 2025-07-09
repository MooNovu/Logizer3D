using UnityEngine;
using DG.Tweening;

public static class UIAnimationHandler
{
    private readonly static float Duration = 0.1f;
    public static void OpenAnimation(GameObject uiElement, bool removeAnimation = false)
    {
        CanvasGroup canvasGroup = uiElement.GetComponent<CanvasGroup>();

        float dur = !removeAnimation ? Duration : 0f;

        uiElement.transform.DOScale(Vector3.one, dur).SetEase(Ease.InQuad);
        canvasGroup.DOFade(1f, dur).SetEase(Ease.InQuad);
        canvasGroup.blocksRaycasts = true;
    }
    public static void CloseAnimation(GameObject uiElement, bool removeAnimation = false)
    {
        CanvasGroup canvasGroup = uiElement.GetComponent<CanvasGroup>();

        float dur = !removeAnimation ? Duration : 0f;

        uiElement.transform.DOScale(Vector3.zero, dur).SetEase(Ease.InQuad);
        canvasGroup.DOFade(0f, dur).SetEase(Ease.InQuad);
        canvasGroup.blocksRaycasts = false;
    }
}
