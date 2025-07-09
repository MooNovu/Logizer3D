using System;
using System.Collections;
using UnityEngine;

public static class GameEvents
{
    public static Action OnPlayerReachedExit;
    public static Action OnLevelNextLoad;
    public static Action<LevelData> OnLevelLoad;
    public static Action OnLevelReload;
    public static Action<Vector3> OnPlayerMoved;
    public static void PlayerReachedExit() => OnPlayerReachedExit?.Invoke();
    public static void LoadNextLevel() => OnLevelNextLoad?.Invoke();
    public static void LoadLevel(LevelData lvl) => OnLevelLoad?.Invoke(lvl);
    public static void ReloadLevel() => OnLevelReload?.Invoke();
    public static void PlayerMoved(Vector3 pos) => OnPlayerMoved?.Invoke(pos);


    //Editor
    public static Action<LevelData> OnInitializeEditor;
    public static void InitializeEditor(LevelData levelData) => OnInitializeEditor.Invoke(levelData);

    public static Action<GridElementTypeSO> OnElementChange;
    public static void ElementChangeTo(GridElementTypeSO element) => OnElementChange.Invoke(element);

    public static Action<GridFloorTypeSO> OnFloorChange;
    public static void FloorChangeTo(GridFloorTypeSO floor) => OnFloorChange.Invoke(floor);
}
