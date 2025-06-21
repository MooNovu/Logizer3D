using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerSkin", menuName = "Grid/PlayerSkin")]
public class PlayerSkinsSO : ScriptableObject
{
    public Skin Skin;
    public GameObject Prefab;
    //public Texture2D EditorIcon;
}

public enum Skin
{
    Default
}