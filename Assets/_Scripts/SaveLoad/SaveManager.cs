using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager
{
    private readonly string fileName = "_output.json";

    private readonly GridSystem _gridSystem;
    public SaveManager(GridSystem gridSystem)
    {
        _gridSystem = gridSystem;
    }
    public void SaveLevel()
    {
        LevelData data = CreateSaveData();
        string json = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, json);
        Debug.Log($"Level saved to {path}");
    }

    private LevelData CreateSaveData()
    {
        LevelData data = new()
        {
            Name = "Name",
            width = _gridSystem.Width,
            height = _gridSystem.Height
        };

        for (int x = 0; x < _gridSystem.Width; x++)
        {
            for (int y = 0; y < _gridSystem.Height; y++)
            {
                var cell = _gridSystem.GetCell(new Vector2Int(x, y));
                if (cell.Elements.Count > 0 || cell.Floor != null)
                {
                    data.cells.Add(new CellData
                    {
                        x = x,
                        y = y,
                        floorRotation = cell.Floor?.Rotation ?? 0,
                        floorType = cell.Floor?.Type ?? FloorType.Abyss,
                        FloorState = cell.Floor is ISavable savableFloor ? savableFloor.CaptureState() : null,
                        elements = ListElements(cell)
                    });
                }
            }
        }
        return data;
    }
    private List<ElementData> ListElements(GridCell cell)
    {
        List<ElementData> elem = new();
        foreach (IGridElement element in cell.Elements)
        {
            if (element.Type == GridElementType.None) continue;
            ElementState state = element is ISavable savableElement ? savableElement.CaptureState() : null;
            elem.Add(new ElementData
            {
                elementRotation = element.Rotation,
                elementType = element.Type,
                ElementState = state
            });
        }
        return elem;
    }
}
