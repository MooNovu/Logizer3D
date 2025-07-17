using UnityEngine;

public class Road : FloorElement, IStatic
{
    public override FloorType Type => FloorType.Road;
    public override bool IsWalkable => true;

}
