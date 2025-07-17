using UnityEngine;
using Zenject;

public class Ice : FloorElement, IInteractable
{
    [Inject] private readonly GridSystem _gridSystem;
    public override FloorType Type => FloorType.Ice;
    public override bool IsWalkable => true;
    private readonly int _maxIter = 100;
    public void Interact(IMovable movable)
    {
        Vector2Int targetPos = GridPosistion;
        for (int i = 0; i < _maxIter; i++)
        {
            Vector2Int iterTarget = GridPosistion + movable.LastMoveVector * i;
            if (_gridSystem.GetCell(iterTarget)?.Floor?.Type == FloorType.Ice)
            {
                targetPos = iterTarget;
                continue;
            }
            break;
        }
        movable.TrySlide(targetPos);
    }
}
