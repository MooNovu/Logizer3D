using UnityEngine;
using UnityEngine.UI;

public class CloseUiPanel : MonoBehaviour
{
    private GameObject _backgroundPanel;
    public void CreateBackgroundPanel()
    {
        _backgroundPanel = new GameObject("BackgroundClosePanel");

        var canvas = GetTopmostCanvas(transform);
        _backgroundPanel.transform.SetParent(canvas.transform, false);

        _backgroundPanel.transform.SetSiblingIndex(transform.GetSiblingIndex());

        var bgRect = _backgroundPanel.AddComponent<RectTransform>();
        var bgImage = _backgroundPanel.AddComponent<Image>();
        var bgButton = _backgroundPanel.AddComponent<Button>();

        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        bgImage.color = new Color(0, 0, 0, 0.2f);


        bgButton.onClick.AddListener(() => 
        {
            UIAnimationHandler.CloseAnimation(gameObject);
        });
    }
    public void DestroyPanel()
    {
        if (_backgroundPanel != null)
        {
            Destroy(_backgroundPanel);
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
