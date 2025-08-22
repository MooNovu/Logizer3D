using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveFileManager
{
    public readonly static string UserLevelPath = Path.Combine(Application.persistentDataPath, "CustomLevels");
    public static void SaveLevel(GridSystem gridSystem, string levelName)
    {
        if (!Directory.Exists(UserLevelPath))
        {
            Directory.CreateDirectory(UserLevelPath);
        }
        LevelData data = CreateSaveData(gridSystem, levelName);
        string json = JsonUtility.ToJson(data, true);

        string fileName = $"{levelName}.json";
        string path = Path.Combine(UserLevelPath, fileName);

        File.WriteAllText(path, json);
        Debug.Log($"Level saved to {path}");
    }
    public static void DeleteLevel(string levelName)
    {
        string fileName = $"{levelName}.json";
        string path = Path.Combine(UserLevelPath, fileName);
        File.Delete(path);
    }

    private static LevelData CreateSaveData(GridSystem gridSystem, string levelName)
    {
        LevelData data = new()
        {
            Name = levelName,
            width = gridSystem.Width,
            height = gridSystem.Height
        };

        for (int x = 0; x < gridSystem.Width; x++)
        {
            for (int y = 0; y < gridSystem.Height; y++)
            {
                var cell = gridSystem.GetCell(new Vector2Int(x, y));
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
    private static List<ElementData> ListElements(GridCell cell)
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
