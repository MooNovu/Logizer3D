using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class LevelList
{
    private readonly static string _levelsPath = "Levels/";
    public static int GetLevelsCount()
    {
        TextAsset[] levelFiles = Resources.LoadAll<TextAsset>(_levelsPath);
        
        return levelFiles.Count();
    }
    public static LevelData GetLevel(int levelId)
    {
        TextAsset levelFile = Resources.Load<TextAsset>(Path.Combine(_levelsPath, levelId.ToString()));

        return JsonUtility.FromJson<LevelData>(levelFile.text);
    }

    public static int GetUserLevelsCount()
    {
        TextAsset[] levelFiles = Resources.LoadAll<TextAsset>(SaveManager.UserLevelPath);

        return levelFiles.Count();
    }
    public static List<string> GetAllUserLevelNames()
    {
        List<string> levelNames = new();

        if (!Directory.Exists(SaveManager.UserLevelPath))
        {
            Directory.CreateDirectory(SaveManager.UserLevelPath);
            return levelNames;
        }

        string[] jsonFiles = Directory.GetFiles(SaveManager.UserLevelPath, "*.json");

        foreach (string filePath in jsonFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            levelNames.Add(fileName);
        }
        return levelNames;
    }
    public static LevelData GetUserLevel(string levelName)
    {
        string filePath = Path.Combine(SaveManager.UserLevelPath, $"{levelName}.json");

        if (!Directory.Exists(SaveManager.UserLevelPath))
        {
            Directory.CreateDirectory(SaveManager.UserLevelPath);
            return null;
        }

        try
        {
            string jsonData = File.ReadAllText(filePath);
            LevelData levelData = JsonUtility.FromJson<LevelData>(jsonData);

            if (levelData == null)
            {
                Debug.LogError($"Failed to parse level data from: {filePath}");
            }

            return levelData;

        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading level {levelName}: {e.Message}");
            return null;
        }
    }
}