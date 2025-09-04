using DG.Tweening;
using System;
using UnityEngine;

public static class UIEvents
{
    public static event Action OnResaulMenu;
    public static void ShowResaulMenu() => OnResaulMenu?.Invoke();

    //Загрузочный экран
    public static event Func<Sequence> OnLoadingScreenAnimationStart;
    public static Sequence LoadingScreenAnimationStart() => OnLoadingScreenAnimationStart?.Invoke();

    public static event Func<Sequence> OnLoadingScreenAnimationEnd;
    public static Sequence LoadingScreenAnimationEnd() => OnLoadingScreenAnimationEnd?.Invoke();
}
