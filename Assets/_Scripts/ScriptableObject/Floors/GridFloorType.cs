using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Floor", menuName = "Grid/GridFloorType")]
public class GridFloorTypeSO : ScriptableObject
{
    public FloorType Type;
    public GameObject Prefab;
    public TileBase EditorTile;
    public Texture2D EditorIcon;
}
