using UnityEngine;

[CreateAssetMenu(fileName = "New Type", menuName = "Grid/GridElementType")]
public class GridElementTypeSO : ScriptableObject
{
    public GridElementType Type;
    public GameObject Prefab;
    //public Texture2D EditorIcon;
}
