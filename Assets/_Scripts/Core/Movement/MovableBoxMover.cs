using System.Collections.Generic;
using UnityEngine;

public class MovableBoxMover : Mover
{
    public override bool TryMove(Vector2Int direction)
    {
        Vector2Int targetPosition = Position + direction;
        if (_gridSystem.IsCellWalkable(targetPosition, direction))
        {
            List<GameObject> targetGameObjects = _gridSystem.GetElementsAsGameObjects(targetPosition);
            foreach (GameObject gameObject in targetGameObjects)
            {
                if (gameObject != null && gameObject.TryGetComponent(out IMovable _))
                {
                    direction = -direction;
                    break;
                }
            }
        }
        else direction = -direction;

        if (base.TryMove(direction)) return true;

        return false;
    }
}
