using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnlineLevelCardMenu : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private TextMeshProUGUI _levelName;
    [SerializeField] private TextMeshProUGUI _author;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _downloadsCount;
    [SerializeField] private TextMeshProUGUI _likesCount;
    [SerializeField] private TextMeshProUGUI _size;
    [SerializeField] private Button _playBtn;

    public void OpenWith(
        string levelName,
        string author,
        string description,
        int downloads,
        int likes,
        float size
        )
    {
        _levelName.text = levelName;
        _author.text = author;
        _description.text = description;
        _downloadsCount.text = downloads.ToString();
        _likesCount.text = likes.ToString();
        _size.text = size.ToString();
    }
}
