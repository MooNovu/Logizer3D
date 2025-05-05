using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneSwitcher : MonoBehaviour, ISceneSwitcher
{
    [Inject] private ILoadingScreen _loadingPanel;
    private float _progress;
    public float LoadingProgress { get { return _progress; } }
    public void LoadLevel()
    {
        StartCoroutine(LoadSceneAsync(2));
    }
    public void LoadMainMenu()
    {
        StartCoroutine(LoadSceneAsync(1));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;
        _loadingPanel.FadeOut();
        float startTime = Time.time;

        while (!asyncOperation.isDone)
        {
            _progress = Mathf.Clamp01(asyncOperation.progress);

            if (asyncOperation.progress >= 0.9f && Time.time - startTime >= 0.75f)
            {
                asyncOperation.allowSceneActivation = true;
                _loadingPanel.FadeIn();
            }

            yield return null;
        }
    }
}