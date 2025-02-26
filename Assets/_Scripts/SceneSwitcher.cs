using System.Collections;
using Unity.Loading;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneSwitcher : MonoBehaviour
{
    private static SceneSwitcher _instance;
    public static SceneSwitcher Instance => _instance;

    private float progress;
    public float LoadingProgress { get { return progress; } }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void LoadLevel()
    {
        StartCoroutine(LoadSceneAsync(1));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            if (asyncOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1);
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
