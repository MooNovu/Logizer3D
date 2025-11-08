using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MobileInput : MonoBehaviour, IInputProvider, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public event Action<Vector2Int> OnMove;

    [SerializeField] private GameObject _circleIndicator;

    CanvasGroup _indicatorCanvasGroup;
    RectTransform _indicatorTransform;

    private readonly float _swipeThreshold = 50f;
    private readonly float _repeatDelay = 0.3f;
    private readonly float _repeatInterval = 0.25f;

    private bool _isActive = false;

    private Vector2 _touchStartPos;
    private Vector2Int _currentDirection;
    private bool _isSwiping;
    private bool _hasSwiped;
    private bool _safeZone;
    private float _repeatTimer;

    private void Start()
    {
        _indicatorCanvasGroup = _circleIndicator.GetComponent<CanvasGroup>();
        _indicatorTransform = _circleIndicator.GetComponent<RectTransform>();

        GameEvents.OnInputEnable += EnableInput;
        GameEvents.OnInputDisable += DisableInput;
    }
    private void OnDisable()
    {
        GameEvents.OnInputEnable -= EnableInput;
        GameEvents.OnInputDisable -= DisableInput;
    }
    private void DisableInput()
    {
        _isActive = false;
    }
    private void EnableInput()
    {
        _isActive = true;
    }
    private void Update()
    {
        if (!_isActive) return;
        if (!_isSwiping || !_hasSwiped || _safeZone) return;

        _repeatTimer -= Time.deltaTime;

        if (_repeatTimer <= 0)
        {
            OnMove?.Invoke(_currentDirection);
            _repeatTimer = _repeatInterval;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _touchStartPos = eventData.position;
        ShowIndicator();
        _isSwiping = true;
        _hasSwiped = false;
        _safeZone = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        HideIndicator();
        _isSwiping = false;
        _hasSwiped = false;
        _safeZone = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!_isActive) return;
        if (!_isSwiping) return;
        _safeZone = true;
        Vector2 currentPos = eventData.position;
        Vector2 totalDelta = currentPos - _touchStartPos;
        if (!_hasSwiped && totalDelta.magnitude >= _swipeThreshold)
        {
            _currentDirection = GetSwipeDirection(totalDelta);
            OnMove?.Invoke(_currentDirection);
            _hasSwiped = true;
            _repeatTimer = _repeatDelay;
            return;
        }

        if (_hasSwiped && totalDelta.magnitude >= _swipeThreshold)
        {
            _safeZone = false;
            Vector2Int newDirection = GetSwipeDirection(totalDelta);
            if (newDirection != _currentDirection)
            {
                _currentDirection = newDirection;
                //OnMove?.Invoke(_currentDirection);
                //_repeatTimer = _repeatInterval;
            }
        }
    }
    private Vector2Int GetSwipeDirection(Vector2 swipeDelta)
    {
        Vector2 rotated = Quaternion.Euler(0, 0, -22.5f) * swipeDelta;

        if (Mathf.Abs(rotated.x) > Mathf.Abs(rotated.y))
            return rotated.x > 0 ? Vector2Int.right : Vector2Int.left;
        else
            return rotated.y > 0 ? Vector2Int.up : Vector2Int.down;
    }

    private void ShowIndicator()
    {
        _indicatorTransform.transform.position = _touchStartPos;
        _indicatorTransform.DOScale(new Vector3(1, 1, 1), 0.1f).SetEase(Ease.InQuad);
        _indicatorCanvasGroup.DOFade(1f, 0.1f).SetEase(Ease.InQuad);
    }
    private void HideIndicator()
    {
        _indicatorTransform.DOScale(new Vector3(0, 0, 0), 0.1f).SetEase(Ease.InQuad);
        _indicatorCanvasGroup.DOFade(0f, 0.1f).SetEase(Ease.InQuad);
    }
}
