using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class GridFactory
{
    private readonly DiContainer _container;
    private readonly Dictionary<GridElementType, GridElementTypeSO> _elementPrefabMap;
    private readonly Dictionary<FloorType, GridFloorTypeSO> _floorPrefabMap;
    private readonly Dictionary<Skin, PlayerSkinsSO> _playerPrefabMap;
    private readonly Transform _elementsParent;
    private readonly Transform _floorParent;

    public GridFactory(
        DiContainer container,
        GridElementsContainer elementsContainer,
        GridFloorsContainer floorsContainer,
        PlayerSkinsContainer skinsContainer,
        Transform elementsParent, 
        Transform floorParent
        )
    {
        _container = container;
        _elementPrefabMap = elementsContainer.AllElements.ToDictionary(x => x.Type, x => x);
        _floorPrefabMap = floorsContainer.AllFloors.ToDictionary(x => x.Type, x => x);
        _playerPrefabMap = skinsContainer.AllSkins.ToDictionary(x => x.Skin, x => x);
        _elementsParent = elementsParent;
        _floorParent = floorParent;
    }

    public GameObject CreateElement(GridElementType type, Vector3 position, int rotation)
    {
        GameObject prefab = GetPrefabForGridElement(type);
        if (prefab == null) return null;

        GameObject instance = _container.InstantiatePrefab(prefab, position, Quaternion.Euler(0, rotation * 90, 0), _elementsParent);

        return instance;
    }

    public GameObject CreateFloor(FloorType type, Vector3 position, int rotation)
    {
        GameObject prefab = GetPrefabForFloor(type);
        if (prefab == null) return null;

        GameObject instance = _container.InstantiatePrefab(prefab, position, Quaternion.Euler(0, rotation * 90, 0), _floorParent);
        return instance;
    }

    public GameObject CreatePlayer(Skin skin, Vector3 position)
    {
        GameObject prefab = GetPrefabForPlayer(skin);
        if (prefab == null) return null;

        GameObject instance = _container.InstantiatePrefab(prefab, position, Quaternion.Euler(0, 0, 0), _elementsParent);
        return instance;
    }

    public GameObject CreateCombineObject(List<GameObject> staticObjects)
    {
        List<MeshFilter> meshFilters = new();
        List<Material> materials = new();

        foreach (var obj in staticObjects)
        {
            var filter = obj.GetComponentInChildren<MeshFilter>();
            if (filter != null)
            {
                meshFilters.Add(filter);
                materials.Add(obj.GetComponentInChildren<MeshRenderer>().material);
            }
        }

        if (meshFilters.Count > 0)
        {
            CombineInstance[] combine = new CombineInstance[meshFilters.Count];

            for (int i = 0; i < meshFilters.Count; i++)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            }

            Mesh combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(combine);

            GameObject combinedObject = new GameObject("CombineObject");
            combinedObject.AddComponent<MeshFilter>().mesh = combinedMesh;
            var renderer = combinedObject.AddComponent<MeshRenderer>();

            renderer.material = materials[0];

            combinedObject.AddComponent<MeshCollider>();
            combinedObject.transform.SetParent(_floorParent);
            return combinedObject;
        }
        return null;
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
    public GameObject GetPrefabForPlayer(Skin skin)
    {
        if (_playerPrefabMap.TryGetValue(skin, out var skinVer))
        {
            return skinVer.Prefab;
        }
        throw new KeyNotFoundException($"No prefab found for floor type {skin}");
    }
}
