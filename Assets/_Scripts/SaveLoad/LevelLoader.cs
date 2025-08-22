using DG.Tweening;
using System.Collections;
using UnityEngine;
using Zenject;

public class LevelLoader : MonoBehaviour
{
    [Inject] private readonly LoadManager _loadManager;
    [Inject] private readonly CameraController _cameraController;

    private void Awake()
    {
        GameEvents.OnPlayerReachedExit += LevelCompleted;
        GameEvents.OnLevelLoad += LoadLevel;
        GameEvents.OnLevelNextLoad += LoadNextLevel;
        GameEvents.OnLevelReload += RetryLevel;
    }
    private void OnDestroy()
    {
        GameEvents.OnPlayerReachedExit -= LevelCompleted;
        GameEvents.OnLevelLoad -= LoadLevel;
        GameEvents.OnLevelNextLoad -= LoadNextLevel;
        GameEvents.OnLevelReload -= RetryLevel;
        StopAllCoroutines();
        DOTween.Clear();
    }
    public void LevelCompleted()
    {
        StartCoroutine(CompleteLevelSequence());
    }
    public void LoadLevel(LevelData lvl)
    {

        StartCoroutine(LoadingLevelSequence(lvl));
    }
    public void LoadNextLevel()
    {
        CurrentLevelHandler.LoadNextLevel();
        if (CurrentLevelHandler.LevelData == null)
        {
            Debug.Log("All Levels Completed");
            return;
        }
        StartCoroutine(LoadingLevelSequence(CurrentLevelHandler.LevelData));
    }
    public void RetryLevel()
    {
        StopAllCoroutines();
        StartCoroutine(RetryLevelSequence());
    }

    public IEnumerator LoadingLevelSequence(LevelData lvl)
    {
        GameEvents.ClearCandies();
        _loadManager.ClearLevel();
        //Debug.Log($"Starting Loading level {CurrentLevelHandler.LevelId}");
        _loadManager.LoadLevel(lvl);
        yield return null;

        StartCoroutine(Camera());

        yield return _loadManager.SpawnAnimation();

        _loadManager.OptimizeStaticObjects();
        GameEvents.LevelFullLoaded();
    }
    public IEnumerator RetryLevelSequence()
    {
        UIEvents.LoadingScreenAnimationStart();
        yield return new WaitForSeconds(0.5f);
        UIEvents.LoadingScreenAnimationEnd();
        GameEvents.ClearCandies();
        _loadManager.ClearLevel();
        _loadManager.RemovePlayer();

        LevelData lvl = CurrentLevelHandler.LevelData;
        //Debug.Log($"Reload level {lvl.Name}");
        _loadManager.LoadLevel(lvl);
        yield return null;

        StartCoroutine(Camera());

        yield return _loadManager.SpawnAnimation();

        _loadManager.OptimizeStaticObjects();
        GameEvents.LevelFullLoaded();
    }
    public IEnumerator CompleteLevelSequence()
    {
        yield return null;//_loadManager.ClearLevel();
        _loadManager.RemovePlayer();
        UIEvents.ShowResaulMenu();
    }
    private IEnumerator Camera()
    {
        yield return _cameraController.CameraSequence();
        _loadManager.SpawnPlayer();
    }
}
