using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject _bg;
    [SerializeField] private GameObject _blocker;

    private readonly float duration = 0.5f;
    private void Awake()
    {
        UIEvents.OnLoadingScreenAnimationStart += StartAnimation;
        UIEvents.OnLoadingScreenAnimationEnd += EndAnimation;
    }
    private void OnDestroy()
    {
        UIEvents.OnLoadingScreenAnimationStart -= StartAnimation;
        UIEvents.OnLoadingScreenAnimationEnd -= EndAnimation;
    }
    private void Start()
    {
        EndAnimation();
    }
    public void StartAnimation()
    {
        SetActives(true);
        _bg.GetComponent<RectTransform>().DOScale(Vector3.one, duration).From(Vector3.zero);
    }
    public void EndAnimation()
    {
        SetActives(true);
        _bg.GetComponent<RectTransform>().DOScale(Vector3.zero, duration).From(Vector3.one).OnComplete(() => SetActives(false));
    }
    private void SetActives(bool state)
    {
        _bg.SetActive(state);
        _blocker.SetActive(state);
    }
    //public void SetProgress(float progress)
    //{

    //}

}