using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorCamera : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private List<GraphicRaycaster> _graphicRaycasters;

    private EditorInput _editorInput;
    private Camera _sceneCamera;

    private int _gridWidth = 0;
    private int _gridHeight = 0;

    private float _initialPinchDistance;
    private float _initialOrthoSize;
    private readonly float _cameraMoveSpeed = 5f;
    private readonly float _minZoom = 4f;
    private float _maxZoom = 12f;

    private void Awake()
    {
        GameEvents.OnInitializeEditor += (LevelData levelData) =>
        {
            _gridWidth = levelData.width;
            _gridHeight = levelData.height;
            _maxZoom = Mathf.Max(_gridWidth, _gridHeight);
        };

        _sceneCamera = GetComponent<Camera>();

        _editorInput = new();
        _editorInput.Editor.Zoom2.performed += CameraZoom;
        _editorInput.Editor.MouseZoom.performed += CameraMouseZoom;
        _editorInput.Editor.Move.performed += CameraMove;
        _editorInput.Enable();
    }
    private void OnDisable()
    {
        GameEvents.OnInitializeEditor -= (LevelData levelData) =>
        {
            _gridWidth = levelData.width;
            _gridHeight = levelData.height;
            _maxZoom = Mathf.Max(_gridWidth, _gridHeight);
        };

        _editorInput.Editor.MouseZoom.performed -= CameraMouseZoom;
        _editorInput.Editor.Move.performed -= CameraMove;
        _editorInput.Editor.Zoom2.performed -= CameraZoom;
        _editorInput.Disable();
    }
    private void CameraMouseZoom(InputAction.CallbackContext ctx)
    {
        float scrollDelta = ctx.ReadValue<float>();
        _sceneCamera.orthographicSize = Mathf.Clamp(_sceneCamera.orthographicSize + scrollDelta, _minZoom, _maxZoom);
    }
    private void CameraZoom(InputAction.CallbackContext ctx)
    {
        if (IsPointerOverUI()) return;
        TouchControl firstTouch = Touchscreen.current.touches[0];
        TouchControl secondTouch = Touchscreen.current.touches[1];

        if (firstTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began ||
            secondTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
        {
            _initialPinchDistance = Vector2.Distance(firstTouch.position.ReadValue(), secondTouch.position.ReadValue());
            _initialOrthoSize = _sceneCamera.orthographicSize;
        }
        else if (firstTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved ||
                secondTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved)
        {
            float currentDistance = Vector2.Distance(firstTouch.position.ReadValue(), secondTouch.position.ReadValue());

            float zoomFactor = _initialPinchDistance / currentDistance;
            float newOrthoSize = _initialOrthoSize * zoomFactor;

            _sceneCamera.orthographicSize = Mathf.Clamp(newOrthoSize, _minZoom, _maxZoom);
        }
    }
    private void CameraMove(InputAction.CallbackContext ctx)
    {
        if (IsPointerOverUI()) return;
        Vector2 delta = ctx.ReadValue<Vector2>() * -Vector2.one;
        Vector3 newPosition = _sceneCamera.transform.position +
            _cameraMoveSpeed * _sceneCamera.orthographicSize * 0.0002f * new Vector3(delta.x, 0, delta.y);

        newPosition.x = Mathf.Clamp(newPosition.x, -0.5f, _gridWidth);
        newPosition.z = Mathf.Clamp(newPosition.z, -0.5f, _gridHeight);

        _sceneCamera.transform.position = newPosition;
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
}