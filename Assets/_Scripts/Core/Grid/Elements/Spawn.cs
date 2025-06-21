using UnityEngine;

public class Spawn : GridElement
{
    public override GridElementType Type => GridElementType.Spawn;
    public override bool IsWalkable(Vector2Int moveDirection) => true;
}
