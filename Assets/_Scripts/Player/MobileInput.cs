using System;
using UnityEngine;
using UnityEngine.UI;

public class MobileInput : MonoBehaviour, IInputProvider
{
    public event Action<Vector2Int> OnMove;

    [SerializeField] private Button _upButton;
    [SerializeField] private Button _downButton;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;

    private void Awake()
    {
        _upButton.onClick.AddListener(() => OnMove?.Invoke(Vector2Int.up));
        _downButton.onClick.AddListener(() => OnMove?.Invoke(Vector2Int.down));
        _leftButton.onClick.AddListener(() => OnMove?.Invoke(Vector2Int.left));
        _rightButton.onClick.AddListener(() => OnMove?.Invoke(Vector2Int.right));
    }
}
