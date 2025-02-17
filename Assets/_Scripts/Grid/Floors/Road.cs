using UnityEngine;

public class Road : FloorElement
{
    public override FloorType Type => FloorType.Road;
    public override bool IsWalkable => true;

}
