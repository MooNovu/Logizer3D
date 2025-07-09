using DG.Tweening;
using UnityEngine;
using Zenject;

public class LogoAnimation : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;

    [Inject] private ISceneSwitcher _sceneSwitcher;

    private void Start()
    {
        _canvasGroup.alpha = 0;
    }

    public void StartAnimation()
    {
        _canvasGroup.gameObject.SetActive(true);
        DOTween.Sequence()
            .Append(_canvasGroup.DOFade(1, 1f).SetEase(Ease.OutQuad))
            .AppendInterval(1f)
            .Append(_canvasGroup.DOFade(0, 1f).SetEase(Ease.OutQuad))
            .AppendCallback(() => _sceneSwitcher.SwitchScene("MainMenu"));
    }
}
