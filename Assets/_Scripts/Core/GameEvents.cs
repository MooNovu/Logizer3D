using System;
using System.Collections;
using UnityEngine;

public static class GameEvents
{
    public static Action OnPlayerReachedExit;
    public static Action OnLevelLoad;
    public static Action OnLevelReload;
    public static Action<Vector3> OnPlayerMoved;
    public static void PlayerReachedExit() => OnPlayerReachedExit?.Invoke();
    public static void LoadLevel() => OnLevelLoad?.Invoke();
    public static void ReloadLevel() => OnLevelReload?.Invoke();
    public static void PlayerMoved(Vector3 pos) => OnPlayerMoved?.Invoke(pos);

}
