using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public interface ILevelList
{
    List<LevelData> LoadStage(int stageId);
    int GetStagesCount();
    int GetLevelsCount(int stageId);
    LevelData GetLevel(int stageId, int levelId);
}

public class LevelList : ILevelList
{
    private readonly string _levelsPath = "Levels";
    public List<LevelData> LoadStage(int stageId)
    {
        List<LevelData> levels = new();
        TextAsset[] levelFiles = Resources.LoadAll<TextAsset>($"{_levelsPath}/{stageId}");
        foreach (TextAsset file in levelFiles)
        {
            LevelData level = JsonUtility.FromJson<LevelData>(file.text);
            levels.Add(level);
        }
        return levels;
    }
    public int GetStagesCount()
    {
        TextAsset[] stages = Resources.LoadAll<TextAsset>(_levelsPath);

        return stages.Count();
    }

    public int GetLevelsCount(int stageId)
    {
        TextAsset[] levelsInStage = Resources.LoadAll<TextAsset>($"{_levelsPath}/{stageId}");
        
        return levelsInStage.Count();
    }

    public LevelData GetLevel(int stageId, int levelId)
    {
        TextAsset[] levelFiles = Resources.LoadAll<TextAsset>($"{_levelsPath}/{stageId}");
        if (levelId < 1 || levelId - 1 >= levelFiles.Length) return null;
        return JsonUtility.FromJson<LevelData>(levelFiles[levelId - 1].text);
    }
}