using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GridCell
{
    public IFloor Floor { get; private set; }
    public List<IGridElement> Elements { get; private set; }
    public Vector2Int Position { get; }
    public GridCell(Vector2Int position)
    {
        Position = position;
        Elements = new();
    }
    public void SetFloor(IFloor floor)
    {
        Floor = floor;
    }

    public void AddElement(IGridElement element)
    {
        if (Elements.Contains(element)) return;

        Elements.Add(element); 
    }
    public IGridElement GetFirstElementOfType(GridElementType type)
    {
        foreach (IGridElement el in Elements)
        {
            if (el.Type == type)
            {
                return el;
            }
        }
        return null;
    }
    public void ClearFloor()
    {
        Floor = null;
    }
    public void RemoveElement(IGridElement element)
    {
        Elements.Remove(element); 
    }
}
