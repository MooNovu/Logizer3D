using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using System;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject _bg;
    [SerializeField] private GameObject _blocker;

    private Material _transitionMaterial;
    private Material TransitionMaterial
    {
        get
        {
            if (_transitionMaterial == null)
            {
                Image image = _bg.GetComponent<Image>();
                if (image != null)
                    _transitionMaterial = image.material;
            }
            return _transitionMaterial;
        }
    }

    private readonly float duration = 0.4f;
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
    public Sequence StartAnimation()
    {
        SetActives(true);
        TransitionMaterial.SetFloat("_Progress", 0f);
        TransitionMaterial.SetFloat("_IsOpen", 1f);
        Sequence seq = DOTween.Sequence();
        seq.Append(DOVirtual.Float(0f, 2f, duration, value =>
        {
            TransitionMaterial.SetFloat("_Progress", value);
        }).SetDelay(0.1f).SetEase(Ease.InExpo));
        return seq;
    }
    public Sequence EndAnimation()
    {
        SetActives(true);
        TransitionMaterial.SetFloat("_Progress", 0f);
        TransitionMaterial.SetFloat("_IsOpen", 0f);
        Sequence seq = DOTween.Sequence();
        seq.Append(DOVirtual.Float(0f, 2f, duration, value =>
        {
            TransitionMaterial.SetFloat("_Progress", value);
        }).SetDelay(0.1f).SetEase(Ease.InExpo).OnComplete(() => SetActives(false)));

        return seq;
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