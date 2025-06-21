using JetBrains.Annotations;
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
    public List<ElementData> elements = new();
}

[System.Serializable]
public class ElementData 
{
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