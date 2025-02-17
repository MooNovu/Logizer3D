using UnityEngine;
using Zenject;

public class MovableBox : GridElement
{
    public override GridElementType Type => GridElementType.MovableBox;
    public override bool IsWalkable => true;

    private MovableBoxMover mover;
    private void OnEnable()
    {
        mover = GetComponent<MovableBoxMover>();
    }
    public override void Interact(IMovable movable)
    {
        var lastMove = movable.GetLastMoveVector();
        mover.Move(lastMove);
    }
}
