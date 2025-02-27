using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    private GridElementType _selectedElementType = GridElementType.Obstacle;
    private FloorType _selectedFloorType = FloorType.Road;
    private bool _placementElementMode;
    private bool _placementFloorMode;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _selectedElementType = (GridElementType)EditorGUILayout.EnumPopup("Element Type", _selectedElementType);
        _placementElementMode = GUILayout.Toggle(_placementElementMode, "Place Element", "Button");

        _selectedFloorType = (FloorType)EditorGUILayout.EnumPopup("Floor Type", _selectedFloorType);
        _placementFloorMode = GUILayout.Toggle(_placementFloorMode, "Place Floor", "Button");


        if (_placementFloorMode)
        {
            SceneView.duringSceneGui += DuringSceneFloorGUI;
            _placementElementMode = false;
        }
        else
        {
            SceneView.duringSceneGui -= DuringSceneFloorGUI;
        }
        if (_placementElementMode)
        {
            SceneView.duringSceneGui += DuringSceneElementGUI;
            _placementFloorMode = false;
        }
        else
        {
            SceneView.duringSceneGui -= DuringSceneElementGUI;
        }
    }

    private void DuringSceneFloorGUI(SceneView sceneView)
    {
        if (!_placementFloorMode) return;

        Event e = Event.current;
        if (e.type == EventType.MouseDown)
        {
            if (e.button == 0)
            {
                Vector2 mousePosition = e.mousePosition;
                Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

                if (Physics.Raycast(ray, out var hit))
                {
                    var gridManager = (GridManager)target;
                    var prefab = gridManager.GetPrefabForFloor(_selectedFloorType);
                    var newFloor = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                    newFloor.transform.SetParent(gridManager.FloorParent, true);
                    Undo.RegisterCreatedObjectUndo(newFloor, "Create Floor");

                    if (gridManager.TryPlaceFloor(hit.point, newFloor.GetComponent<IFloor>()))
                    {
                        Debug.Log($"Floor placed");
                    }
                    else
                    {
                        Debug.LogWarning("Failed to place floor");
                        DestroyImmediate(newFloor);
                    }
                    //_placementFloorMode = false;
                    e.Use();
                }
            }
            if (e.button == 1)
            {
                _placementFloorMode = false;

            }
        }
    }
    private void DuringSceneElementGUI(SceneView sceneView)
    {
        if (!_placementElementMode) return;

        Event e = Event.current;
        if (e.type == EventType.MouseDown)
        {
            if (e.button == 0)
            {
                Vector2 mousePosition = e.mousePosition;
                Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

                if (Physics.Raycast(ray, out var hit))
                {
                    var gridManager = (GridManager)target;
                    var prefab = gridManager.GetPrefabForGridElement(_selectedElementType);
                    var newElement = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                    newElement.transform.SetParent(gridManager.ElementsParent, true);
                    Undo.RegisterCreatedObjectUndo(newElement, "Create Grid Element");

                    if (gridManager.TryPlaceElement(hit.point, newElement.GetComponent<IGridElement>()))
                    {
                        Debug.Log($"Element placed");
                    }
                    else
                    {
                        Debug.LogWarning("Failed to place element");
                        DestroyImmediate(newElement);
                    }
                    //_placementElementMode = false;
                    e.Use();
                }
            }
            if (e.button == 1)
            {
                _placementElementMode = false;

            }
        }
    }
    private void OnSceneGUI()
    {
        if (!_placementElementMode && !_placementFloorMode) return;
        Event e = Event.current;

        Vector2 mousePosition = Event.current.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
        //var gridManager = (GridManager)target;
        if (Physics.Raycast(ray, out var hit))
        {
            Handles.color = Color.green;
            if (GridManager.GridSystem.TryGetGridPosition(hit.point, out var gridPos))
            {
                Handles.DrawWireCube(new Vector3(gridPos.x, 0, gridPos.y), Vector3.one * 0.5f);
            }
            else
            {
                Handles.DrawWireCube(hit.point, Vector3.one * 0.5f);
            }
            
            Handles.Label(hit.point + Vector3.up, "Place position");
        }
        if (e.type == EventType.MouseMove)
        {
            SceneView.RepaintAll();
        }
    }
}
#endif