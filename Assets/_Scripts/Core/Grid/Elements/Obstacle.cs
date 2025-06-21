using UnityEngine;

public class Obstacle : GridElement
{
    public override GridElementType Type => GridElementType.Obstacle;
    public override bool IsWalkable(Vector2Int moveDirection) => false;
}
