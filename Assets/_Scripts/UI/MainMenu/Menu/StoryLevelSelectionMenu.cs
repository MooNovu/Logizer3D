using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class StoryLevelSelectionMenu : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _levelPrefab;
    [SerializeField] private TextMeshProUGUI _pageUGUI;

    [Inject] private ISceneSwitcher _sceneSwitcher;
    public int Page { get; private set; }

    private List<StoryLevelCard> _buttons = new();
    private const int _itemsOnPageCount = 9;
    private int LevelCount => LevelList.GetLevelsCount();

    private void Start()
    {
        Page = 1;
        CreateButttons();
        ReloadButtons();
        UpdatePageText();
    }
    public void NextPage()
    {
        if (_itemsOnPageCount * (Page) >= LevelCount) return;
        Page++;
        ReloadButtons();
        UpdatePageText();
    }
    public void PreviousPage()
    {
        if (Page <= 1) return;
        Page--;
        ReloadButtons();
        UpdatePageText();
    }

    private void ReloadButtons()
    {
        StartCoroutine(ReloadButtonsSequence());
    }
    private IEnumerator ReloadButtonsSequence()
    {
        for (int i = 0; i < _itemsOnPageCount; i++)
        {
            int index = i;
            RectTransform t = _buttons[index].gameObject.GetComponent<RectTransform>();

            t.DOComplete();

            t.DOScale(Vector3.one, 0.25f).From(Vector3.zero);
            ReloadButton(index);

            yield return null;
        }
    }
    private void ReloadButton(int i) 
    {
        StoryLevelCard btn = _buttons[i];
        btn.Button.onClick.RemoveAllListeners();

        int id = (_itemsOnPageCount * (Page - 1)) + i + 1;

        if (LevelCount <= ((Page - 1) * _itemsOnPageCount + i))
        {
            btn.SetUnfinishedLevel(id);
            btn.SetText("");
            return;
        }

        int? stars = ProgressSaver.IsLevelCompleted(id);
        if (stars != null)
            btn.SetFinishedLevel(id, (int)stars);

        else
            btn.SetUnfinishedLevel(id);

        btn.Button.onClick.AddListener(() =>
            {
                CurrentLevelHandler.SetLevel(id);
                _sceneSwitcher.SwitchScene("Game");
            }
        );
    }
    private void CreateButttons()
    {
        for (int i = 0; i < _itemsOnPageCount; i++)
        {
            GameObject instance = GameObject.Instantiate(_levelPrefab, _parent);
            _buttons.Add(instance.GetComponent<StoryLevelCard>());
        }
    }
    private void UpdatePageText()
    {
        _pageUGUI.text = Page.ToString();
    }
}
