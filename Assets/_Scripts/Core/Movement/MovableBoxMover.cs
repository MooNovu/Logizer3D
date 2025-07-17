using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MovableBoxMover : Mover
{
    public override bool TryMove(Vector2Int direction)
    {
        Vector2Int targetPosition = Position + direction;

        if (_gridSystem.IsCellWalkable(targetPosition, direction))
        {
            if (_gridSystem.IsMovableOnPosition(targetPosition))
            {
                direction = -direction;
            }
        }
        else direction = -direction;

        return base.TryMove(direction);
    }
}
