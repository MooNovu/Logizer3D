using System;
using UnityEngine;

public static class UIEvents
{
    public static Action OnResaulMenu;
    public static void ShowResaulMenu() => OnResaulMenu?.Invoke();

    //����������� �����
    public static event Action OnLoadingScreenAnimationStart;
    public static void LoadingScreenAnimationStart() => OnLoadingScreenAnimationStart?.Invoke();
}
