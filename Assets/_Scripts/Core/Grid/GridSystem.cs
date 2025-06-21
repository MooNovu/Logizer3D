using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GridSystem
{
    private GridCell[,] _grid;
    public int Width { get; private set; }
    public int Height { get; private set; }

    public const float CellSize = 1f;

    public GridSystem(int width, int height)
    {
        Initialize(width, height);
    }

    public void ReInitialize(int width, int height)
    {
        Initialize(width, height);
    }

    private void Initialize(int width, int height)
    {
        Width = width;
        Height = height;
        _grid = new GridCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _grid[x, y] = new GridCell(new Vector2Int(x, y));
            }
        }
    }

    public Vector3 GetWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.x * CellSize, 0, gridPosition.y * CellSize);
    }

    public bool TryGetGridPosition(Vector3 worldPosition, out Vector2Int gridPosition)
    {
        gridPosition = new Vector2Int(
            Mathf.FloorToInt(worldPosition.x / CellSize + 0.5f),
            Mathf.FloorToInt(worldPosition.z / CellSize + 0.5f)
        );
        return IsValidGridPosition(gridPosition);
    }
    public static Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        var gridPosition = new Vector2Int(
            Mathf.FloorToInt(worldPosition.x + 0.5f),
            Mathf.FloorToInt(worldPosition.z + 0.5f)
        );
        return gridPosition;
    }

    public GridCell GetCell(Vector2Int gridPosition)
    {
        if (IsValidGridPosition(gridPosition))
        {
            return _grid[gridPosition.x, gridPosition.y];
        }
        return null;
    }
    public GameObject GetElementAsGameObject(Vector2Int gridPosition, IGridElement element)
    {
        var cell = GetCell(gridPosition);
        if (cell == null) return null;
        if (cell.GetFirstElementOfType(element.Type) is MonoBehaviour monoBehaviour)
        {
            if (monoBehaviour != null)
            {
                return monoBehaviour.gameObject;
            }
        }
        return null;
    }
    public List<GameObject> GetElementsAsGameObjects(Vector2Int gridPosition)
    {
        var cell = GetCell(gridPosition);
        if (cell == null) return null;
        List<GameObject> gameObjects = new();
        foreach (IGridElement el in cell.Elements)
        {
            if (el is MonoBehaviour monoBehaviour)
            {
                if (monoBehaviour != null)
                {
                    gameObjects.Add(monoBehaviour.gameObject);
                }
            }
        }

        return gameObjects;
    }
    public GameObject GetFloorAsGameObject(Vector2Int gridPosition)
    {
        var cell = GetCell(gridPosition);
        if (cell?.Floor is MonoBehaviour monoBehaviour)
        {
            if (monoBehaviour != null)
            {
                return monoBehaviour.gameObject;
            }
        }
        return null;
    }
    public bool TryMoveElement(Vector2Int from, Vector2Int to, IGridElement element)
    {
        if (!IsValidGridPosition(to)) return false;

        if (GetElementAsGameObject(from, element) != null)
        {
            GetCell(from)?.RemoveElement(element);
            GetCell(to)?.AddElement(element);

            return true;
        }
        return false;

    }
    public bool IsCellWalkable(Vector2Int gridPosition, Vector2Int direction)
    {
        if (!IsValidGridPosition(gridPosition)) return false;
        var cell = _grid[gridPosition.x, gridPosition.y];
        if (cell?.Floor?.IsWalkable == null || cell?.Floor?.IsWalkable == false) return false;
        foreach (IGridElement el in cell?.Elements)
        {
            if (el.IsWalkable(direction) == false) return false;
        }
        return true;
    }

    public bool IsValidGridPosition(Vector2Int gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < Width &&
               gridPosition.y >= 0 && gridPosition.y < Height;
    }
}
