using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultMenu : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _stepsText;

    [Header("Stars")]
    [SerializeField] private List<GameObject> _filledStars;

    private UiAnimator UiAnimator => GetComponent<UiAnimator>();
    public void OpenAnimation() => StartCoroutine(Open());
    public void CloseAnimation() => UiAnimator.CloseAnimation();
    private IEnumerator Open()
    {
        for (int i = 0; i < _filledStars.Count; i++)
        {
            _filledStars[i].SetActive(false);
        }
        _timeText.text = "0:00";
        _stepsText.text = "0";

        yield return UiAnimator.OpenAnimation().WaitForCompletion();

        _timeText.text = FloatToTime(GameEvents.GetTimeDifference());
        _stepsText.text = GameEvents.StepsDone.ToString();

        for (int i = 0; i < GameEvents.CandiesCollected; i++)
        {
            _filledStars[i].SetActive(true);
            yield return StarAnimation(_filledStars[i]).WaitForCompletion();
        }
    }


    private Sequence StarAnimation(GameObject star)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(star.transform.DOScale(1f, 0.25f).From(3f));
        seq.Join(star.GetComponent<Image>().DOFade(1f, 0.25f).From(0f));
        return seq;
    }

    private string FloatToTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60) % 60;
        int seconds = Mathf.FloorToInt(time % 60);
        
        return $"{minutes}:{seconds:00}";
    }
}
