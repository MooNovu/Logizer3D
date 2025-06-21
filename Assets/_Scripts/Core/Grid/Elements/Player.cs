using UnityEngine;

public class Player : GridElement
{
    public override GridElementType Type => GridElementType.Player;
    public override bool IsWalkable(Vector2Int moveDirection) => true;
}
