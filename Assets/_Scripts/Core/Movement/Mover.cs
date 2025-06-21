using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class Mover : MonoBehaviour, IMovable
{
    [Inject] protected GridSystem _gridSystem;
    protected IGridElement thisElement;

    protected Vector2Int Position
    {
        get
        {
            if (_gridSystem.TryGetGridPosition(transform.position, out var gridPos)) return gridPos;
            return new Vector2Int(-1000, -1000);
        }
    }
    public Vector2Int PreviousPosition { get; set; }
    public Vector2Int LastMoveVector => Position - PreviousPosition;
    public virtual bool TryMove(Vector2Int direction)
    {
        thisElement ??= GetComponent<IGridElement>();
        Vector2Int targetPosition = Position + direction;
        if (!_gridSystem.IsCellWalkable(targetPosition, direction)) return false;

        PreviousPosition = Position;
        Vector2Int storePos = Position;
        transform.position = _gridSystem.GetWorldPosition(targetPosition);
        
        TryInteractWithObjects(targetPosition);

        _gridSystem.TryMoveElement(storePos, Position, thisElement);
        return true;
    }

    public virtual bool TryTeleport(Vector2Int targetPosition)
    {
        if (!_gridSystem.IsCellWalkable(targetPosition, LastMoveVector)) return false;
        thisElement ??= GetComponent<IGridElement>();

        transform.position = _gridSystem.GetWorldPosition(targetPosition);
        TryInteractWithObjects(targetPosition);
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
