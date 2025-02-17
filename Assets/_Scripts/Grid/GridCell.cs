using UnityEngine;

public class GridCell
{
    public IFloor Floor;
    public IGridElement Element { get; private set; }

    public Vector2Int Position { get; }

    public GridCell(Vector2Int position)
    {
        Position = position; 
    }
    public void SetFloor(IFloor floor)
    {
        Floor = floor;
    }
    public void SetElement(IGridElement element)
    {
        Element = element; 
    }
    public void ClearFloor()
    {
        Floor = null; 
    }
    public void ClearElement()
    {
        Element = null; 
    }
}
