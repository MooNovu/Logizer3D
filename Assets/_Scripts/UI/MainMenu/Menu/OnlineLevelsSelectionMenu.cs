using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class OnlineLevelsSelectionMenu : MonoBehaviour
{
    private UiAnimator _onlineLevelUi => GetComponent<UiAnimator>();
    [SerializeField] private UiAnimator _searchUi;

    [SerializeField] private UiAnimator _levelCardUi;

    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Transform _parent;

    [SerializeField] private OnlineLevelCardMenu _levelMenu;

    private List<GameObject> _cards = new();

    public void Open()
    {
        if (_cards.Count < 1) FetchLevels();
    }

    private void FetchLevels()
    {
        StartCoroutine(ApiHandle.GetLevelDataCoroutine(
                onSuccess: levels => {
                    CreateCards(levels);
                },
                onError: error => {
                    Debug.LogError(error);
                }
            ));

    }

    private void CreateCards(List<FetchedLevelData> levels)
    {
        foreach (FetchedLevelData level in levels)
        {
            var newCard = GameObject.Instantiate(_cardPrefab, _parent);

            _cards.Add(newCard);
            newCard.GetComponent<OnlineLevelCardList>()
                .Set(level.name, "Имя автора", level.playCount, level.likeCount, level.difficulty, OpenLevel, level.LevelDataObject);
        }
    }

    public void OpenLevel(string levelName,
        string author,
        string description,
        int downloads,
        int likes,
        LevelData levelData
        )
    {

        _levelCardUi.OpenAnimation();
        _onlineLevelUi.CloseAnimation();
        _levelMenu.Set(levelName, author, description, downloads, likes, levelData);
    }
    public void CloseLevel()
    {
        _levelCardUi.CloseAnimation();
        _onlineLevelUi.OpenAnimation();
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
