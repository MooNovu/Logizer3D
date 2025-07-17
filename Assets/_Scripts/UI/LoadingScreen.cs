using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private GameObject _blocker;
    private Transform _loadingTransform;
    private Vector2 _leftSide;
    private Vector2 _fillPos;
    private Vector2 _rightSide;
    private readonly float duration = 0.5f;
    private void Awake()
    {
        _loadingScreen.SetActive(true);
        _blocker.SetActive(true);
        _loadingTransform = _loadingScreen.transform;
        float width = Screen.width;// _loadingScreen.GetComponent<RectTransform>().rect.width;
        _fillPos = new(_loadingTransform.position.x, _loadingTransform.position.y);
        _leftSide = new(_loadingTransform.position.x - width, _loadingTransform.position.y);
        _rightSide = new(_loadingTransform.position.x + width, _loadingTransform.position.y);

        UIEvents.OnLoadingScreenAnimationStart += StartAnimation;
    }
    private void OnDestroy()
    {
        UIEvents.OnLoadingScreenAnimationStart -= StartAnimation;
    }
    private void Start()
    {
        EndAnimation();
    }
    public void StartAnimation()
    {
        SetDefaultPos();
        _blocker.SetActive(true);
        _loadingTransform.DOMove(_fillPos, duration).SetEase(Ease.OutExpo);
    }
    public void EndAnimation()
    {
        _loadingTransform.DOMove(_rightSide, duration).SetEase(Ease.InExpo).OnComplete(() => _blocker.SetActive(false));
    }
    public void SetDefaultPos()
    {
        _loadingTransform.position = _leftSide;
    }
    //public void SetProgress(float progress)
    //{

    //}

}