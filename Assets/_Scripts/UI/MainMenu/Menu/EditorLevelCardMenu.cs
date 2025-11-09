using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EditorLevelCardMenu : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private TextMeshProUGUI _levelName;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private Button _playBtn;
    [SerializeField] private Button _publishBtn;
    [SerializeField] private Button _editBtn;
    [SerializeField] private Button _deleteBtn;

    [Inject] private readonly ISceneSwitcher _sceneSwitcher;

    public void Set(string levelName, string description, Action<string> deleteLevel)
    {
        _levelName.text = levelName;
        _description.text = description;

        ConfigureButton(levelName, deleteLevel);
    }

    private void ConfigureButton(string levelName, Action<string> deleteLevel)
    {
        _playBtn.onClick.RemoveAllListeners();
        _playBtn.onClick.AddListener(
            () =>
                {
                    CurrentLevelHandler.SetLevel(LevelList.GetUserLevel(levelName));
                    _sceneSwitcher.SwitchScene("Game");
                }
            );
        _publishBtn.onClick.RemoveAllListeners();
        //


        _editBtn.onClick.RemoveAllListeners();
        _editBtn.onClick.AddListener(
            () =>
                {
                    CurrentLevelHandler.SetLevel(LevelList.GetUserLevel(levelName));
                    _sceneSwitcher.SwitchScene("Redactor");
                }
            );
        _deleteBtn.onClick.RemoveAllListeners();
        _deleteBtn.onClick.AddListener(
            () =>
                {
                    deleteLevel(levelName);
                }
            );
    }
}
