using UnityEngine;

public interface IMovable
{
    Vector2Int PreviousPosition { get; set; }
    Vector2Int LastMoveVector { get; }
    bool TryMove(Vector2Int direction);
    bool TryTeleport(Vector2Int targetPosition, bool Inretact = true);
    void UndoLastMove();
}
