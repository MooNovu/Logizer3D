using System.Linq;
using UnityEngine;

public class LevelInitializer
{
    private readonly GridSystem _gridSystem;

    public LevelInitializer(GridSystem gridSystem)
    {
        _gridSystem = gridSystem;
    }

    public void LoadLevelFromSceneObject()
    {
        IGridElement[] gridElements = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IGridElement>()
            .ToArray();

        foreach (var gridElement in gridElements)
        {
            _gridSystem.AddElement(gridElement.GridPosistion, gridElement);
        }

        IFloor[] gridFloors = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IFloor>()
            .ToArray();

        foreach (var gridFloor in gridFloors)
        {
            _gridSystem.AddFloor(gridFloor.GridPosistion, gridFloor);
        }
    }
}
