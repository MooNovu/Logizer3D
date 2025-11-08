using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEditor.UIElements;

public class UiAnimator : MonoBehaviour
{
    public enum AnimationType
    {
        SlideFromBottom,
        SlideFromTop,
        SlideFromLeft,
        SlideFromRight,
        Pulse,
        Splash
    }

    [Header("Ui Elements to Animate")]
    [SerializeField] private RectTransform[] _uiElements;

    [Header("Settings")]
    private float AnimationDuration = 0.5f;
    [SerializeField] private bool IsHideByDefault = true;
    [SerializeField] private AnimationType openAnimation = AnimationType.SlideFromBottom;
    [SerializeField] private AnimationType closeAnimation = AnimationType.SlideFromBottom;

    [Header("Initial Open")]
    [SerializeField] private bool InitialOpenAnimation = false;
    [SerializeField] private float InitialOpenDelay = 0.5f;

    private Dictionary<RectTransform, Vector2> _originalPositions;

    private CloseUiPanel _closePanel = null;
    private CanvasGroup _canvasGroup;

    private void Start()
    {

        if (TryGetComponent(out CloseUiPanel cp)) _closePanel = cp;
        
        if (TryGetComponent(out CanvasGroup cg)) _canvasGroup = cg;

        if (InitialOpenAnimation) IsHideByDefault = true;

        SetStartPositions();

        if (InitialOpenAnimation)
            DOTween.Sequence().SetDelay(InitialOpenDelay).OnComplete(() => OpenAnimation());
    }
    public Sequence OpenAnimation() => OpenAnimation(openAnimation);
    public Sequence OpenAnimation(AnimationType animType)
    {
        if (_closePanel != null) _closePanel.CreateBackgroundPanel();
        Sequence seq;
        switch (animType)
        {
            case AnimationType.SlideFromBottom:
                seq = SlideFrom(Vector2.down, true);
                break;
            case AnimationType.SlideFromTop:
                seq = SlideFrom(Vector2.up, true);
                break;
            case AnimationType.SlideFromLeft:
                seq = SlideFrom(Vector2.left, true);
                break;
            case AnimationType.SlideFromRight:
                seq = SlideFrom(Vector2.right, true);
                break;
            case AnimationType.Pulse:
                seq = Pulse(true);
                break;
            case AnimationType.Splash:
                seq = Splash(true);
                break;
            default:
                seq = SlideFrom(Vector2.down, true);
                break;
        }
        return seq;
    }
    public Sequence CloseAnimation() => CloseAnimation(closeAnimation);
    public Sequence CloseAnimation(AnimationType animType)
    {
        if (_closePanel != null) _closePanel.DestroyBlackBG();
        Sequence seq;
        switch (animType)
        {
            case AnimationType.SlideFromBottom:
                seq = SlideFrom(Vector2.down, false);
                break;
            case AnimationType.SlideFromTop:
                seq = SlideFrom(Vector2.up, false);
                break;
            case AnimationType.SlideFromLeft:
                seq = SlideFrom(Vector2.left, false);
                break;
            case AnimationType.SlideFromRight:
                seq = SlideFrom(Vector2.right, false);
                break;
            case AnimationType.Pulse:
                seq = Pulse(false);
                break;
            case AnimationType.Splash:
                seq = Splash(false);
                break;
            default:
                seq = SlideFrom(Vector2.down, false);
                break;
        }
        return seq;
    }
    private Sequence SlideFrom(Vector2 side, bool isOpening)
    {
        if (isOpening == false && _canvasGroup != null) _canvasGroup.interactable = false;
        Sequence seq = DOTween.Sequence();
        for (int i = 0; i < _uiElements.Length; i++)
        {
            int elementIndex = isOpening ? i : _uiElements.Length - 1 - i;
            RectTransform element = _uiElements[elementIndex];

            element.DOKill();

            Vector2 hiddenPos = _originalPositions[element] + new Vector2(side.x * Screen.width, side.y * Screen.height);
            if (isOpening)
            {
                element.anchoredPosition = hiddenPos;
                element.DOScale(1, 0);
            }

            Vector2 endPos = isOpening ? _originalPositions[element] : hiddenPos;

            seq = DOTween.Sequence();

            seq.Append(element.DOAnchorPos(endPos, AnimationDuration)
                .SetDelay(i * 0.1f)
                .SetEase(isOpening ? Ease.OutBack : Ease.InBack, 1f));

            if (isOpening == true && i == _uiElements.Length - 1)
            {
                seq.OnComplete(() => { if (_canvasGroup != null) _canvasGroup.interactable = true; });
            }
        }
        return seq;
    }
    private Sequence Pulse(bool isOpening)
    {
        if (isOpening == false && _canvasGroup != null) _canvasGroup.interactable = false;
        Sequence seq = DOTween.Sequence();
        for (int i = 0; i < _uiElements.Length; i++)
        {
            int elementIndex = isOpening ? i : _uiElements.Length - 1 - i;
            RectTransform element = _uiElements[elementIndex];

            _uiElements[i].DOKill();

            seq = DOTween.Sequence();
            if (isOpening)
            {
                _uiElements[i].anchoredPosition = _originalPositions[element];
                seq.Append(element.DOScale(1.1f, AnimationDuration / 2).SetDelay(i * 0.1f).SetEase(Ease.OutQuad).From(0));
                seq.Append(element.DOScale(1.0f, AnimationDuration / 2).SetEase(Ease.InQuad));
            }
            else
            {
                seq.Append(element.DOScale(1.1f, AnimationDuration / 2).SetDelay(i * 0.1f).SetEase(Ease.OutQuad));
                seq.Append(element.DOScale(0f, AnimationDuration / 2).SetEase(Ease.InQuad));

                Vector2 hiddenPos = _originalPositions[element] + new Vector2(0, -Screen.height);
                seq.Append(element.DOAnchorPos(hiddenPos, 0));
            }

            if (isOpening == true && i == _uiElements.Length - 1)
            {
                seq.OnComplete(() => { if (_canvasGroup != null) _canvasGroup.interactable = true; });
            }
        }
        return seq;
    }
    private Sequence Splash(bool isOpening)
    {
        if (isOpening == false && _canvasGroup != null) _canvasGroup.interactable = false;
        Sequence seq = DOTween.Sequence();
        for (int i = 0; i < _uiElements.Length; i++)
        {
            int elementIndex = isOpening ? i : _uiElements.Length - 1 - i;
            RectTransform element = _uiElements[elementIndex];

            _uiElements[i].DOKill();

            seq = DOTween.Sequence();
            if (isOpening)
            {
                _uiElements[i].anchoredPosition = _originalPositions[element];
                seq.Append(element.DOScale(1f, AnimationDuration).SetDelay(0f).SetEase(Ease.OutQuad).From(1.5f));
            }
            else
            {
                seq.Append(element.DOScale(1.5f, AnimationDuration).SetEase(Ease.InQuad));

                Vector2 hiddenPos = _originalPositions[element] + new Vector2(0, -Screen.height);
                seq.Append(element.DOAnchorPos(hiddenPos, 0));
            }
            if (isOpening == false && i == 0) _canvasGroup.DOFade(0f, AnimationDuration).From(1f);
            if (isOpening == true && i == _uiElements.Length - 1)
            {
                _canvasGroup.DOFade(1f, AnimationDuration).From(0f);
                seq.OnComplete(() => { if (_canvasGroup != null) _canvasGroup.interactable = true; });
            }
        }
        return seq;
    }
    private void SetStartPositions()
    {
        _originalPositions = new();
        for (int i = 0; i < _uiElements.Length; i++)
        {
            _originalPositions.Add(_uiElements[i], _uiElements[i].anchoredPosition);
            if (IsHideByDefault)
            {
                _uiElements[i].anchoredPosition = new Vector2(_uiElements[i].anchoredPosition.x, _uiElements[i].anchoredPosition.y - Screen.height);
            }
        }
        _canvasGroup.interactable = false;
    }
}
