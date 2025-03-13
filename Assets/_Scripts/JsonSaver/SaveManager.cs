using System.IO;
using UnityEngine;
using Zenject;

public class SaveManager : MonoBehaviour
{
    [Inject] private GridManager gridManager;
    [SerializeField] private string fileName = "level.json";
    //private void Start()
    //{
    //    SaveLevel();
    //}
    public void SaveLevel()
    {
        LevelData data = gridManager.CreateSaveData();
        string json = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, json);
        Debug.Log($"Level saved to {path}");
    }

    public void LoadLevel(LevelData levelData)
    {
        gridManager.LoadFromSaveData(levelData);
        Debug.Log($"Level loaded");
    }
}
