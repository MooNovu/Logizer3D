using UnityEngine;
using Zenject;

public class Key : GridElement
{
    public override GridElementType Type => GridElementType.Key;
    public override bool IsWalkable(Vector2Int _) => true;


}
