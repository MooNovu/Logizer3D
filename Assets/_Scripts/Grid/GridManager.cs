using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using Zenject;

[ExecuteInEditMode]
public class GridManager : MonoBehaviour
{
    //Родители
    [SerializeField] private Transform elementsParent;
    [SerializeField] private Transform floorParent;
    public Transform ElementsParent { get { return elementsParent; } }
    public Transform FloorParent { get { return floorParent; } }

    //Сетка
    [SerializeField] private int _width = 10;
    [SerializeField] private int _height = 10;
    private readonly float _cellSize = 1f;
    //Префабы
    [SerializeField] private GridElementTypeSO[] _elementConfigs;
    [SerializeField] private GridFloorTypeSO[] _floorConfigs;
    private Dictionary<GridElementType, GridElementTypeSO> elementPrefabMap;
    private Dictionary<FloorType, GridFloorTypeSO> floorPrefabMap;

    public static GridSystem GridSystem { get; private set; }

    private void Awake()
    {
        InitializeGrid();
        LoadLevelFromSceneObject();
        elementPrefabMap = _elementConfigs.ToDictionary(x => x.Type, x => x);
        floorPrefabMap = _floorConfigs.ToDictionary(x => x.Type, x => x);

        if (elementsParent == null) Debug.LogError("Не установлен elementParent");
        if (floorParent == null) Debug.LogError("Не установлен floorParent");
    }

    #region Сохранения и тп
    public void InitializeGrid()
    {
        GridSystem = new GridSystem(_width, _height, _cellSize);
    }
    public void LoadLevelFromSceneObject()
    {
        IGridElement[] gridElements = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IGridElement>().ToArray();
        foreach (var gridElement in gridElements)
        {
            //Debug.Log($"{gridElement.Type} - {gridElement.GridPosistion}");
            GridSystem.AddElement(gridElement.GridPosistion, gridElement);
        }
        IFloor[] gridFloors = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IFloor>().ToArray();
        foreach (var gridFloor in gridFloors)
        {
            //Debug.Log($"{gridFloor.Type} - {gridFloor.GridPosistion}");
            GridSystem.AddFloor(gridFloor.GridPosistion, gridFloor);
        }
    }
    public LevelData CreateSaveData()
    {
        LevelData data = new() 
        {
            width = _width, 
            height = _height,
            cellSize = _cellSize
        };

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var cell = GridSystem.GetCell(new Vector2Int(x, y));
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
        GridSystem = new GridSystem(data.width, data.height, data.cellSize);

        foreach (var cellData in data.cells)
        {
            //Солздаем игровой экземпляр Пола
            GameObject floorPrefab = GetPrefabForFloor(cellData.floorType);
            if (floorPrefab != null)
            {
                //GameObject floorInstance = PrefabUtility.InstantiatePrefab(floorPrefab) as GameObject;
                GameObject floorInstance = Instantiate(floorPrefab);
                Vector3 floorWorldPos = GridSystem.GetWorldPosition(new Vector2Int(cellData.x, cellData.y));
                floorInstance.transform.position = floorWorldPos;
                Vector3 floorRotation = transform.eulerAngles;
                floorRotation.y = cellData.floorRotation * 90;
                floorInstance.transform.rotation = Quaternion.Euler(floorRotation);
                floorInstance.transform.SetParent(floorParent, true);

                GridSystem.GetCell(new Vector2Int(cellData.x, cellData.y)).SetFloor(floorInstance.GetComponent<IFloor>());
                if (floorInstance.TryGetComponent(out ISavable savableFloor))
                {
                    savableFloor.RestoreState(cellData.FloorState);
                }
            }
            //Солздаем игровой экземпляр Элемента
            GameObject elementPrefab = GetPrefabForGridElement(cellData.elementType);
            if (elementPrefab != null)
            {
                //GameObject elementInstance = PrefabUtility.InstantiatePrefab(elementPrefab) as GameObject;
                GameObject elementInstance = Instantiate(elementPrefab);
                Vector3 elementWorldPos = GridSystem.GetWorldPosition(new Vector2Int(cellData.x, cellData.y));
                elementInstance.transform.position = elementWorldPos;
                Vector3 elementRotation = transform.eulerAngles;
                elementRotation.y = cellData.elementRotation * 90;
                elementInstance.transform.rotation = Quaternion.Euler(elementRotation);
                elementInstance.transform.SetParent(elementsParent, true);

                GridSystem.GetCell(new Vector2Int(cellData.x, cellData.y)).SetElement(elementInstance.GetComponent<IGridElement>());
                if (elementInstance.TryGetComponent(out ISavable savableElement))
                {
                    savableElement.RestoreState(cellData.ElementState);
                }
            }

        }

    }
#endregion

    public GameObject GetPrefabForGridElement(GridElementType type)
    {
        if (type == GridElementType.None) return null;
        if (elementPrefabMap.TryGetValue(type, out var elementType))
        {
            return elementType.Prefab;
        }
        Debug.LogError($"Нет префаба элемента для {type}");
        return null;
    }
    public GameObject GetPrefabForFloor(FloorType type)
    {
        if (type == FloorType.Abyss) return null;
        if (floorPrefabMap.TryGetValue(type, out var floorType))
        {
            return floorType.Prefab;
        }
        Debug.LogError($"Нет префаба пола для {type}");
        return null;
    }


    #region Работа с GridSystem
    public bool TryPlaceElement(Vector3 worldPosition, IGridElement element)
    {
        if (GridSystem.TryGetGridPosition(worldPosition, out var gridPosition))
        {
            if (GridSystem.AddElement(gridPosition, element))
            {
                if (element is MonoBehaviour monoBehaviour)
                {
                    monoBehaviour.transform.position = GridSystem.GetWorldPosition(gridPosition);
                }
                return true;
            }
        }
        return false;
    }
    public bool TryPlaceFloor(Vector3 worldPosition, IFloor floor)
    {
        if (GridSystem.TryGetGridPosition(worldPosition, out var gridPosition))
        {
            if (GridSystem.AddFloor(gridPosition, floor))
            {
                if (floor is MonoBehaviour monoBehaviour)
                {
                    monoBehaviour.transform.position = GridSystem.GetWorldPosition(gridPosition);
                }
                return true;
            }
        }
        return false;
    }

    #endregion
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (GridSystem == null) return;

        for (int x = 0; x < GridSystem.Width; x++)
        {
            for (int y = 0; y < GridSystem.Height; y++)
            {
                var pos = GridSystem.GetWorldPosition(new Vector2Int(x, y));
                Gizmos.DrawWireCube(pos + Vector3.up * 0.01f, 
                    new Vector3(_cellSize, 0.01f,_cellSize));
            }
        }
    }
#endif
}
