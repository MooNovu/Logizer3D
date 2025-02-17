using UnityEngine;

[CreateAssetMenu(fileName = "New Floor", menuName = "Grid/GridFloorType")]
public class GridFloorTypeSO : ScriptableObject
{
    public FloorType Type;
    public GameObject Prefab;
    //public Texture2D EditorIcon;
}
