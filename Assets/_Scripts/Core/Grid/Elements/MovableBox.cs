using TMPro;
using UnityEngine;
using Zenject;

public class MovableBox : GridElement, IInteractable, ISpecialAnimation
{
    public override GridElementType Type => GridElementType.MovableBox;
    public override bool IsWalkable(Vector2Int _) => true;
    public MoverAnimation EnterAnimation() => MoverAnimation.ToMoverAnimation;
    private MovableBoxMover mover;
    private void OnEnable()
    {
        mover = GetComponent<MovableBoxMover>();
    }
    public void Interact(IMovable movable)
    {
        mover.TryMove(movable.LastMoveVector);
    }
}
