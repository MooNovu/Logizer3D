using NUnit.Framework;
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
public class GameProgress
{
    public DictionaryWrapper CompletedLevels = new();
}

[System.Serializable]
public class DictionaryWrapper
{
    public List<int> keys = new();
    public List<int> values = new();

    public Dictionary<int, int> ToDictionary()
    {
        Dictionary<int, int> dict = new();
        for (int i = 0; i < keys.Count; i++)
        {
            dict[keys[i]] = values[i];
        }
        return dict;
    }
    public void FromDictionary(Dictionary<int, int> dict)
    {
        keys.Clear();
        values.Clear();
        foreach (var pair in dict)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }
}