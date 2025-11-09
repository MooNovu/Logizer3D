using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class OnlineLevelsSelectionMenu : MonoBehaviour
{
    private UiAnimator _onlineLevelUi => GetComponent<UiAnimator>();
    [SerializeField] private UiAnimator _searchUi;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Transform _parent;

    public void Start()
    {
        CreateCards();
    }

    private void CreateCards()
    {
        StartCoroutine(ApiHandle.GetLevelDataCoroutine(
                onSuccess: levels => {
                    Debug.Log($"Получено уровней: {levels.Count}");
                    CreateCardsForSure(levels);
                    
                },
                onError: error => {
                    Debug.LogError(error);
                }
            ));

    }

    private void CreateCardsForSure(List<FetchedLevelData> levels)
    {
        foreach (FetchedLevelData level in levels)
        {
            var newCard = GameObject.Instantiate(_cardPrefab, _parent);
            newCard.GetComponent<OnlineLevelCardList> ().Set(level.name, "Имя автора", level.playCount, level.likeCount, level.difficulty);
        }
    }

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
