using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class LoadManager
{
    private readonly GridSystem _gridSystem;
    private readonly GridFactory _gridFactory;
    private readonly LevelAnimationHandler _animationHandler;
    private readonly IStateMachine _levelStateMachine;

    private readonly float _timeToSpawn = 1.0f;
    private readonly Vector3 _spawnOffset = Vector3.up * 10f;
    private readonly float _duration = 1.25f;
    private readonly Vector3 _playerInactivePosition = new(-1000, 0, -1000);

    private readonly GameObject _player;
    private Vector2Int _playerSpawn = new(0, 0);

    private List<GameObject> allObjects = new();
    private List<GameObject> staticObjects = new();

    private GameObject _combineObject;

    public LoadManager(
        GridSystem gridSystem,
        GridFactory gridFactory,
        LevelAnimationHandler animationHandler,
        IStateMachine levelStateMachine
        )
    {
        _gridSystem = gridSystem;
        _gridFactory = gridFactory;
        _animationHandler = animationHandler;
        _levelStateMachine = levelStateMachine;

        _player = _gridFactory.CreatePlayer(Skin.Default, _playerInactivePosition);
    }
    public void LoadLevel(LevelData levelData)
    {
        LoadFromSaveData(levelData);
    }
    private void LoadFromSaveData(LevelData data)
    {
        RemovePlayer();
        _gridSystem.ReInitialize(data.width, data.height);

        allObjects = new();
        staticObjects = new();

        foreach (CellData cellData in data.cells)
        {
            Vector2Int position = new(cellData.x, cellData.y);
            Vector3 worldPos = _gridSystem.GetWorldPosition(position);
            worldPos += _spawnOffset;

            if (cellData.floorType != FloorType.Abyss)
            {
                GameObject floorInstance = _gridFactory.CreateFloor(cellData.floorType, worldPos, cellData.floorRotation);
                IFloor floor = floorInstance.GetComponent<IFloor>();

                _gridSystem.GetCell(position).SetFloor(floor);
                if (floorInstance.TryGetComponent(out ISavable savableFloor))
                {
                    savableFloor.RestoreState(cellData.FloorState);
                }
                floorInstance.SetActive(false);
                allObjects.Add(floorInstance);
                if (IsStaticObject(floorInstance)) staticObjects.Add(floorInstance);
            }
            foreach (ElementData cellElementData in cellData.elements)
            {
                if (cellElementData.elementType != GridElementType.None)
                {
                    GameObject elementInstance = _gridFactory.CreateElement(cellElementData.elementType, worldPos, cellElementData.elementRotation);
                    IGridElement element = elementInstance.GetComponent<IGridElement>();

                    _gridSystem.GetCell(position).AddElement(element);
                    if (elementInstance.TryGetComponent(out ISavable savableElement))
                    {
                        savableElement.RestoreState(cellElementData.ElementState);
                    }

                    if (element.Type == GridElementType.Spawn) _playerSpawn = position;

                    elementInstance.SetActive(false);
                    allObjects.Add(elementInstance);
                    if (IsStaticObject(elementInstance)) staticObjects.Add(elementInstance);
                }
            }
        }
    }
    public void SpawnPlayer()
    {
        _player.transform.position = _gridSystem.GetWorldPosition(_playerSpawn);
        _gridSystem.GetCell(_playerSpawn)?.AddElement(_player.GetComponent<IGridElement>());
    }
    public void RemovePlayer()
    {
        Vector2Int pos = GridSystem.GetGridPosition(_player.transform.position);
        _gridSystem.GetCell(pos)?.RemoveElement(_player.GetComponent<IGridElement>());
        _player.transform.position = _playerInactivePosition;
    }
    public void OptimizeStaticObjects()
    {
        GameObject combineInstance = _gridFactory.CreateCombineObject(staticObjects);
        if (combineInstance != null)
        {
            foreach (var obj in staticObjects) obj.SetActive(false);
        }
        Resources.UnloadUnusedAssets();
        if (_combineObject != null) GameObject.Destroy(_combineObject);
        _combineObject = combineInstance;
    }
    private bool IsStaticObject(GameObject obj)
    {
        if (obj.TryGetComponent(out ISavable _) ||
            obj.TryGetComponent(out IInteractable _) ||
            obj.TryGetComponent(out ISteppable _)
            )
        {
            return false;
        }
        return true;
    }
    public IEnumerator SpawnAnimation()
    {

        float delay = _timeToSpawn / allObjects.Count;
        foreach (GameObject obj in allObjects)
        {
            obj.SetActive(true);

            Vector3 startPos = obj.transform.position;
            _gridSystem.TryGetGridPosition(startPos - _spawnOffset, out Vector2Int tempPos);
            Vector3 targetPos = _gridSystem.GetWorldPosition(tempPos);

            obj.transform.DOMove(targetPos, _duration).SetEase(Ease.OutBounce);
            obj.transform.DOScale(Vector3.one, _duration).From(Vector3.zero).SetEase(Ease.OutBounce);

            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(_duration);
    }
    public IEnumerator ClearLevel()
    {
        float delay = _timeToSpawn / allObjects.Count;
        GameObject.Destroy(_combineObject);
        _combineObject = null;
        foreach (var obj in staticObjects) obj.SetActive(true);

        yield return null;
        foreach (GameObject obj in allObjects)
        {
            obj.transform.DOScale(Vector3.zero, _duration).SetEase(Ease.OutQuart);

            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(_duration);
        foreach (var obj in allObjects) GameObject.Destroy(obj);
    }
}
