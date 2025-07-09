using UnityEngine;

public static class CurrentLevelHandler
{
    public static LevelData LevelData { get; private set; }

    public static int LevelId { get; private set; }

    public static bool IsUserLevel { get; private set; }

    public static void SetLevel(int lvlId)
    {
        IsUserLevel = false;
        LevelData = LevelList.GetLevel(lvlId);
        LevelId = lvlId;
    }
    public static void SetLevel(LevelData lvlData, bool userLvl = true)
    {
        IsUserLevel = userLvl;
        LevelData = lvlData;
        LevelId = -1;
    }

    public static void LoadNextLevel()
    {
        if (!IsUserLevel)
        {
            LevelData = LevelList.GetLevel(LevelId + 1);
            LevelId++;
        }
    }
}
