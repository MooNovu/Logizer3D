using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Type", menuName = "Grid/GridElementType")]
public class GridElementTypeSO : ScriptableObject
{
    public GridElementType Type;
    public GameObject Prefab;

    public GameObject EditorPrefab;
    public Texture2D EditorIcon;
}
