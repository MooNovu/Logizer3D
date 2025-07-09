using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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

    protected virtual void Start()
    {
        Position = GridSystem.GetGridPosition(transform.position);
        thisElement ??= GetComponent<IGridElement>();
    }

    public virtual bool TryMove(Vector2Int direction)
    {
        Vector2Int targetPosition = Position + direction;
        if (!_gridSystem.IsCellWalkable(targetPosition, direction)) return false;

        PreviousPosition = Position;
        Vector2Int storePos = Position;
        Position = targetPosition;

        _animation = transform.DOMove(GridSystem.GetWorldPosition(targetPosition), 0.25f)
            .SetEase(Ease.InQuad)
            .From(GridSystem.GetWorldPosition(PreviousPosition));

        TryInteractWithObjects(targetPosition);

        _gridSystem.TryMoveElement(storePos, Position, thisElement);
        return true;
    }

    public virtual bool TryTeleport(Vector2Int targetPosition, bool Inretact = true)
    {
        if (!_gridSystem.IsCellWalkable(targetPosition)) return false;
        Position = targetPosition;
        _animation.Kill();
        transform.position = GridSystem.GetWorldPosition(targetPosition);

        if (Inretact) TryInteractWithObjects(targetPosition);

        return true;
    }
    public virtual void UndoLastMove()
    {
        TryMove(-LastMoveVector);
    }
    protected void TryInteractWithObjects(Vector2Int target)
    {
        List<GameObject> elements = _gridSystem.GetElementsAsGameObjects(target);
        foreach (GameObject element in elements)
        {
            if (element != null && element.TryGetComponent(out IInteractable interact))
            {
                interact.Interact(this);
            }
        }

        List<GameObject> floors = _gridSystem.GetElementsAsGameObjects(target);
        foreach (GameObject floor in floors)
        {
            if (floor != null && floor.TryGetComponent(out ISteppable step))
            {
                step.OnStep(this);
            }
        }
    }

}
