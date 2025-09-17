using System;
using UnityEngine;

public static class GameEvents
{
    public static int CandiesCollected { get; private set; }

    public static event Action OnPlayerPickCandy;
    public static event Action OnCandyCleared;
    public static void PlayerPickedCandy() 
    {
        OnPlayerPickCandy?.Invoke();
        CandiesCollected += 1;
    }
    public static void ClearCandies()
    {
        OnCandyCleared?.Invoke();
        CandiesCollected = 0;
    }
    public static int StepsDone { get; private set; }
    public static void AddStep() => StepsDone += 1;
    public static void ClearSteps() => StepsDone = 0;

    public static float TimeCounter { get; private set; }
    public static void SetCurrentTime() => TimeCounter = Time.time;
    public static float GetTimeDifference() => Time.time - TimeCounter;

    public static event Action OnPlayerReachedExit;
    public static event Action OnLevelNextLoad;
    public static event Action<LevelData> OnLevelLoad;
    public static event Action OnLevelReload;
    public static event Action OnLevelFullLoad;
    public static event Action<Vector3> OnPlayerMoved;
    public static void PlayerReachedExit() => OnPlayerReachedExit?.Invoke();
    public static void LoadNextLevel() => OnLevelNextLoad?.Invoke();
    public static void LoadLevel(LevelData lvl) => OnLevelLoad?.Invoke(lvl);
    public static void ReloadLevel() => OnLevelReload?.Invoke();
    public static void LevelFullLoaded() => OnLevelFullLoad?.Invoke();
    public static void PlayerMoved(Vector3 pos) => OnPlayerMoved?.Invoke(pos);


    //Editor
    public static event Action<LevelData> OnInitializeEditor;
    public static event Action<GridElementTypeSO> OnElementChange;
    public static event Action<GridFloorTypeSO> OnFloorChange;
    public static event Action<RedactorTool> OnRedactorToolChange;
    public static event Action<EditorSelectedType> OnSelectedTypeChange;

    public static void InitializeEditor(LevelData levelData) => OnInitializeEditor.Invoke(levelData);
    public static void ElementChangeTo(GridElementTypeSO element) => OnElementChange.Invoke(element);
    public static void FloorChangeTo(GridFloorTypeSO floor) => OnFloorChange.Invoke(floor);
    public static void RedactorModeChangeTo(RedactorTool tool) => OnRedactorToolChange.Invoke(tool);
    public static void RedactorSelectedTypeChangeTo(EditorSelectedType type) => OnSelectedTypeChange.Invoke(type);
}
