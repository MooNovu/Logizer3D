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
        Dictionary<Material, List<CombineInstance>> materialGroups = new();

        foreach (var obj in staticObjects)
        {
            MeshFilter meshFilter = obj.GetComponentInChildren<MeshFilter>();
            MeshRenderer meshRenderer = obj.GetComponentInChildren<MeshRenderer>();

            if (meshFilter == null || meshRenderer == null) continue;

            Mesh mesh = meshFilter.sharedMesh;
            Material[] materials = meshRenderer.sharedMaterials;

            for (int subMeshIndex = 0; subMeshIndex < mesh.subMeshCount; subMeshIndex++)
            {
                Material material = materials[subMeshIndex % materials.Length];

                if (!materialGroups.ContainsKey(material))
                    materialGroups[material] = new List<CombineInstance>();

                materialGroups[material].Add(new CombineInstance
                {
                    mesh = mesh,
                    subMeshIndex = subMeshIndex,
                    transform = meshFilter.transform.localToWorldMatrix
                });
            }
        }

        if (materialGroups.Count == 0) return null;

        List<CombineInstance> finalCombines = new();
        List<Material> finalMaterials = new(materialGroups.Keys);

        foreach (var material in finalMaterials)
        {
            Mesh groupedMesh = new();
            groupedMesh.CombineMeshes(
                materialGroups[material].ToArray(),
                true,
                true
            );
            finalCombines.Add(new CombineInstance
            {
                mesh = groupedMesh,
                subMeshIndex = 0,
                transform = Matrix4x4.identity
            });
        }

        Mesh combinedMesh = new();
        combinedMesh.CombineMeshes(
            finalCombines.ToArray(),
            false,
            false
        );

        GameObject combinedObject = new("CombinedObject");
        combinedObject.AddComponent<MeshFilter>().mesh = combinedMesh;
        combinedObject.AddComponent<MeshRenderer>().sharedMaterials = finalMaterials.ToArray();
        combinedObject.transform.SetParent(_floorParent);

        return combinedObject;
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
