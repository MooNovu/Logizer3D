using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ElementState { }

[System.Serializable]
public class LevelData
{
    public string Name;
    public int width;
    public int height;
    public float cellSize;
    public List<CellData> cells = new();
}

[System.Serializable]
public class CellData
{
    public int x;
    public int y;
    public int floorRotation;
    public FloorType floorType;
    [SerializeReference] public ElementState FloorState;
    public int elementRotation;
    public GridElementType elementType;
    [SerializeReference] public ElementState ElementState;
}

[System.Serializable]
public class StageData
{
    public string stageName;
    public List<LevelData> levels = new();
}

public static class LevelDataHolder
{
    public static LevelData SelectedLevel { get; private set; }
    public static string CurrentStage { get; private set; }

    public static void SetLevel(LevelData level)
    {
        SelectedLevel = level;
    }
    public static void ClearLevel()
    {
        SelectedLevel = null;
    }
    public static void SetStage(string stage)
    {
        CurrentStage = stage;
    }
    public static void ClearStage()
    {
        CurrentStage = null;
    }
}