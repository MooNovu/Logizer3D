using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelList : ILevelList
{
    private string currentStage;
    private List<LevelData> currentStageLevels = new();

    public LevelList()
    {
        currentStage = "Stage1";

    }

    public List<LevelData> LoadStage(string stageName)
    {
        currentStage = stageName;
        currentStageLevels.Clear();

        TextAsset[] levelFiles = Resources.LoadAll<TextAsset>($"Levels/{stageName}");
        foreach (TextAsset file in levelFiles)
        {
            LevelData level = JsonUtility.FromJson<LevelData>(file.text);
            currentStageLevels.Add(level);
        }
        return currentStageLevels;
    }

    public LevelData GetLevel(int levelId)
    {
        if (currentStageLevels.Count <= 0) LoadStage(currentStage);
        return currentStageLevels[levelId] ?? null;
    }

    public LevelData GetLevel(string stageName, int levelId)
    {
        TextAsset[] levelFiles = Resources.LoadAll<TextAsset>($"Levels/{stageName}");
        return JsonUtility.FromJson<LevelData>(levelFiles[levelId].text);
    }
}


public interface ILevelList 
{
    List<LevelData> LoadStage(string stageName);
    LevelData GetLevel(int levelId);
    LevelData GetLevel(string stageName, int levelId);
}

