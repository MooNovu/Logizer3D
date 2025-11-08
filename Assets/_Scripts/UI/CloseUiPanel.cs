using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CloseUiPanel : MonoBehaviour
{
    public UnityEvent PanelClosed;
    public UnityEvent<Sequence> PanelClosedSequence;
    public bool CloseOnClick = true;
    private GameObject _backgroundPanel;
    public void CreateBackgroundPanel()
    {
        _backgroundPanel = new GameObject("BackgroundClosePanel");

        var canvas = GetTopmostCanvas(transform);
        _backgroundPanel.transform.SetParent(canvas.transform, false);

        if (canvas.gameObject == this.gameObject)
            _backgroundPanel.transform.SetSiblingIndex(0);
        else
            _backgroundPanel.transform.SetSiblingIndex(transform.GetSiblingIndex());

        var bgRect = _backgroundPanel.AddComponent<RectTransform>();
        var bgImage = _backgroundPanel.AddComponent<Image>();
        var bgButton = _backgroundPanel.AddComponent<Button>();

        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        bgImage.color = new Color(0, 0, 0, 0f);
        bgImage.DOFade(0.4f, 0.25f);
        bgButton.transition = Selectable.Transition.None;

        if (CloseOnClick)
        {
            bgButton.onClick.AddListener(() =>
            {
                bgButton.interactable = false;
                ClosePanel();
            });
        }
    }
    public void ClosePanel()
    {
        Sequence seq = GetComponent<UiAnimator>().CloseAnimation();
        PanelClosed?.Invoke();
        PanelClosedSequence?.Invoke(seq);
    }
    public void DestroyBlackBG()
    {
        if (_backgroundPanel != null)
        {
            _backgroundPanel.GetComponent<Image>().DOFade(0f, 0.25f).OnComplete( () => Destroy(_backgroundPanel));
        }
    }
    private Canvas GetTopmostCanvas(Transform child)
    {
        var canvas = child.GetComponentInParent<Canvas>();
        if (canvas != null && canvas.isRootCanvas)
        {
            return canvas;
        }
        return null;
    }

}
