using UnityEngine;

public class MovableBoxMover : Mover
{
    public override void Move(Vector2Int moveVector)
    {
        Vector2Int targetPosition = new(ThisObjectPosition.x + moveVector.x, ThisObjectPosition.y + moveVector.y);
        SetPreviousPosition(ThisObjectPosition);
        if (IsValidMove(targetPosition) && !CheckForTag(targetPosition, "Movable"))
        {
            transform.position = _gridSystem.GetWorldPosition(targetPosition);
            InteractWInteractable(targetPosition);
            return;
        }
        targetPosition = new(ThisObjectPosition.x - moveVector.x, ThisObjectPosition.y - moveVector.y);
        transform.position = _gridSystem.GetWorldPosition(targetPosition);
        InteractWInteractable(targetPosition);
    }
    public override void CancelLastMove()
    {
        var lastMove = GetLastMoveVector();
        Move(-lastMove);
        if (CheckForTag(ThisObjectPosition - lastMove, "Player"))
        {
            Move(-lastMove);
        }
    }

    private bool CheckForTag(Vector2Int target, string tag)
    {
        Collider[] coll = Physics.OverlapSphere(_gridSystem.GetWorldPosition(target), 0.2f);
        var thisObjectCollider = GetComponent<Collider>();
        foreach (Collider col in coll)
        {
            if (col == thisObjectCollider) continue;
            if (col.CompareTag(tag)) return true;
        }
        return false;
    }
}
