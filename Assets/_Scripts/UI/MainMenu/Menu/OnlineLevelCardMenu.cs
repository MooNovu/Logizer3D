using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class OnlineLevelCardMenu : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private TextMeshProUGUI _levelName;
    [SerializeField] private TextMeshProUGUI _author;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _downloadsCount;
    [SerializeField] private TextMeshProUGUI _likesCount;
    [SerializeField] private Button _playBtn;

    [Inject] private readonly ISceneSwitcher sceneSwitcher;

    public void Set(
        string levelName,
        string author,
        string description,
        int downloads,
        int likes,
        LevelData leveldata
        )
    {
        _levelName.text = levelName;
        _author.text = author;
        _description.text = description;
        _downloadsCount.text = downloads.ToString();
        _likesCount.text = likes.ToString();

        _playBtn.onClick.RemoveAllListeners();
        _playBtn.onClick.AddListener(
            () =>
                {
                    CurrentLevelHandler.SetLevel(leveldata);
                    sceneSwitcher.SwitchScene("Game");
                }
            );
    }
}
