using UnityEngine;
using UnityEngine.Events;
using System;

public abstract class InputHandler : MonoBehaviour
{
    public event Action<Vector2Int> OnMovuInput;
    public event Action OnMenuOpen;

    protected void InvokeMove(Vector2Int direction) => OnMovuInput?.Invoke(direction);
    protected void InvokeMenu() => OnMenuOpen?.Invoke();
}
