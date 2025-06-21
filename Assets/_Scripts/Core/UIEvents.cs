using System;
using UnityEngine;

public static class UIEvents
{
    public static Action OnResaulMenu;
    public static Action OnBlockUI;
    public static Action OnUnblockUI;
    public static void ShowResaulMenu() => OnResaulMenu?.Invoke();
    public static void BlockUI() => OnBlockUI?.Invoke();
    public static void UnblockUI() => OnUnblockUI?.Invoke();

}
