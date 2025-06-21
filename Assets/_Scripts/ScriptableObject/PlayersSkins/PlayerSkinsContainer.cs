using UnityEngine;

[CreateAssetMenu(fileName = "SkinContainer", menuName = "Grid/PlayerSkinContainer")]
public class PlayerSkinsContainer : ScriptableObject
{
    public PlayerSkinsSO[] AllSkins;
}