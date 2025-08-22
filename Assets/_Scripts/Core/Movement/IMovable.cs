using DG.Tweening;
using UnityEngine;

public interface IMovable
{
    Vector2Int PreviousPosition { get; set; }
    Vector2Int LastMoveVector { get; }
    bool TryMove(Vector2Int direction);
    bool TrySlide(Vector2Int targetPosition);
    bool TryTeleport(Vector2Int targetPosition, bool Inretact = true);
    void UndoLastMove();
    void SetNextMoveAnimation(MoverAnimation anim);
}
