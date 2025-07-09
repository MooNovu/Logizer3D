using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour, ILoadingScreen
{
    [SerializeField] private GameObject _loadingScreen;
    private Transform _loadingTransform;
    private Vector2 _leftSide;
    private Vector2 _fillPos;
    private Vector2 _rightSide;
    private readonly float duration = 0.5f;
    private void Awake()
    {
        _loadingTransform = _loadingScreen.transform;
        float width = _loadingScreen.GetComponent<RectTransform>().rect.width;
        _fillPos = new(_loadingTransform.position.x, _loadingTransform.position.y);
        _leftSide = new(_loadingTransform.position.x - width, _loadingTransform.position.y);
        _rightSide = new(_loadingTransform.position.x + width, _loadingTransform.position.y);
        SetDefaultPos();
    }

    public void StartAnimation()
    {
        SetDefaultPos();
        _loadingTransform.DOMove(_fillPos, duration).SetEase(Ease.InQuad);
    }
    public void EndAnimation()
    {
        _loadingTransform.DOMove(_rightSide, duration).SetEase(Ease.InQuad);
    }
    public void SetDefaultPos()
    {
        _loadingTransform.position = _leftSide;
    }
    public void SetProgress(float progress)
    {

    }

}

public interface ILoadingScreen
{
    public void StartAnimation();
    public void EndAnimation();
    public void SetDefaultPos();
    public void SetProgress(float progress);
}