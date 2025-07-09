using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform _gridRenderer;
    [SerializeField] private Camera _sceneCamera;


    [SerializeField] private EditorSettings _editorSettings;
    [SerializeField] private EditorButtonCreator _editorButtonCreator;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private GraphicRaycaster _graphicRaycaster;

    [Header("Data")]
    [SerializeField] private GridElementsContainer _elementContainer;
    [SerializeField] private GridFloorsContainer _floorContainer;

    private Dictionary<GridElementType, GridElementTypeSO> _getSObyElement = new();
    private Dictionary<FloorType, GridFloorTypeSO> _getSObyFloor = new();

    private EditorGrid _gameObjectGrid;
    private EditorInput _editorInput;
    private int _gridWidth = 0;
    private int _gridHeight = 0;
    private Vector2 _pointerPos;

    private GridElementTypeSO _selectedElement;
    private GridFloorTypeSO _selectedFloor;
    private EditorSelectedType _selectedType = EditorSelectedType.Floors;
    private RedactorTool _redactorTool = RedactorTool.Create;

    private void Awake()
    {
        foreach (GridElementTypeSO element in _elementContainer.AllElements)
        {
            _getSObyElement.Add(element.Type, element);
        }
        foreach (GridFloorTypeSO floor in _floorContainer.AllFloors)
        {
            _getSObyFloor.Add(floor.Type, floor);
        }

        GameEvents.OnInitializeEditor += HandleInitializeEditor;
        _editorInput = new();
        _editorInput.Editor.Click.performed += PointerTools;
        _editorInput.Enable();

        GameEvents.OnElementChange += HandleElementChange;
        GameEvents.OnFloorChange += HandleFloorChange;
    }
    private void OnDisable()
    {
        GameEvents.OnInitializeEditor -= HandleInitializeEditor;
        _editorInput.Editor.Click.performed -= PointerTools;
        _editorInput.Disable();

        GameEvents.OnElementChange -= HandleElementChange;
        GameEvents.OnFloorChange -= HandleFloorChange;
    }
    #region Обработчики
    private void HandleInitializeEditor(LevelData levelData)
    {
        _gridWidth = levelData.width;
        _gridHeight = levelData.height;
        InitializeGrid(levelData);
    }
    private void HandleElementChange(GridElementTypeSO el) => _selectedElement = el;
    private void HandleFloorChange(GridFloorTypeSO fl) => _selectedFloor = fl;
    #endregion
    public void InitializeGrid(LevelData levelData)
    {
        _gameObjectGrid?.ClearAll();

        _gameObjectGrid = new(levelData, _getSObyElement, _getSObyFloor);

        float offsetW = _gridWidth % 2 == 0 ? 0.5f : 0f;
        float offsetH = _gridHeight % 2 == 0 ? 0.5f : 0f;
        _gridRenderer.position = new(_gridWidth / 2 - offsetW, 2, _gridHeight / 2 - offsetH);
        _gridRenderer.localScale = new(_gridWidth, _gridHeight, 1);
    }

    private void PointerTools(InputAction.CallbackContext ctx)
    {
        if (IsPointerOverUI()) return;
        _pointerPos = Pointer.current.position.ReadValue();
        Ray ray = _sceneCamera.ScreenPointToRay(_pointerPos);
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            Vector2Int cellPos = GridSystem.GetGridPosition(hit.point);

            switch (_redactorTool)
            {
                case RedactorTool.Create:
                    RedactorCreate(cellPos);
                    break;

                case RedactorTool.Edit:
                    RedactorEdit(cellPos);
                    break;

                case RedactorTool.Delete:
                    RedactorDelete(cellPos);
                    break;

            }
        }
    }
    private void RedactorCreate(Vector2Int cellPos)
    {
        if (_selectedType == EditorSelectedType.Floors)
        {
            if (_selectedFloor == null) return;
            _gameObjectGrid.FloorGrid[cellPos.x, cellPos.y].SetFloor(_selectedFloor);
            return;
        }

        if (_selectedType == EditorSelectedType.Elements)
        {
            if (_selectedElement == null) return;
            _gameObjectGrid.ElementGrid[cellPos.x, cellPos.y].AddElement(_selectedElement);
            return;
        }

    }
    private void RedactorEdit(Vector2Int cellPos)
    {
        if (_selectedType == EditorSelectedType.Floors)
        {
            _editorSettings.OpenSettings(_gameObjectGrid.GetInteractableGridFloor(cellPos));
            return;
        }
        if (_selectedType == EditorSelectedType.Elements)
        {
            List<IGridElement> elements = _gameObjectGrid.GetInteractableGridElements(cellPos);
            if (elements == null || elements?.Count < 1) return;
            _editorSettings.OpenSettings(elements[0]);
            return;
        }
    }
    private void RedactorDelete(Vector2Int cellPos)
    {
        if (_selectedType == EditorSelectedType.Floors)
        {
            _gameObjectGrid.DeleteFloorFrom(cellPos);
            return;
        }
        if (_selectedType == EditorSelectedType.Elements)
        {
            _gameObjectGrid.DeleteElementsFrom(cellPos);
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
        _editorButtonCreator.RedactorButtonMode(_redactorTool);
    }
    public void ToggleSelectedType()
    {
        if (_selectedType == EditorSelectedType.Floors)
        {
            _selectedType = EditorSelectedType.Elements;
            _editorButtonCreator.ShowElementPanel();
        }
        else
        {
            _selectedType = EditorSelectedType.Floors;
            _editorButtonCreator.ShowFloorPanel();
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
}
public class EditorElement
{
    public List<GameObject> Elements = new();
    private readonly Vector3 Position;
    public EditorElement(Vector3 pos)
    {
        Position = pos;
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
        GameObject instance = GameObject.Instantiate(element.EditorPrefab, position + Vector3.up, Quaternion.Euler(90, rotation * 90, 0));

        if (elementState != null && instance.TryGetComponent(out ISavable savable))
        {
            savable.RestoreState(elementState);
        }

        return instance;
    }
}
public class EditorFloor
{
    public GameObject Floor;
    private readonly Vector3 Position;
    public EditorFloor(Vector3 pos)
    {
        Position = pos;
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
        GameObject instance = GameObject.Instantiate(floor.EditorPrefab, position, Quaternion.Euler(90, rotation * 90, 0));

        if (floorState != null && instance.TryGetComponent(out ISavable savable))
        {
            savable.RestoreState(floorState);
        }

        return instance;
    }
}
public class EditorGrid
{
    public EditorFloor[,] FloorGrid;
    public EditorElement[,] ElementGrid;

    private int _width;
    private int _height;
    public EditorGrid(LevelData levelData, 
        Dictionary<GridElementType, GridElementTypeSO> getSObyElement, 
        Dictionary<FloorType, GridFloorTypeSO> getSObyFloor)
    {
        _width = levelData.width;
        _height = levelData.height;
        ElementGrid = new EditorElement[_width, _height];
        FloorGrid = new EditorFloor[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                ElementGrid[x, y] = new(new Vector3(x, 1, y));
                FloorGrid[x, y] = new(new Vector3(x, 1, y));
            }
        }
        foreach (CellData cell in levelData.cells)
        {
            foreach (ElementData data in cell.elements)
            {
                if (getSObyElement.TryGetValue(data.elementType, out GridElementTypeSO element))
                {
                    ElementGrid[cell.x, cell.y].AddElement(element, data.elementRotation, data.ElementState);
                }
            }
            if (getSObyFloor.TryGetValue(cell.floorType, out GridFloorTypeSO floor))
            {
                FloorGrid[cell.x, cell.y].SetFloor(floor, cell.floorRotation, cell.FloorState);
            }
            
        }
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
    private bool IsValidGridPosition(Vector2Int gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < _width &&
               gridPosition.y >= 0 && gridPosition.y < _height;
    }
    public void ClearAll()
    {
        foreach (EditorFloor obj in FloorGrid) obj.ClearFloor();
        foreach (EditorElement obj in ElementGrid) obj.ClearElements();
    }
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