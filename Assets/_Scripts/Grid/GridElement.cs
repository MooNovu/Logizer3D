using UnityEngine;

public abstract class GridElement : MonoBehaviour, IGridElement, IInteractable, ISavable
{
    public abstract GridElementType Type { get; }
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

    public virtual void Interact(IMovable movable) { }
    public virtual ElementState CaptureState() => null;
    public virtual void RestoreState(ElementState state) { }
}
