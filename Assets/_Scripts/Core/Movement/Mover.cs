using UnityEngine;
using Zenject;

public abstract class Mover : MonoBehaviour, IMovable
{
    [Inject] protected GridSystem _gridSystem;
    protected Vector2Int ThisObjectPosition
    {
        get
        {
            if (_gridSystem.TryGetGridPosition(transform.position, out var gridPos)) return gridPos;
            return new Vector2Int(0, 0);
        }
    }
    public Vector2Int PreviousPosition { get; private set; }
    public virtual void Move(Vector2Int moveVector)
    {
        Vector2Int targetPosition = new(ThisObjectPosition.x + moveVector.x, ThisObjectPosition.y + moveVector.y);
        if (IsValidMove(targetPosition))
        {
            PreviousPosition = ThisObjectPosition;
            transform.position = _gridSystem.GetWorldPosition(targetPosition);
            InteractWInteractable(targetPosition);
            return;
        }
        //Debug.Log("Некорректный ход");
    }

    public virtual void Teleport(Vector2Int targetPosition)
    {
        transform.position = _gridSystem.GetWorldPosition(targetPosition);
        InteractWInteractable(targetPosition);
        return;
        //Если упал то убить...
    }
    public Vector2Int GetLastMoveVector()
    {
        return (ThisObjectPosition - PreviousPosition);
    }
    public void SetPreviousPosition(Vector2Int position)
    {
        PreviousPosition = position;
    }
    public virtual void CancelLastMove()
    {
        var lastMove = GetLastMoveVector();
        Move(-lastMove);
    }

    protected bool IsValidMove(Vector2Int targetPosition)
    {
        if (!_gridSystem.IsCellWalkable(targetPosition)) return false;
        return true;
    }

    protected void InteractWInteractable(Vector2Int target)
    {
        Collider[] coll = Physics.OverlapSphere(_gridSystem.GetWorldPosition(target), 0.2f, LayerMask.GetMask("Interactable"));
        var thisObjectCollider = GetComponent<Collider>();
        foreach (Collider col in coll)
        {
            if (col == thisObjectCollider) continue;
            var element = col.GetComponent<IInteractable>();
            element?.Interact(this);
        }

    }
}
