using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelEditor : MonoBehaviour
{
    [Header("TileMap References")]
    [SerializeField] private Tilemap _elementsTilemap;
    [SerializeField] private Tilemap _floorTilemap;

    [Header("Data")]
    [SerializeField] private GridElementsContainer _elementContainer;
    [SerializeField] private GridFloorsContainer _floorContainer;

    private void Start()
    {
        Debug.Log("He");
    }
}
