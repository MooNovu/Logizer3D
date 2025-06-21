using UnityEngine;
using Zenject;

public class Exit : GridElement, IInteractable
{
    public override GridElementType Type => GridElementType.Exit;
    public override bool IsWalkable(Vector2Int _) => true;

    public void Interact(IMovable movable)
    {
        if (movable is PlayerMover _)
        {
            GameEvents.PlayerReachedExit();
        }
    }
}
