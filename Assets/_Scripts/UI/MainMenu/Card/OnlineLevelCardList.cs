using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnlineLevelCardList : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private TextMeshProUGUI _levelName;
    [SerializeField] private TextMeshProUGUI _author;
    [SerializeField] private TextMeshProUGUI _downloadsCount;
    [SerializeField] private TextMeshProUGUI _likesCount;
    [SerializeField] private Button _viewButton;

    [Header("Sprites")]
    [SerializeField] private Sprite _easySprite;
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _hardSprite;
    [SerializeField] private Sprite _insaneSprite;
    [SerializeField] private Sprite _unknownSprite;

    private LevelData _levelData;

    private Image Image => GetComponent<Image>();
    public void Set(
        string levelName, 
        string author, 
        int downloads, 
        int likes, 
        LevelDifficulty difficulty,
        Action<string, string, string, int, int, LevelData> view,
        LevelData levelData
        )
    {
        _levelName.text = levelName;
        _author.text = author;
        _downloadsCount.text = downloads.ToString();
        _likesCount.text = likes.ToString();
        _levelData = levelData;

        _viewButton.onClick.RemoveAllListeners();
        _viewButton.onClick.AddListener(
            () =>
                {
                    view(levelName, author, levelData.Description, downloads, likes, levelData);
                }
            );

        SetDifficulty(difficulty);
    }

    private void SetDifficulty(LevelDifficulty difficulty)
    {
        switch (difficulty)
        {
            case LevelDifficulty.Easy:
                Image.sprite = _easySprite;
                break;
            case LevelDifficulty.Normal:
                Image.sprite = _normalSprite;
                break;
            case LevelDifficulty.Hard:
                Image.sprite = _hardSprite;
                break;
            case LevelDifficulty.Insane:
                Image.sprite = _insaneSprite;
                break;
            case LevelDifficulty.Unknown:
                Image.sprite = _unknownSprite;
                break;
        }
    }
}

public enum LevelDifficulty
{
    Unknown,
    Easy,
    Normal,
    Hard,
    Insane
}
