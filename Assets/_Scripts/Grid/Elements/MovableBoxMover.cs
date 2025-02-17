using UnityEngine;

public class MovableBoxMover : Mover
{
    public override void Move(Vector2Int moveVector)
    {
        Vector2Int targetPosition = new(ThisObjectPosition.x + moveVector.x, ThisObjectPosition.y + moveVector.y);
        SetPreviousPosition(ThisObjectPosition);
        if (IsValidMove(targetPosition))
        {
            transform.position = GridManager.GridSystem.GetWorldPosition(targetPosition);
            InteractWInteractable(targetPosition);
            return;
        }
        targetPosition = new(ThisObjectPosition.x - moveVector.x, ThisObjectPosition.y - moveVector.y);
        transform.position = GridManager.GridSystem.GetWorldPosition(targetPosition);
        InteractWInteractable(targetPosition);
    }
    public override void CancelLastMove()
    {
        var lastMove = GetLastMoveVector();
        Move(-lastMove);
        if (CheckForPlayer(ThisObjectPosition - lastMove))
        {
            Move(-lastMove);
        }
    }

    private bool CheckForPlayer(Vector2Int target)
    {
        Collider[] coll = Physics.OverlapSphere(GridManager.GridSystem.GetWorldPosition(target), 0.2f);
        var thisObjectCollider = GetComponent<Collider>();
        foreach (Collider col in coll)
        {
            if (col == thisObjectCollider) continue;
            if (col.CompareTag("Player")) return true;
        }
        return false;
    }
}
