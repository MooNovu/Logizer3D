using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;


//Класс говно переделать
public static class UIHelper
{
    public static void SetButtonFunction(Button thisButton, UnityAction func)
    {
        if (thisButton == null) return;

        thisButton.transform.DOScale(0.9f, 0.2f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                thisButton.transform.DOScale(1, 0.2f);
                func();
            });
    }
}