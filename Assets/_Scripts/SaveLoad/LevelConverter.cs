using System.Linq;
using UnityEngine;

public class LevelConverter
{
    private readonly GridSystem _gridSystem;

    public LevelConverter(int width, int height)
    {
        _gridSystem = new(width, height);
    }

    public GridSystem LoadLevelFromSceneObject()
    {
        IGridElement[] gridElements = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IGridElement>()
            .ToArray();

        foreach (IGridElement gridElement in gridElements)
        {
            _gridSystem.GetCell(gridElement.GridPosition).AddElement(gridElement);
        }

        IFloor[] gridFloors = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IFloor>()
            .ToArray();

        foreach (var gridFloor in gridFloors)
        {
            _gridSystem.GetCell(gridFloor.GridPosistion).SetFloor(gridFloor);
        }
        return _gridSystem;
    }
}
