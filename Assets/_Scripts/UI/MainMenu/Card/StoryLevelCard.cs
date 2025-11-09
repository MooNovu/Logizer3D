using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryLevelCard : MonoBehaviour
{
    public Button Button;

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _bg;

    [SerializeField] private Image _star1;
    [SerializeField] private Image _star2;
    [SerializeField] private Image _star3;

    [SerializeField] private Sprite _finishedSprite;
    [SerializeField] private Sprite _unfinishedSprite;

    [SerializeField] private Sprite _filledStar;
    [SerializeField] private Sprite _clearStar;

    public void SetText(string text) => _text.text = $"{text}";
    public void SetFinishedLevel(int lvlId, int starsCollected)
    {
        SetText(lvlId.ToString());

        _text.gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 80);

        _bg.sprite = _finishedSprite;

        _star1.gameObject.SetActive(true);
        _star2.gameObject.SetActive(true);
        _star3.gameObject.SetActive(true);
        switch (starsCollected)
        {
            case 0:
                _star1.sprite = _clearStar;
                _star2.sprite = _clearStar;
                _star3.sprite = _clearStar;
                break;
            case 1:
                _star1.sprite = _filledStar;
                _star2.sprite = _clearStar;
                _star3.sprite = _clearStar;
                break;
            case 2:
                _star1.sprite = _filledStar;
                _star2.sprite = _filledStar;
                _star3.sprite = _clearStar;
                break;
            case 3:
                _star1.sprite = _filledStar;
                _star2.sprite = _filledStar;
                _star3.sprite = _filledStar;
                break;
        }
    }
    public void SetUnfinishedLevel(int lvlId)
    {
        SetText(lvlId.ToString());

        _text.gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);

        _bg.sprite = _unfinishedSprite;

        _star1.gameObject.SetActive(false);
        _star2.gameObject.SetActive(false);
        _star3.gameObject.SetActive(false);
    }
}
