using UnityEngine;
using Zenject;

public class EditorInstace : MonoBehaviour
{
    [Inject] private FromSceneLoader _fromSceneLoader;
    [Inject] private SaveManager _saveManager;

    private void Start()
    {
        _fromSceneLoader.LoadLevelFromSceneObject();
        _saveManager.SaveLevel();
    }
}
