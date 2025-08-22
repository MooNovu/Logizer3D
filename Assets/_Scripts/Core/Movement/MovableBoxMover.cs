using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MovableBoxMover : Mover
{
    private Transform ChilrenTransform;
    protected override void Start()
    {
        base.Start();
        if (transform.childCount > 0) ChilrenTransform = transform.GetChild(0).GetComponent<Transform>();
    }
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

        if (base.TryMove(direction))
        {
            ChilrenTransform.DORotate(GetRotation(direction), 0.25f, RotateMode.WorldAxisAdd).OnComplete(RoundRotation);
            return true;
        }
        return false;
    }
    private Vector3 GetRotation(Vector2Int direction)
    {
        if (direction == Vector2Int.up) return new Vector3(90, 0, 0);

        if (direction == Vector2Int.down) return new Vector3(-90, 0, 0);

        if (direction == Vector2Int.right) return new Vector3(0, 0, -90);

        if (direction == Vector2Int.left) return new Vector3(0, 0, 90);

        return Vector3.zero;
    }
    private void RoundRotation()
    {
        Vector3 angles = ChilrenTransform.eulerAngles;

        angles.x = (int)((angles.x + 10f) / 90) * 90;
        angles.y = (int)((angles.y + 10f) / 90) * 90;
        angles.z = (int)((angles.z + 10f) / 90) * 90;
        ChilrenTransform.rotation = Quaternion.Euler(angles);
    }
}
