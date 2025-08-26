using System.IO;
using UnityEngine;

public static class ProgressSaver
{
    private const string _fileName = "progress.json";
    private static readonly string _savePath = Path.Combine(Application.persistentDataPath, _fileName);
    public static void SaveProgress(int lvlId, int StarsCollected)
    {
        GameProgress progress = LoadProgress();
        if (!progress.CompletedLevels.ContainsKey(lvlId))
        {
            progress.CompletedLevels.Add(lvlId, StarsCollected);
        }
        else if (progress.CompletedLevels[lvlId] < StarsCollected)
        {
            progress.CompletedLevels[lvlId] = StarsCollected;
        }
        else return;
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

        GameProgress progress = new();
        File.WriteAllText(_savePath, JsonUtility.ToJson(progress));
        return progress;
    }
    public static int? IsLevelCompleted(int lvlId)
    {
        GameProgress progress = LoadProgress();
        if (progress.CompletedLevels.ContainsKey(lvlId))
            return progress.CompletedLevels[lvlId];

        return null;
    }
}
