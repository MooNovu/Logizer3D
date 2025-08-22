using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneSwitcher : MonoBehaviour, ISceneSwitcher
{
    private float _progress;
    public float LoadingProgress => _progress;

    public void SwitchScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        UIEvents.LoadingScreenAnimationStart();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        float startTime = Time.time;

        while (!asyncOperation.isDone)
        {
            _progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            //_loadingScreen.SetProgress(_progress);

            if (asyncOperation.progress >= 0.9f && Time.time - startTime >= 0.55f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}