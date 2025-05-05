using UnityEngine;

public interface IMovable
{
    void Move(Vector2Int moveVector);
    void Teleport(Vector2Int targetPosition);
    public Vector2Int GetLastMoveVector();
    public void SetPreviousPosition(Vector2Int pos);
    public void CancelLastMove();
}
