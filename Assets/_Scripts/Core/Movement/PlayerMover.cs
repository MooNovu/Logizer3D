using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMover : Mover
{
    [Inject] private IInputProvider _input;

    private void Start()
    {
        _input.OnMove += Move;
    }
    private void Move(Vector2Int direction)
    {
        if (TryMove(direction))
        {
            GameEvents.PlayerMoved(_gridSystem.GetWorldPosition(Position));
        }
    }
    private void OnDestroy()
    {
        _input.OnMove -= Move;
    }
}
