using Unity.VisualScripting;
using UnityEngine;

public class GridSystem
{
    private readonly GridCell[,] _grid;
    public int Width { get; }
    public int Height { get; }
    public float CellSize { get; }

    public GridSystem(int width, int height, float cellSize)
    {
        Width = width;
        Height = height;
        CellSize = cellSize;
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
    public bool AddElement(Vector2Int gridPosition, IGridElement element)
    {
        var cell = GetCell(gridPosition);
        if (cell == null || cell.Element != null)
        {
            return false;
        }
        cell.SetElement(element);
        return true;
    }
    public bool AddFloor(Vector2Int gridPosition, IFloor floor)
    {
        var cell = GetCell(gridPosition);
        if (cell == null || cell.Floor != null)
        {
            return false;
        }
        cell.SetFloor(floor);
        return true;
    }
    public bool IsCellWalkable(Vector2Int gridPosition)
    {
        if (!IsValidGridPosition(gridPosition)) return false;
        var cell = _grid[gridPosition.x, gridPosition.y];
        if (cell?.Floor?.IsWalkable == null || cell?.Floor?.IsWalkable == false) return false;
        if (cell?.Floor?.IsWalkable == null || cell?.Element?.IsWalkable == false) return false;
        return true; //Добавить проверку на то, можно ли сдвинуть блок перед - !cell.Element.Any() 
    }

    public bool IsValidGridPosition(Vector2Int gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < Width &&
               gridPosition.y >= 0 && gridPosition.y < Height;
    }
}
