using UnityEngine;

public abstract class GridElement : MonoBehaviour, IGridElement
{
    public abstract GridElementType Type { get; }
    public abstract bool IsWalkable(Vector2Int moveDirection);

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
}
