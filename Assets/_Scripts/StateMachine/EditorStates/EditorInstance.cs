using UnityEngine;
using Zenject;

public class EditorInstance : MonoBehaviour
{
    private void Start()
    {
        GameEvents.InitializeEditor(CurrentLevelHandler.LevelData);
    }
}
