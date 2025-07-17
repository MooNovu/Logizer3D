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
    [SerializeField] private EditorInterfaceManager _editorInterfaceManager;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private GraphicRaycaster _graphicRaycaster;

    [Header("Data")]
    [SerializeField] private GridElementsContainer _elementContainer;
    [SerializeField] private GridFloorsContainer _floorContainer;

    private GridElementTypeSO _selectedElement;
    private GridFloorTypeSO _selectedFloor;

    private EditorSelectedType _selectedType = EditorSelectedType.Floors;
    private RedactorTool _redactorTool = RedactorTool.Create;
    private Dictionary<RedactorTool, Action<Vector2Int>> _toolActions;

    private IReadOnlyDictionary<GridElementType, GridElementTypeSO> _elementTypeToSO;
    private IReadOnlyDictionary<FloorType, GridFloorTypeSO> _floorTypeToSO;

    private EditorGrid _editorGrid;
    private EditorInput _editorInput;
    private Vector2 _pointerPos;

    private void Awake()
    {
        _elementTypeToSO = _elementContainer.AllElements.ToDictionary(x => x.Type);
        _floorTypeToSO = _floorContainer.AllFloors.ToDictionary(x => x.Type);

        _toolActions = new()
        {
            [RedactorTool.Create] = RedactorCreate,
            [RedactorTool.Edit] = RedactorEdit,
            [RedactorTool.Delete] = RedactorDelete
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
    }
    private void Unsubscribe()
    {
        GameEvents.OnInitializeEditor -= InitializeGrid;
        _editorInput.Editor.Click.performed -= PointerTools;
        _editorInput.Disable();

        GameEvents.OnElementChange -= HandleElementChange;
        GameEvents.OnFloorChange -= HandleFloorChange;
    }
    private void HandleElementChange(GridElementTypeSO el) => _selectedElement = el;
    private void HandleFloorChange(GridFloorTypeSO fl) => _selectedFloor = fl;
    #endregion
    public void InitializeGrid(LevelData levelData)
    {
        _editorGrid?.ClearAll();
        _editorGrid = new(levelData, _elementParent, _floorParent, _elementTypeToSO, _floorTypeToSO);
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
    private void RedactorCreate(Vector2Int cellPos)
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
    private void RedactorEdit(Vector2Int cellPos)
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
    private void RedactorDelete(Vector2Int cellPos)
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
    public void ToggleRedactorMode()
    {
        switch (_redactorTool) 
        {
            case RedactorTool.Create:
                _redactorTool = RedactorTool.Edit;
                break;
            case RedactorTool.Edit:
                _redactorTool = RedactorTool.Delete;
                break;
            case RedactorTool.Delete:
                _redactorTool = RedactorTool.Create;
                break;
        }
        _editorInterfaceManager.RedactorButtonMode(_redactorTool);
    }
    public void ToggleSelectedType()
    {
        if (_selectedType == EditorSelectedType.Floors)
        {
            _selectedType = EditorSelectedType.Elements;
            _editorInterfaceManager.ShowElementPanel();
        }
        else
        {
            _selectedType = EditorSelectedType.Floors;
            _editorInterfaceManager.ShowFloorPanel();
        }
    }
    private bool IsPointerOverUI()
    {
        if (_graphicRaycaster == null || _eventSystem == null)
            return false;

        PointerEventData pointerData = new(_eventSystem)
        {
            position = Pointer.current.position.ReadValue()
        };

        List<RaycastResult> results = new();
        _graphicRaycaster.Raycast(pointerData, results);

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

            Elements.Add(GenerateElementPrefab(Position, element, rotation, elementState));
            return true;
        }
        public void ClearElements()
        {
            foreach (GameObject obj in Elements) GameObject.Destroy(obj);
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
        }
        public void ClearFloor()
        {
            if (Floor != null) GameObject.Destroy(Floor);
            Floor = null;
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
    Delete
}