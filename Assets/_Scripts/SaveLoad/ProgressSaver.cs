using System.IO;
using UnityEngine;

public static class ProgressSaver
{
    private const string _fileName = "progress.json";   
    private static readonly string _savePath = Path.Combine(Application.persistentDataPath, _fileName);
    public static void SaveProgress(int lvlId)
    {
        GameProgress progress = LoadProgress();
        if (!progress.CompletedLevels.Contains(lvlId))
        {
            progress.CompletedLevels.Add(lvlId);
        }
        string json = JsonUtility.ToJson(progress);
        File.WriteAllText(_savePath, json);
    }
    public static GameProgress LoadProgress()
    {
        if (File.Exists(_savePath))
        {
            string json = File.ReadAllText(_savePath);
            return JsonUtility.FromJson<GameProgress>(json);
        }
        return new();
    }
    public static bool IsLevelCompleted(int lvlId)
    {
        GameProgress progress = LoadProgress();
        return progress.CompletedLevels.Contains(lvlId);
    }
}
