using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultMenu : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _timesLeftText;
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
        yield return UiAnimator.OpenAnimation().WaitForCompletion();

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
}
