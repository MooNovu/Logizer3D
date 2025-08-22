using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    [Header("DI")]
    [SerializeField] private Transform _gridRenderer;
    [SerializeField] private Transform _floorParent;
    [SerializeField] private Transform _elementParent;
    [SerializeField] private Camera _sceneCamera;

    [SerializeField] private EditorSettingsPanel _editorSettings;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private List<GraphicRaycaster> _graphicRaycasters;

    [Header("Data")]
    [SerializeField] private GridElementsContainer _elementContainer;
    [SerializeField] private GridFloorsContainer _floorContainer;

    private GridElementTypeSO _selectedElement;
    private GridFloorTypeSO _selectedFloor;

    private EditorSelectedType _selectedType = EditorSelectedType.Floors;
    private RedactorTool _redactorTool = RedactorTool.Create;
    private Dictionary<RedactorTool, Action<Vector2Int>> _toolActions;

    private EditorGrid _editorGrid;
    private EditorInput _editorInput;
    private Vector2 _pointerPos;

    private void Awake()
    {
        _toolActions = new()
        {
            [RedactorTool.Create] = Create,
            [RedactorTool.Edit] = Edit,
            [RedactorTool.Delete] = Delete,
            [RedactorTool.Rotate] = Rotate
        };
        Subscribe();
    }
    private void OnDisable()
    {
        Unsubscribe();
    }
    #region Îáðàáîò÷èêè & Ïîäïèñêè íà ñîáûòèÿ
    private void Subscribe()
    {
        GameEvents.OnInitializeEditor += InitializeGrid;
        _editorInput = new();
        _editorInput.Editor.Click.performed += PointerTools;
        _editorInput.Enable();

        GameEvents.OnElementChange += HandleElementChange;
        GameEvents.OnFloorChange += HandleFloorChange;
        GameEvents.OnRedactorToolChange += HandleRedactorModeChange;
        GameEvents.OnSelectedTypeChange += HandleSelectedTypeChange;
    }
    private void Unsubscribe()
    {
        GameEvents.OnInitializeEditor -= InitializeGrid;
        _editorInput.Editor.Click.performed -= PointerTools;
        _editorInput.Disable();

        GameEvents.OnElementChange -= HandleElementChange;
        GameEvents.OnFloorChange -= HandleFloorChange;
        GameEvents.OnRedactorToolChange -= HandleRedactorModeChange;
        GameEvents.OnSelectedTypeChange -= HandleSelectedTypeChange;
    }
    private void HandleElementChange(GridElementTypeSO el) => _selectedElement = el;
    private void HandleFloorChange(GridFloorTypeSO fl) => _selectedFloor = fl;
    private void HandleRedactorModeChange(RedactorTool tool) => _redactorTool = tool;
    private void HandleSelectedTypeChange(EditorSelectedType type) => _selectedType = type;
    #endregion
    public void InitializeGrid(LevelData levelData)
    {
        _editorGrid?.ClearAll();

        IReadOnlyDictionary<GridElementType, GridElementTypeSO> elementTypeToSO = _elementContainer.AllElements.ToDictionary(x => x.Type);
        IReadOnlyDictionary<FloorType, GridFloorTypeSO> floorTypeToSO = _floorContainer.AllFloors.ToDictionary(x => x.Type);

        _editorGrid = new(levelData, _elementParent, _floorParent, elementTypeToSO, floorTypeToSO);
        ÑonfigureGridRendrer(levelData.width, levelData.height);
    }
    private void ÑonfigureGridRendrer(int width, int height)
    {
        float offsetW = width % 2 == 0 ? 0.5f : 0f;
        float offsetH = height % 2 == 0 ? 0.5f : 0f;
        _gridRenderer.position = new(width / 2 - offsetW, 2, height / 2 - offsetH);
        _gridRenderer.localScale = new(width, height, 1);
    }

    private void PointerTools(InputAction.CallbackContext ctx)
    {
        if (IsPointerOverUI()) return;
        _pointerPos = Pointer.current.position.ReadValue();
        Ray ray = _sceneCamera.ScreenPointToRay(_pointerPos);
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            Vector2Int cellPos = GridSystem.GetGridPosition(hit.point);

            _toolActions[_redactorTool](cellPos);
        }
    }
    private void Create(Vector2Int cellPos)
    {
        if (_selectedType == EditorSelectedType.Floors)
        {
            if (_selectedFloor == null) return;
            _editorGrid.FloorGrid[cellPos.x, cellPos.y].SetFloor(_selectedFloor);
            return;
        }

        if (_selectedType == EditorSelectedType.Elements)
        {
            if (_selectedElement == null) return;
            _editorGrid.ElementGrid[cellPos.x, cellPos.y].AddElement(_selectedElement);
            return;
        }

    }
    private void Edit(Vector2Int cellPos)
    {
        if (_selectedType == EditorSelectedType.Floors)
        {
            _editorSettings.OpenSettings(_editorGrid.GetInteractableGridFloor(cellPos));
            return;
        }
        if (_selectedType == EditorSelectedType.Elements)
        {
            List<IGridElement> elements = _editorGrid.GetInteractableGridElements(cellPos);
            if (elements == null || elements?.Count < 1) return;
            _editorSettings.OpenSettings(elements[0]);
            return;
        }
    }
    private void Delete(Vector2Int cellPos)
    {
        if (_selectedType == EditorSelectedType.Floors)
        {
            _editorGrid.DeleteFloorFrom(cellPos);
            return;
        }
        if (_selectedType == EditorSelectedType.Elements)
        {
            _editorGrid.DeleteElementsFrom(cellPos);
            return;
        }
    }
    private void Rotate(Vector2Int cellPos)
    {
        if (_selectedType == EditorSelectedType.Floors)
        {
            _editorGrid.FloorGrid[cellPos.x, cellPos.y].Rotate();
            return;
        }
        if (_selectedType == EditorSelectedType.Elements)
        {
            _editorGrid.ElementGrid[cellPos.x, cellPos.y].Rotate();
            return;
        }
    }
    private bool IsPointerOverUI()
    {
        if (_graphicRaycasters == null || _eventSystem == null)
            return false;

        PointerEventData pointerData = new(_eventSystem)
        {
            position = Pointer.current.position.ReadValue()
        };

        List<RaycastResult> results = new();
        foreach (var _reycaster in _graphicRaycasters)
        {
            _reycaster.Raycast(pointerData, results);
        }
        return results.Count > 0;
    }
    #region EditorGrid
    private class EditorElement
    {
        public List<GameObject> Elements = new();
        private readonly Vector3 Position;
        private readonly Transform ElementParent;
        public EditorElement(Vector3 pos, Transform elementParent)
        {
            Position = pos;
            ElementParent = elementParent;
        }
        public bool AddElement(GridElementTypeSO element, int rotation = 0, ElementState elementState = null)
        {
            foreach (GameObject obj in Elements)
            {
                if (obj.GetComponent<IGridElement>().Type == element.Type) return false;
            }
            var el = GenerateElementPrefab(Position, element, rotation, elementState);
            el.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero);
            Elements.Add(el);
            return true;
        }
        public void Rotate()
        {
            foreach (GameObject obj in Elements)
            {
                obj.transform.DOComplete();
                obj.transform.DORotate(new Vector3(0, -90, 0), 0.25f, RotateMode.WorldAxisAdd);

            }
        }
        public void ClearElements()
        {
            foreach (GameObject obj in Elements)
            {
                obj.transform.DOComplete();
                obj.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
                {
                    GameObject.Destroy(obj);
                });
            }
            Elements.Clear();
        }

        private GameObject GenerateElementPrefab(Vector3 position, GridElementTypeSO element, int rotation, ElementState elementState)
        {
            GameObject instance = GameObject.Instantiate(element.EditorPrefab, position + Vector3.up, Quaternion.Euler(90, rotation * 90, 0), ElementParent);

            if (elementState != null && instance.TryGetComponent(out ISavable savable))
            {
                savable.RestoreState(elementState);
            }

            return instance;
        }
    }
    private class EditorFloor
    {
        public GameObject Floor;
        private readonly Vector3 Position;
        private readonly Transform FloorParent;
        public EditorFloor(Vector3 pos, Transform floorParent)
        {
            Position = pos;
            FloorParent = floorParent;
        }
        public void SetFloor(GridFloorTypeSO floor, int rotation = 0, ElementState floorState = null)
        {
            ClearFloor();
            Floor = GenerateFloorPrefab(Position, floor, rotation, floorState);
            Floor.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero);
        }
        public void Rotate()
        {
            Floor.transform.DOComplete();
            Floor.transform.DORotate(new Vector3(0, -90, 0), 0.25f, RotateMode.WorldAxisAdd);
        }
        public void ClearFloor()
        {
            if (Floor == null) return;
            var temp = Floor;
            Floor = null;
            temp.transform.DOComplete();
            temp.transform.DOScale(Vector3.zero, 0.25f).OnComplete( () =>
            {
                GameObject.Destroy(temp);
            });
        }

        private GameObject GenerateFloorPrefab(Vector3 position, GridFloorTypeSO floor, int rotation, ElementState floorState)
        {
            GameObject instance = GameObject.Instantiate(floor.EditorPrefab, position, Quaternion.Euler(90, rotation * 90, 0), FloorParent);

            if (floorState != null && instance.TryGetComponent(out ISavable savable))
            {
                savable.RestoreState(floorState);
            }

            return instance;
        }
    }
    private class EditorGrid
    {
        public EditorFloor[,] FloorGrid;
        public EditorElement[,] ElementGrid;

        private readonly int _width;
        private readonly int _height;
        public EditorGrid(LevelData levelData,
            Transform elementParent,
            Transform floorParent,
            IReadOnlyDictionary<GridElementType, GridElementTypeSO> elementTypeToSO,
            IReadOnlyDictionary<FloorType, GridFloorTypeSO> floorTypeToSO)
        {
            _width = levelData.width;
            _height = levelData.height;

            InitializeGrid(elementParent, floorParent);

            RestoreGridFromLevelData(levelData, elementTypeToSO, floorTypeToSO);
        }
        public List<IGridElement> GetInteractableGridElements(Vector2Int gridPosition)
        {
            if (IsValidGridPosition(gridPosition))
            {
                List<IGridElement> interactable = new();
                foreach (GameObject el in ElementGrid[gridPosition.x, gridPosition.y].Elements)
                {
                    IGridElement elComp = el.GetComponent<IGridElement>();
                    if (elComp is ISavable _)
                    {
                        interactable.Add(elComp);
                    }
                }
                if (interactable.Count < 1) return null;
                return interactable;
            }
            return null;
        }
        public IFloor GetInteractableGridFloor(Vector2Int gridPosition)
        {
            GameObject floor = FloorGrid[gridPosition.x, gridPosition.y].Floor;
            if (IsValidGridPosition(gridPosition) && floor != null)
            {
                IFloor fl = floor.GetComponent<IFloor>();
                if (fl is ISavable _)
                {
                    return fl;
                }
            }
            return null;
        }
        public void DeleteElementsFrom(Vector2Int gridPosition)
        {
            if (IsValidGridPosition(gridPosition))
            {
                ElementGrid[gridPosition.x, gridPosition.y].ClearElements();
            }
        }
        public void DeleteFloorFrom(Vector2Int gridPosition)
        {
            if (IsValidGridPosition(gridPosition))
            {
                FloorGrid[gridPosition.x, gridPosition.y].ClearFloor();
            }
        }
        public void ClearAll()
        {
            foreach (EditorFloor obj in FloorGrid) obj.ClearFloor();
            foreach (EditorElement obj in ElementGrid) obj.ClearElements();
        }
        private bool IsValidGridPosition(Vector2Int gridPosition)
        {
            return gridPosition.x >= 0 && gridPosition.x < _width &&
                   gridPosition.y >= 0 && gridPosition.y < _height;
        }
        private void InitializeGrid(Transform elementParent, Transform floorParent)
        {
            ElementGrid = new EditorElement[_width, _height];
            FloorGrid = new EditorFloor[_width, _height];
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    ElementGrid[x, y] = new(new Vector3(x, 1, y), elementParent);
                    FloorGrid[x, y] = new(new Vector3(x, 1, y), floorParent);
                }
            }
        }
        private void RestoreGridFromLevelData(LevelData levelData, 
            IReadOnlyDictionary<GridElementType, GridElementTypeSO> elementTypeToSO,
            IReadOnlyDictionary<FloorType, GridFloorTypeSO> floorTypeToSO)
        {
            foreach (CellData cell in levelData.cells)
            {
                foreach (ElementData data in cell.elements)
                {
                    if (elementTypeToSO.TryGetValue(data.elementType, out GridElementTypeSO element))
                    {
                        ElementGrid[cell.x, cell.y].AddElement(element, data.elementRotation, data.ElementState);
                    }
                }
                if (floorTypeToSO.TryGetValue(cell.floorType, out GridFloorTypeSO floor))
                {
                    FloorGrid[cell.x, cell.y].SetFloor(floor, cell.floorRotation, cell.FloorState);
                }
            }
        }
    }
    #endregion
}
public enum EditorSelectedType
{
    Floors,
    Elements
}
public enum RedactorTool
{
    Create,
    Edit,
    Delete,
    Rotate
}