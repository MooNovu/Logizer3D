using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiHandle
{
    private static string apiBaseUrl = "http://localhost:5000/api/Levels";

    public static IEnumerator GetLevelDataCoroutine(Action<List<FetchedLevelData>> onSuccess, Action<string> onError = null)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiBaseUrl))
        {
            // await
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log($"Получен ответ: {jsonResponse}");

                string wrappedJson = "{\"levels\":" + jsonResponse + "}";

                var levelList = JsonUtility.FromJson<LevelListWrapper>(wrappedJson);
                onSuccess?.Invoke(levelList.levels.ToList());
            }
            else
            {
                string error = $"Ошибка: {request.error}";
                Debug.LogError(error);
                onError?.Invoke(error);
            }
        }
    }

    public static IEnumerator CreateLevelCoroutine(UploadLevelData levelData, Action<UploadLevelData> onSuccess = null, Action<string> onError = null)
    {
        string jsonData = JsonUtility.ToJson(levelData);

        using (UnityWebRequest request = new UnityWebRequest(apiBaseUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log($"Уровень создан: {jsonResponse}");

                var createdLevel = JsonUtility.FromJson<UploadLevelData>(jsonResponse);
                onSuccess?.Invoke(createdLevel);
            }
            else
            {
                string error = $"Ошибка создания уровня: {request.error}";
                Debug.LogError(error);
                onError?.Invoke(error);
            }
        }
    }

    public static IEnumerator UploadLevelCoroutine(LevelData levelData, Action<UploadLevelData> onSuccess = null, Action<string> onError = null)
    {
        string serializedLevelData = JsonUtility.ToJson(levelData);

        var levelToUpload = new UploadLevelData
        {
            name = levelData.Name,
            levelData = serializedLevelData,
            description = levelData.Description
        };

        yield return CreateLevelCoroutine(levelToUpload, onSuccess, onError);
    }
}

// Враппер для JSON
[Serializable]
public class LevelListWrapper
{
    public FetchedLevelData[] levels;
}

[Serializable]
public class FetchedLevelData
{
    public string name;
    public string description;
    public LevelData levelData;
    public LevelDifficulty difficulty;
    public int likeCount;
    public int playCount;
}

[Serializable]
public class UploadLevelData
{
    public string name;
    public string description;
    public string levelData;
}