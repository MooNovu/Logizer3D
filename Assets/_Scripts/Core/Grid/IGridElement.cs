using UnityEngine;
using UnityEngine.EventSystems;

public interface IGridElement
{
    GridElementType Type { get; }
    Vector2Int GridPosistion { get; }
    bool IsWalkable(Vector2Int moveDirection);
    int Rotation { get; }
}

public interface IFloor
{
    FloorType Type { get; }
    Vector2Int GridPosistion { get; }
    bool IsWalkable { get; }
    int Rotation { get; }
}

public enum FloorType
{
    Abyss,
    Road
}
public enum GridElementType 
{
    None,
    Obstacle,
    Portal,
    MovableBox,
    Spawn,
    Exit,
    Player
}
