using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public abstract class Mover : MonoBehaviour, IMovable
{
    [Inject] protected GridSystem _gridSystem;
    protected IGridElement thisElement;
    protected Vector2Int Position { get; set; }
    public Vector2Int PreviousPosition { get; set; }
    public Vector2Int LastMoveVector => Position - PreviousPosition;

    protected Tween _animation;
    protected IExitSpecialAnimation _nextMoveAnimation = null;
    protected Vector2Int? _pendingDirection = null;
    protected bool _isAnimationRunning;

    protected virtual void Start()
    {
        Position = GridSystem.GetGridPosition(transform.position);
        thisElement ??= GetComponent<IGridElement>();
    }

    public virtual bool TryMove(Vector2Int direction)
    {
        if (_isAnimationRunning)
        {
            _pendingDirection = direction;
            _animation.Complete();
            return true;
        }
        Vector2Int targetPosition = Position + direction;
        if (!_gridSystem.IsCellWalkable(targetPosition, direction)) return false;

        _isAnimationRunning = true;

        PreviousPosition = Position;

        StartCoroutine(MoveSequence(targetPosition));

        return true;
    }
    protected IEnumerator MoveSequence(Vector2Int targetPosition)
    {
        Vector2Int originalPosition = Position;
        Vector2Int newPosition = targetPosition;

        TryPostInteract(PreviousPosition);
        Position = newPosition;

        _animation.Kill();
        if (_nextMoveAnimation != null)
        {
            _animation = _nextMoveAnimation.GetExitAnimation(transform, newPosition);
            _nextMoveAnimation = null;
        }
        else if (_gridSystem.TryGetSpecialAnimation(newPosition, out ISpecialAnimation anim))
        {
            _animation = anim.GetAnimation(transform, newPosition);
        }
        else
        {
            _animation = transform.DOMove(GridSystem.GetWorldPosition(newPosition), 0.25f)
                .SetEase(Ease.InQuad)
                .From(GridSystem.GetWorldPosition(PreviousPosition));
        }
        _gridSystem.TryMoveElement(originalPosition, Position, thisElement);
        yield return _animation.WaitForCompletion();

        TryInteract(newPosition);

        _isAnimationRunning = false;

        if (_pendingDirection.HasValue)
        {
            Vector2Int nextMoveDirection = _pendingDirection.Value;
            _pendingDirection = null;
            TryMove(nextMoveDirection);
        }

    }
    public virtual bool TrySlide(Vector2Int targetPosition)
    {
        if (!_gridSystem.IsCellWalkable(targetPosition)) return false;

        Vector2Int delta = targetPosition - Position;
        if ((delta.x == 0 && delta.y == 0) || (delta.x != 0 && delta.y != 0)) return false;

        Vector2Int storePos = Position;

        int stepX = delta.x != 0 ? (int)Mathf.Sign(delta.x) : 0;
        int stepY = delta.y != 0 ? (int)Mathf.Sign(delta.y) : 0;
        Vector2Int step = new(stepX, stepY);

        Vector2Int calculatedTargetPosition = Position;

        while (calculatedTargetPosition != targetPosition && !_gridSystem.IsMovableOnPosition(calculatedTargetPosition + step))
        {
            if (_gridSystem.GetCell(calculatedTargetPosition + step).HasTransformChanger())
                return TryMove(step);

            TryPostInteract(calculatedTargetPosition);
            calculatedTargetPosition += step;
            TryInteract(calculatedTargetPosition, false);
        }

        if (calculatedTargetPosition == storePos) return false;
        PreviousPosition = Position;
        Position = calculatedTargetPosition;

        _animation.Kill();
        _animation = transform.DOMove(GridSystem.GetWorldPosition(calculatedTargetPosition), 0.25f)
            .SetEase(Ease.InQuad);

        _gridSystem.TryMoveElement(storePos, Position, thisElement);
        return true;

    }
    public virtual bool TryTeleport(Vector2Int targetPosition, bool Interact = true)
    {
        if (!_gridSystem.IsCellWalkable(targetPosition)) return false;

        if (Interact) TryPostInteract(Position);

        Vector2Int storePos = Position;
        Position = targetPosition;
        _animation.Kill();
        transform.position = GridSystem.GetWorldPosition(targetPosition);

        if (Interact) TryInteract(targetPosition);

        _gridSystem.TryMoveElement(storePos, Position, thisElement);

        return true;
    }
    public virtual void UndoLastMove()
    {
        TryMove(-LastMoveVector);
    }
    public virtual void SetNextMoveAnimation(IExitSpecialAnimation anim)
    {
        _nextMoveAnimation = anim;
    }
    protected void TryPostInteract(Vector2Int previousPosition, bool checkFloor = true)
    {

        GridCell cell = _gridSystem.GetCell(previousPosition);
        List<IGridElement> elements = cell.Elements.ToList();
        foreach (IGridElement e in elements)
        {
            if (e != null && e is IPostInteractable postInteractableElement)
            {
                if (postInteractableElement == thisElement) continue;
                postInteractableElement.PostInteract(this);
            }
        }
        if (checkFloor && cell.Floor != null && cell.Floor is IPostInteractable postInteractableFloor)
        {
            postInteractableFloor.PostInteract(this);
        }
    }
    protected void TryInteract(Vector2Int target, bool checkFloor = true)
    {
        GridCell cell = _gridSystem.GetCell(target);
        List<IGridElement> elements = cell.Elements.ToList();
        foreach (IGridElement e in elements)
        {
            if (e != null && e is IInteractable interactableElement)
            {
                if (interactableElement == thisElement) continue;
                interactableElement.Interact(this);
            }
        }
        if (checkFloor && cell.Floor != null && cell.Floor is IInteractable interactableFloor)
        {
            interactableFloor.Interact(this);
        }
    }

}
