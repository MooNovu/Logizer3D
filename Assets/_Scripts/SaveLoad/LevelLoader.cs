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
    public void LevelCompleted() => CompleteLevelSequence();
    public void LoadLevel(LevelData lvl) => StartCoroutine(FirstLoadingLevelSequence(lvl));
    public void LoadNextLevel()
    {
        CurrentLevelHandler.LoadNextLevel();
        if (CurrentLevelHandler.LevelData == null)
        {
            Debug.Log("All Levels Completed");
            return;
        }

        StartCoroutine(LoadingNextLevelSequence(CurrentLevelHandler.LevelData));
    }
    public void RetryLevel()
    {
        StopAllCoroutines();
        StartCoroutine(RetryLevelSequence());
    }
    private IEnumerator LoadingNextLevelSequence(LevelData lvl)
    {
        GameEvents.DisableInput();
        yield return UIEvents.LoadingScreenAnimationStart().WaitForCompletion();

        ClearLevel();
        _loadManager.LoadLevel(lvl);

        UIEvents.LoadingScreenAnimationEnd().WaitForCompletion();

        StartCoroutine(CameraAnimation());

        yield return _loadManager.SpawnAnimation();

        LevelLoadingComplete();
    }
    private IEnumerator FirstLoadingLevelSequence(LevelData lvl)
    {
        ClearLevel();
        GameEvents.DisableInput();
        _loadManager.LoadLevel(lvl);

        StartCoroutine(CameraAnimation());

        yield return _loadManager.SpawnAnimation();

        LevelLoadingComplete();
    }
    private IEnumerator RetryLevelSequence()
    {
        GameEvents.DisableInput();
        yield return UIEvents.LoadingScreenAnimationStart().WaitForCompletion();

        ClearLevel();
        _loadManager.RemovePlayer();

        UIEvents.LoadingScreenAnimationEnd();

        _loadManager.LoadLevel(CurrentLevelHandler.LevelData);

        StartCoroutine(CameraAnimation());
        yield return _loadManager.SpawnAnimation();
        LevelLoadingComplete();

    }
    public void CompleteLevelSequence()
    {
        _loadManager.RemovePlayer();
        ProgressSaver.SaveProgress(CurrentLevelHandler.LevelId, GameEvents.CandiesCollected);
        UIEvents.ShowResaulMenu();
    }
    private IEnumerator CameraAnimation()
    {
        yield return _cameraController.CameraSequence();
        _loadManager.SpawnPlayer();
        GameEvents.SetCurrentTime();
    }

    private void ClearLevel()
    {
        GameEvents.ClearCandies();
        GameEvents.ClearSteps();
        _loadManager.ClearLevel();
    }
    private void LevelLoadingComplete()
    {
        _loadManager.OptimizeStaticObjects();
        GameEvents.EnableInput();
        GameEvents.LevelFullLoaded();
    }
}
