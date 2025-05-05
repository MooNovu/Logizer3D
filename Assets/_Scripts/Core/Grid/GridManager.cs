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
    //Ссылки на обьектов-родитей для размещения
    [SerializeField] private Transform elementsParent;
    [SerializeField] private Transform floorParent;
    public Transform ElementsParent { get { return elementsParent; } }
    public Transform FloorParent { get { return floorParent; } }
    //Сетка
    private int _width = 10;
    private int _height = 10;
    private float _cellSize = 1f;
    public int Width { get { return _width; } }
    public int Height { get { return _height; } }
    public float CellSize { get { return _cellSize; } }

    [Inject] private GridSystem _gridSystem;
    [Inject] private GridFactory _gridFactory;
    //[Inject] private LevelInitializer _levelInitializer;

    private void Awake()
    {
        if (elementsParent == null) Debug.LogError("Не установлен elementParent");
        if (floorParent == null) Debug.LogError("Не установлен floorParent");

        //_levelInitializer.LoadLevelFromSceneObject();
    }

    #region Работа с GridSystem
    public bool TryPlaceElement(Vector3 worldPosition, IGridElement element)
    {
        if (_gridSystem.TryGetGridPosition(worldPosition, out var gridPosition))
        {
            if (_gridSystem.AddElement(gridPosition, element))
            {
                if (element is MonoBehaviour monoBehaviour)
                {
                    monoBehaviour.transform.position = _gridSystem.GetWorldPosition(gridPosition);
                }
                return true;
            }
        }
        return false;
    }
    public bool TryPlaceFloor(Vector3 worldPosition, IFloor floor)
    {
        if (_gridSystem.TryGetGridPosition(worldPosition, out var gridPosition))
        {
            if (_gridSystem.AddFloor(gridPosition, floor))
            {
                if (floor is MonoBehaviour monoBehaviour)
                {
                    monoBehaviour.transform.position = _gridSystem.GetWorldPosition(gridPosition);
                }
                return true;
            }
        }
        return false;
    }
    public void ResizeGrid(int width, int height, float cellSize)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
    }
    #endregion
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_gridSystem == null) return;

        for (int x = 0; x < _gridSystem.Width; x++)
        {
            for (int y = 0; y < _gridSystem.Height; y++)
            {
                var pos = _gridSystem.GetWorldPosition(new Vector2Int(x, y));
                Gizmos.DrawWireCube(pos + Vector3.up * 0.01f, 
                    new Vector3(_cellSize, 0.01f,_cellSize));
            }
        }
    }
#endif
}
