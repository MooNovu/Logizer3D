using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMover : Mover
{
    [Inject] private IInputProvider _input;

    protected override void Start()
    {
        base.Start();
        _input.OnMove += Move;
    }
    private void OnDestroy()
    {
        _input.OnMove -= Move;
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
            GameEvents.PlayerMoved(GridSystem.GetWorldPosition(Position));
            return true;
        }
        return false;
    }
    protected override IEnumerator MoveSequence(Vector2Int targetPosition)
    {
        yield return base.MoveSequence(targetPosition);
        GameEvents.AddStep();
    }
    public override bool TrySlide(Vector2Int targetPosition)
    {
        if (base.TrySlide(targetPosition))
        {
            GameEvents.PlayerMoved(GridSystem.GetWorldPosition(Position));
            return true;
        }
        return false;
    }

    protected override void ToMoverAnimation(Vector2Int to)
    {
        _animation = DOTween.Sequence()
            .Append(transform.DOMove(GridSystem.GetWorldPosition(to), _animationDuration))
            .Join(transform.DOScale(Vector3.one * 2, 0.15f));
        StartCoroutine(ScaleBack());
    }
    private IEnumerator ScaleBack()
    {
        yield return _animation.WaitForCompletion();
        transform.DOScale(Vector3.one, 0.15f);
    }

    private float GetRotation(Vector2Int direction)
    {
        if (direction == Vector2Int.up) return 0;
        if (direction == Vector2Int.down) return 180;
        if (direction == Vector2Int.right) return 90;
        if (direction == Vector2Int.left) return 270;
        return 0;
    }

}
