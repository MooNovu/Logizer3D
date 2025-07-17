using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMover : Mover
{
    [Inject] private IInputProvider _input;

    private Animator _anim;

    protected override void Start()
    {
        base.Start();
        _anim = GetComponent<Animator>();
        _input.OnMove += Move;
    }
    private void Move(Vector2Int direction)
    {
        TryMove(direction);
    }
    public override bool TryMove(Vector2Int direction)
    {
        transform.rotation = Quaternion.Euler(0, GetRotation(direction), 0);
        if (base.TryMove(direction))
        {
            //_anim.SetTrigger("Jump");
            GameEvents.PlayerMoved(GridSystem.GetWorldPosition(Position));
            return true;
        }
        return false;
    }
    public override bool TrySlide(Vector2Int targetPosition)
    {
        if (base.TrySlide(targetPosition))
        {
            //_anim.SetTrigger("Jump");
            GameEvents.PlayerMoved(GridSystem.GetWorldPosition(Position));
            return true;
        }
        return false;
    }
    private float GetRotation(Vector2Int direction)
    {
        if (direction == Vector2Int.up) return 0;
        if (direction == Vector2Int.down) return 180;
        if (direction == Vector2Int.left) return 90;
        if (direction == Vector2Int.right) return 270;
        return 0;
    }
    private void OnDestroy()
    {
        _input.OnMove -= Move;
    }
}
