using UnityEngine;
using System.IO;

public class SaveLoadManager
{
    private readonly GridManager _gridManager;
    private readonly GridSystem _gridSystem;
    private readonly GridFactory _gridFactory;
    private readonly string fileName = "_output.json";
    public SaveLoadManager(GridSystem gridSystem, GridFactory gridFactory, GridManager gridManager)
    {
        _gridSystem = gridSystem;
        _gridFactory = gridFactory;
        _gridManager = gridManager;
    }
    public void SaveLevel()
    {
        LevelData data = CreateSaveData();
        string json = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, json);
        Debug.Log($"Level saved to {path}");
    }

    public void LoadLevel(LevelData levelData)
    {
        LoadFromSaveData(levelData);
        Debug.Log($"Level loaded");
    }

    public LevelData CreateSaveData()
    {
        LevelData data = new()
        {
            width = _gridSystem.Width,
            height = _gridSystem.Height,
            cellSize = _gridSystem.CellSize
        };

        for (int x = 0; x < _gridSystem.Width; x++)
        {
            for (int y = 0; y < _gridSystem.Height; y++)
            {
                var cell = _gridSystem.GetCell(new Vector2Int(x, y));
                if (cell.Element != null || cell.Floor != null)
                {
                    data.cells.Add(new CellData
                    {
                        x = x,
                        y = y,
                        floorRotation = cell.Floor?.Rotation ?? 0,
                        floorType = cell.Floor?.Type ?? FloorType.Abyss,
                        FloorState = cell.Floor is ISavable savableFloor ? savableFloor.CaptureState() : null,
                        elementRotation = cell.Element?.Rotation ?? 0,
                        elementType = cell.Element?.Type ?? GridElementType.None,
                        ElementState = cell.Element is ISavable savableElement ? savableElement.CaptureState() : null
                    });
                }
            }
        }
        return data;
    }

    public void LoadFromSaveData(LevelData data)
    {
        _gridSystem.ReInitialize(data.width, data.height, data.cellSize);
        _gridManager.ResizeGrid(data.width, data.height, data.cellSize);

        foreach (var cellData in data.cells)
        {
            Vector2Int position = new(cellData.x, cellData.y);
            Vector3 worldPos = _gridSystem.GetWorldPosition(position);

            if (cellData.floorType != FloorType.Abyss)
            {
                var floorInstance = _gridFactory.CreateFloor(cellData.floorType, worldPos, cellData.floorRotation);
                var floor = floorInstance.GetComponent<IFloor>();

                _gridSystem.GetCell(position).SetFloor(floor);
                if (floorInstance.TryGetComponent(out ISavable savableFloor))
                {
                    savableFloor.RestoreState(cellData.FloorState);
                }
            }

            if (cellData.elementType != GridElementType.None)
            {
                var elementInstance = _gridFactory.CreateElement(cellData.elementType, worldPos, cellData.elementRotation);
                var element = elementInstance.GetComponent<IGridElement>();

                _gridSystem.GetCell(position).SetElement(element);
                if (elementInstance.TryGetComponent(out ISavable savableElement))
                {
                    savableElement.RestoreState(cellData.ElementState);
                }
            }

        }

    }
}
