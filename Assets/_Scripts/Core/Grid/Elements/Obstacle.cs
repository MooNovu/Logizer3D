using UnityEngine;

public class Obstacle : GridElement, IStatic
{
    public override GridElementType Type => GridElementType.Obstacle;
    public override bool IsWalkable(Vector2Int moveDirection) => false;
}
