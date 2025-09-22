using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    protected MoverAnimation _nextMoveAnimation = MoverAnimation.None;
    protected MoverAnimation _currentMoveAnimation = MoverAnimation.None;

    protected Dictionary<MoverAnimation, Action<Vector2Int>> GetAnimation;

    protected Vector2Int? _pendingDirection = null;
    protected bool _isAnimationRunning;

    protected const float _animationDuration = 0.25f;

    protected virtual void Start()
    {
        Position = GridSystem.GetGridPosition(transform.position);
        thisElement ??= GetComponent<IGridElement>();

        GetAnimation = new()
        {
            [MoverAnimation.None] = null,
            [MoverAnimation.Default] = DefaultAnimation,
            [MoverAnimation.ScaleToZero] = ScaleToZeroAnimation,
            [MoverAnimation.ScaleToNormal] = ScaleToNormalAnimation,
            [MoverAnimation.ToMoverAnimation] = ToMoverAnimation
        };

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
    protected virtual IEnumerator MoveSequence(Vector2Int targetPosition)
    {
        Vector2Int originalPosition = Position;
        Vector2Int newPosition = targetPosition;

        TryPostInteract(PreviousPosition);
        Position = newPosition;

        _animation.Kill();
        if (_nextMoveAnimation != MoverAnimation.None)
        {
            GetAnimation[_nextMoveAnimation](newPosition);
            _nextMoveAnimation = MoverAnimation.None;
        }
        else if (_gridSystem.TryGetSpecialAnimation(newPosition, out MoverAnimation moverAnim))
        {
            GetAnimation[moverAnim](newPosition);
        }
        else
        {
            DefaultAnimation(newPosition);
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

        bool isTransformChanger = false;
        Vector2Int calculatedTargetPosition = Position;

        while (calculatedTargetPosition != targetPosition && !_gridSystem.IsMovableOnPosition(calculatedTargetPosition + step))
        {
            if (_gridSystem.GetCell(calculatedTargetPosition + step).HasTransformChanger())
            {
                isTransformChanger = true;
                break;
            }

            TryPostInteract(calculatedTargetPosition);
            calculatedTargetPosition += step;
            TryInteract(calculatedTargetPosition, false);
        }

        if (calculatedTargetPosition == storePos) return false;
        PreviousPosition = Position;
        Position = calculatedTargetPosition;

        _animation.Kill();
        _animation = transform.DOMove(GridSystem.GetWorldPosition(calculatedTargetPosition), 0.25f)
            .SetEase(Ease.InQuad).OnComplete( () => { if (isTransformChanger) TryMove(step); });

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
    public virtual void SetNextMoveAnimation(MoverAnimation anim)
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
    #region Animation

    protected virtual void DefaultAnimation(Vector2Int to)
    {
        _animation = transform.DOMove(GridSystem.GetWorldPosition(to), _animationDuration);
    }
    protected virtual void ScaleToNormalAnimation(Vector2Int to)
    {
        _animation = DOTween.Sequence()
            .Append(transform.DOMove(GridSystem.GetWorldPosition(to), _animationDuration))
            .Join(transform.DOScale(Vector3.one, _animationDuration).SetEase(Ease.InQuad));
    }
    protected virtual void ScaleToZeroAnimation(Vector2Int to)
    {
        _animation = DOTween.Sequence()
            .Append(transform.DOMove(GridSystem.GetWorldPosition(to), _animationDuration))
            .Join(transform.DOScale(Vector3.zero, _animationDuration).SetEase(Ease.InQuad));
    }
    protected virtual void ToMoverAnimation(Vector2Int to)
    {
        DefaultAnimation(to);
    }
    #endregion
}

public enum MoverAnimation
{
    None,
    Default,
    ScaleToZero, //Portal In
    ScaleToNormal, // Portal Out
    ToMoverAnimation
}