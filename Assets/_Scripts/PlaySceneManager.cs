using UnityEngine;
using Zenject;

public class PlaySceneManager : MonoBehaviour
{
    //[Inject] private GridManager gridManager;
    [Inject] private SaveLoadManager saveManager;

    private void Start()
    {
        saveManager.LoadLevel(LevelDataHolder.SelectedLevel);
    }

}
