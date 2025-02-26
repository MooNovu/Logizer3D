using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelListManager : MonoBehaviour
{
    private static LevelListManager _instance;
    public static LevelListManager Instance => _instance;

    private string currentStage;
    private List<LevelData> currentStageLevels = new();
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            currentStage = "Stage1";
        }
        else
        {
            Destroy(gameObject);
        }
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
}
