using System;
using UnityEngine;

public interface IInputProvider
{
    event Action<Vector2Int> OnMove;
}
