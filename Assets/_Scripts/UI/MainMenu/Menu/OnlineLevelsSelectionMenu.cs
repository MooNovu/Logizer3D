using UnityEngine;

public class OnlineLevelsSelectionMenu : MonoBehaviour
{
    private UiAnimator _onlineLevelUi => GetComponent<UiAnimator>();
    [SerializeField] private UiAnimator _searchUi;

    public void OpenSearch()
    {
        _searchUi.OpenAnimation();
        _onlineLevelUi.CloseAnimation();
    }
    public void CloseSearch()
    {
        _searchUi.CloseAnimation();
        _onlineLevelUi.OpenAnimation();
    }

}
