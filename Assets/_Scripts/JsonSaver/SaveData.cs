using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ElementState { }

[System.Serializable]
public class SaveData
{
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