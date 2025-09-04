using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ProgressSaver
{
    private const string _fileName = "progress.json";
    private static readonly string _savePath = Path.Combine(Application.persistentDataPath, _fileName);
    public static void SaveProgress(int lvlId, int StarsCollected)
    {
        if (lvlId == -1) return;
        GameProgress progress = LoadProgress();
        Dictionary<int, int> CompletedLevels = progress.CompletedLevels.ToDictionary();

        if (!CompletedLevels.ContainsKey(lvlId))
        {
            CompletedLevels.Add(lvlId, StarsCollected);
        }
        else if (CompletedLevels[lvlId] < StarsCollected)
        {
            CompletedLevels[lvlId] = StarsCollected;
        }
        else return;

        progress.CompletedLevels.FromDictionary(CompletedLevels);

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
        Dictionary<int, int> CompletedLevels = progress.CompletedLevels.ToDictionary();

        if (CompletedLevels.ContainsKey(lvlId))
            return CompletedLevels[lvlId];

        return null;
    }
}
