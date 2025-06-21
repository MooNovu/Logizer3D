using System.Linq;
using UnityEngine;

public class FromSceneLoader
{
    private readonly GridSystem _gridSystem;

    public FromSceneLoader(GridSystem gridSystem)
    {
        _gridSystem = gridSystem;
    }

    public void LoadLevelFromSceneObject()
    {
        IGridElement[] gridElements = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IGridElement>()
            .ToArray();

        foreach (IGridElement gridElement in gridElements)
        {
            _gridSystem.GetCell(gridElement.GridPosistion).AddElement(gridElement);
        }

        IFloor[] gridFloors = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IFloor>()
            .ToArray();

        foreach (var gridFloor in gridFloors)
        {
            _gridSystem.GetCell(gridFloor.GridPosistion).SetFloor(gridFloor);
        }
    }
}
