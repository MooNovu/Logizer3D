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
        SaveData data = gridManager.CreateSaveData();
        string json = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, json);
        Debug.Log($"Level saved to {path}");
    }

    public void LoadLevel()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            gridManager.LoadFromSaveData(data);
            Debug.Log($"Level loaded");
        }
        else
        {
            Debug.LogError("Save file not found");
        }
    }
}
