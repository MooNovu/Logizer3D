using UnityEngine;

public abstract class FloorElement : MonoBehaviour, IFloor , IStoppable, ISavable
{
    public abstract FloorType Type { get; }
    public abstract bool IsWalkable { get; }

    public int Rotation
    {
        get
        {
            return (int)transform.eulerAngles.y / 90;
        }
    }
    public Vector2Int GridPosistion
    {
        get
        {
            return GridSystem.GetGridPosition(transform.position);
        }
    }

    public void OnStep(PlayerMover playerMover) { }

    public virtual ElementState CaptureState() => null;

    public virtual void RestoreState(ElementState state) { }
}
