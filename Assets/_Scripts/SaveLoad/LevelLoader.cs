using System.Collections;
using UnityEngine;
using Zenject;

public class LevelLoader : MonoBehaviour
{
    [Inject] private readonly LoadManager _loadManager;
    [Inject] private readonly ILevelList _levelList;
    [Inject] private readonly CameraController _cameraController;

    private void Awake()
    {
        GameEvents.OnPlayerReachedExit += LevelCompleted;
        GameEvents.OnLevelLoad += LoadNextLevel;
        GameEvents.OnLevelReload += RetryLevel;
    }
    private void OnDestroy()
    {
        GameEvents.OnPlayerReachedExit -= LevelCompleted;
        GameEvents.OnLevelLoad -= LoadNextLevel;
        GameEvents.OnLevelReload -= RetryLevel;
    }
    public void LevelCompleted()
    {
        int stage = PlayerPrefs.GetInt("Stage");
        int level = PlayerPrefs.GetInt("Level");
        Debug.Log($"Level {stage}/{level} Completed!");
        StartCoroutine(CompleteLevelSequence());
    }

    public void LoadNextLevel()
    {
        FindNextLevel();

        StartCoroutine(LoadingLevelSequence());
    }
    public void RetryLevel()
    {
        StartCoroutine(RetryLevelSequence());
    }

    public IEnumerator LoadingLevelSequence()
    {
        UIEvents.BlockUI();
        int stage = PlayerPrefs.GetInt("Stage");
        int level = PlayerPrefs.GetInt("Level");
        Debug.Log($"Starting Loading level {stage}/{level}");
        _loadManager.LoadLevel(_levelList.GetLevel(stage, level));
        yield return null;

        StartCoroutine(Camera());

        yield return _loadManager.SpawnAnimation();

        _loadManager.OptimizeStaticObjects();
    }
    public IEnumerator RetryLevelSequence()
    {
        UIEvents.BlockUI();
        yield return _loadManager.ClearLevel();
        _loadManager.RemovePlayer();

        int stage = PlayerPrefs.GetInt("Stage");
        int level = PlayerPrefs.GetInt("Level");
        Debug.Log($"Reload level {stage}/{level}");
        _loadManager.LoadLevel(_levelList.GetLevel(stage, level));
        yield return null;

        StartCoroutine(Camera());

        yield return _loadManager.SpawnAnimation();

        _loadManager.OptimizeStaticObjects();
    }
    public IEnumerator CompleteLevelSequence()
    {
        UIEvents.BlockUI();
        yield return _loadManager.ClearLevel();
        _loadManager.RemovePlayer();
        UIEvents.UnblockUI();
        UIEvents.ShowResaulMenu();
    }
    private IEnumerator Camera()
    {
        yield return _cameraController.CameraSequence();
        _loadManager.SpawnPlayer();
        UIEvents.UnblockUI();
    }

    private void FindNextLevel()
    {
        int stageId = PlayerPrefs.GetInt("Stage", -1);
        int levelId = PlayerPrefs.GetInt("Stage", -1);

        int stagesCount = _levelList.GetStagesCount();
        int levelsInStageCount = _levelList.GetLevelsCount(stageId);
        if (stageId == -1 && levelId == -1)
        {
            PlayerPrefs.SetInt("Stage", 1);
            PlayerPrefs.SetInt("Level", 1);
            return;
        }
        if (levelId < levelsInStageCount)
        {
            PlayerPrefs.SetInt("Level", levelId + 1);
            return;
        }
        if (stageId < stagesCount)
        {
            PlayerPrefs.SetInt("Stage", stageId + 1);
            PlayerPrefs.SetInt("Level", 1);
            return;
        }
    }
}
