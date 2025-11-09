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

    private Image _image => GetComponent<Image>();

    //void Start()
    //{
    //    _image = GetComponent<Image>();
    //}

    public void Set(string levelName, string author, int downloads, int likes, LevelDifficulty difficulty)
    {
        Debug.Log(_image);
        
        _levelName.text = levelName;
        _author.text = author;
        _downloadsCount.text = downloads.ToString();
        _likesCount.text = likes.ToString();

        SetDifficulty(difficulty);
    }

    private void SetDifficulty(LevelDifficulty difficulty)
    {
        switch (difficulty)
        {
            case LevelDifficulty.Easy:
                _image.sprite = _easySprite;
                break;
            case LevelDifficulty.Normal:
                _image.sprite = _normalSprite;
                break;
            case LevelDifficulty.Hard:
                _image.sprite = _hardSprite;
                break;
            case LevelDifficulty.Insane:
                _image.sprite = _insaneSprite;
                break;
            case LevelDifficulty.Unknown:
                _image.sprite = _unknownSprite;
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
