using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayLevelSelection : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _buttonsPrefab;
    [SerializeField] private TextMeshProUGUI _pageUGUI;

    [Inject] private ISceneSwitcher _sceneSwitcher;
    public int Page { get; private set; }

    private List<GameObject> _buttons = new();
    private const int _itemsOnPageCount = 12;
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
        for (int i = 0; i < _itemsOnPageCount; i++)
        {
            string t = "_";
            Button btn = _buttons[i].GetComponent<Button>();
            btn.onClick.RemoveAllListeners();

            if (LevelCount > ((Page - 1) * _itemsOnPageCount + i))
            {
                int id = (_itemsOnPageCount * (Page - 1)) + i + 1;
                t = $"{id}";
                btn.onClick.AddListener(() => 
                    {
                        CurrentLevelHandler.SetLevel(id);
                        _sceneSwitcher.SwitchScene("Game");
                    }
                );
            }
            _buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = t;
        }
    }
    private void CreateButttons()
    {
        for (int i = 0; i < _itemsOnPageCount; i++)
        {
            GameObject instance = GameObject.Instantiate(_buttonsPrefab, _parent);
            _buttons.Add(instance);
        }
    }
    private void UpdatePageText()
    {
        _pageUGUI.text = Page.ToString();
    }
}
