using System.Runtime.CompilerServices;
using UnityEngine;

public class SafeAreaOffsetApplier : MonoBehaviour
{
    [SerializeField] private float MinimumOffset = 0f;
    private void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        float offset = GetTopSafeAreaOffset();
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -(offset > MinimumOffset ? offset : MinimumOffset));
    }

    private float GetTopSafeAreaOffset()
    {
        Rect safeArea = Screen.safeArea;

        float topOffset = Screen.height - safeArea.yMax;

        if (topOffset < 0) topOffset = 0;

        return topOffset;
    }
}
