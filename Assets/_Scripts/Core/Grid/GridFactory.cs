using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridFactory
{
    private readonly Dictionary<GridElementType, GridElementTypeSO> _elementPrefabMap;
    private readonly Dictionary<FloorType, GridFloorTypeSO> _floorPrefabMap;
    private readonly Transform _elementsParent;
    private readonly Transform _floorParent;
    private readonly MoverFactory _moverFactory;

    public GridFactory(
        GridElementTypeSO[] elementConfigs, 
        GridFloorTypeSO[] floorConfigs, 
        Transform elementsParent, 
        Transform floorParent,
        MoverFactory moverFactory
        )
    {
        _elementPrefabMap = elementConfigs.ToDictionary(x => x.Type, x => x);
        _floorPrefabMap = floorConfigs.ToDictionary(x => x.Type, x => x);
        _elementsParent = elementsParent;
        _floorParent = floorParent;
        _moverFactory = moverFactory;
    }

    public GameObject CreateElement(GridElementType type, Vector3 position, int rotation)
    {
        var prefab = GetPrefabForGridElement(type);
        if (prefab == null) return null;

        GameObject instance;

        if (prefab.GetComponent<Mover>() != null) instance = _moverFactory.Create(prefab).gameObject;
        else instance = Object.Instantiate(prefab, position, Quaternion.Euler(0, rotation * 90, 0), _elementsParent);

        instance.transform.SetPositionAndRotation(position, Quaternion.Euler(0, rotation * 90, 0));
        instance.transform.parent = _elementsParent;
        return instance;
    }

    public GameObject CreateFloor(FloorType type, Vector3 position, int rotation)
    {
        var prefab = GetPrefabForFloor(type);
        if (prefab == null) return null;
        var instance = Object.Instantiate(prefab, position, Quaternion.Euler(0, rotation * 90, 0), _floorParent);
        return instance;
    }


    public GameObject GetPrefabForGridElement(GridElementType type)
    {
        if (type == GridElementType.None) return null;
        if (_elementPrefabMap.TryGetValue(type, out var elementType))
        {
            return elementType.Prefab;
        }
        throw new KeyNotFoundException($"No prefab found for element type {type}");
    }
    public GameObject GetPrefabForFloor(FloorType type)
    {
        if (type == FloorType.Abyss) return null;
        if (_floorPrefabMap.TryGetValue(type, out var floorType))
        {
            return floorType.Prefab;
        }
        throw new KeyNotFoundException($"No prefab found for floor type {type}");
    }
}
