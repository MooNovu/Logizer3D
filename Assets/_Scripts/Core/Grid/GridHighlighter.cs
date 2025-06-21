using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using Zenject;

[ExecuteInEditMode]
public class GridHighlighter : MonoBehaviour
{
    [Inject] private GridSystem _gridSystem;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_gridSystem == null) return;

        for (int x = 0; x < _gridSystem.Width; x++)
        {
            for (int y = 0; y < _gridSystem.Height; y++)
            {
                var pos = _gridSystem.GetWorldPosition(new Vector2Int(x, y));
                Gizmos.DrawWireCube(pos + Vector3.up * 0.01f, 
                    new Vector3(GridSystem.CellSize, 0.01f, GridSystem.CellSize));
            }
        }
    }
#endif
}
