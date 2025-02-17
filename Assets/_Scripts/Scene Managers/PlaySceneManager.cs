using UnityEngine;
using Zenject;

public class PlaySceneManager : MonoBehaviour
{
    //[Inject] private GridManager gridManager;
    [Inject] private SaveManager saveManager;

    private void Start()
    {
        saveManager.LoadLevel();
    }

}
