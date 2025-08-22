using System;
using UnityEngine;

public static class UIEvents
{
    public static event Action OnResaulMenu;
    public static void ShowResaulMenu() => OnResaulMenu?.Invoke();

    //Загрузочный экран
    public static event Action OnLoadingScreenAnimationStart;
    public static void LoadingScreenAnimationStart() => OnLoadingScreenAnimationStart?.Invoke();

    public static event Action OnLoadingScreenAnimationEnd;
    public static void LoadingScreenAnimationEnd() => OnLoadingScreenAnimationEnd?.Invoke();
}
