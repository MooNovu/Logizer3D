using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorInterfaceManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _floorButtonsPanel;
    [SerializeField] private GameObject _elementButtonsPanel;
    [SerializeField] private GameObject _buttonPrefab;

    [SerializeField] private Sprite _selectedBtn;
    [SerializeField] private Sprite _defBtn;

    [Header("RedactorModeButton")]
    [SerializeField] private Image _createButton;
    [SerializeField] private Image _editButton;
    [SerializeField] private Image _deleteButton;
    [SerializeField] private Image _rotateButton;

    [Header("Data")]
    [SerializeField] private GridElementsContainer _elementContainer;
    [SerializeField] private GridFloorsContainer _floorContainer;

    private Image _lastFloorBtnPressed;
    private Image _lastElementBtnPressed;

    private bool IsFloorPanelOpen = true;
    private Sequence _swapPanelAnim;
    private RedactorTool lastButton = RedactorTool.Create;
    private void Start()
    {
        SetCreateMode();
        CreateButtons();
    }

    public void SetCreateMode()
    {
        GameEvents.RedactorModeChangeTo(RedactorTool.Create);
        SetLastBtnToDef();
        lastButton = RedactorTool.Create;
        _createButton.sprite = _selectedBtn;
    }
    public void SetEditMode()
    {
        GameEvents.RedactorModeChangeTo(RedactorTool.Edit);
        SetLastBtnToDef();
        lastButton = RedactorTool.Edit;
        _editButton.sprite = _selectedBtn;
    }
    public void SetDeleteMode()
    {
        GameEvents.RedactorModeChangeTo(RedactorTool.Delete);
        SetLastBtnToDef();
        lastButton = RedactorTool.Delete;
        _deleteButton.sprite = _selectedBtn;
    }
    public void SetRotateMode()
    {
        GameEvents.RedactorModeChangeTo(RedactorTool.Rotate);
        SetLastBtnToDef();
        lastButton = RedactorTool.Rotate;
        _rotateButton.sprite = _selectedBtn;
    }
    private void SetLastBtnToDef()
    {
        switch (lastButton)
        {
            case RedactorTool.Create:
                _createButton.sprite = _defBtn;
                break;
            case RedactorTool.Edit:
                _editButton.sprite = _defBtn;
                break;
            case RedactorTool.Delete:
                _deleteButton.sprite = _defBtn;
                break;
            case RedactorTool.Rotate:
                _rotateButton.sprite = _defBtn;
                break;
        }
    }

    public void ToggleElementTypePanel()
    {
        if (IsFloorPanelOpen)
        {
            IsFloorPanelOpen = false;

            RectTransform elRect = _elementButtonsPanel.GetComponent<RectTransform>();
            RectTransform floorRect = _floorButtonsPanel.GetComponent<RectTransform>();

            _swapPanelAnim.Complete();

            _swapPanelAnim = DOTween.Sequence()
                .Append(elRect.DOOffsetMin(new Vector2(16, 16), 0.3f).SetEase(Ease.OutQuad))
                .Join(elRect.DOOffsetMax(new Vector2(-16, 316), 0.3f).SetEase(Ease.OutQuad))
                .Join(floorRect.DOOffsetMin(new Vector2(32, 48), 0.3f).SetEase(Ease.OutQuad))
                .Join(floorRect.DOOffsetMax(new Vector2(-32, 348), 0.3f).SetEase(Ease.OutQuad))
                
                .Join(_floorButtonsPanel.transform.DOScale(1.02f, 0.2f))
                //.Join(_floorButtonsPanel.transform.DORotate(new Vector3(0, 0, 2f), 0.2f))
                .Append(_floorButtonsPanel.transform.DOScale(1f, 0.1f))
                //.Join(_floorButtonsPanel.transform.DORotate(Vector3.zero, 0.1f))

                .OnComplete(() =>
                {
                    _floorButtonsPanel.transform.SetSiblingIndex(0);
                    _elementButtonsPanel.transform.SetSiblingIndex(2);
                });
            GameEvents.RedactorSelectedTypeChangeTo(EditorSelectedType.Elements);
            return;
        }

        IsFloorPanelOpen = true;

        RectTransform elementRT = _elementButtonsPanel.GetComponent<RectTransform>();
        RectTransform floorRT = _floorButtonsPanel.GetComponent<RectTransform>();

        _swapPanelAnim.Complete();

        _swapPanelAnim = DOTween.Sequence()
            .Append(floorRT.DOOffsetMin(new Vector2(16, 16), 0.3f).SetEase(Ease.OutQuad))
            .Join(floorRT.DOOffsetMax(new Vector2(-16, 316), 0.3f).SetEase(Ease.OutQuad))
            .Join(elementRT.DOOffsetMin(new Vector2(32, 48), 0.3f).SetEase(Ease.OutQuad))
            .Join(elementRT.DOOffsetMax(new Vector2(-32, 348), 0.3f).SetEase(Ease.OutQuad))

            .Join(_elementButtonsPanel.transform.DOScale(1.02f, 0.2f))
            //.Join(_elementButtonsPanel.transform.DORotate(new Vector3(0, 0, 2f), 0.2f))
            .Append(_elementButtonsPanel.transform.DOScale(1f, 0.1f))
            //.Join(_elementButtonsPanel.transform.DORotate(Vector3.zero, 0.1f))

            .OnComplete(() =>
            {
                _floorButtonsPanel.transform.SetSiblingIndex(2);
                _elementButtonsPanel.transform.SetSiblingIndex(0);
            });
        GameEvents.RedactorSelectedTypeChangeTo(EditorSelectedType.Floors);
    }

    private void CreateButtons()
    {
        foreach (GridElementTypeSO element in _elementContainer.AllElements)
        {
            GameObject instance = Instantiate(_buttonPrefab, _elementButtonsPanel.transform.position, _elementButtonsPanel.transform.rotation, _elementButtonsPanel.transform.GetChild(0).transform);
            instance.GetComponent<Button>().onClick.AddListener(() => {
                GameEvents.ElementChangeTo(element);
                ElementButtonActive(instance.GetComponent<Image>());
                });
            Image icon = instance.transform.GetChild(0).GetComponent<Image>();

            if (icon != null && element.EditorIcon != null)
            {
                icon.sprite = Sprite.Create(element.EditorIcon,
                            new Rect(0, 0, element.EditorIcon.width, element.EditorIcon.height),
                            Vector2.one * 0.5f);
            }
        }
        foreach (GridFloorTypeSO floor in _floorContainer.AllFloors)
        {
            GameObject instance = Instantiate(_buttonPrefab, _floorButtonsPanel.transform.position, _floorButtonsPanel.transform.rotation, _floorButtonsPanel.transform);
            instance.GetComponent<Button>().onClick.AddListener(() => {
                GameEvents.FloorChangeTo(floor);
                FloorButtonActive(instance.GetComponent<Image>());
                });
            Image icon = instance.GetComponent<Image>();

            if (icon != null && floor.EditorIcon != null)
            {
                icon.sprite = Sprite.Create(floor.EditorIcon,
                            new Rect(0, 0, floor.EditorIcon.width, floor.EditorIcon.height),
                            Vector2.one * 0.5f);
            }
        }
    }
    private void FloorButtonActive(Image image)
    {
        if (_lastFloorBtnPressed != null)
            _lastFloorBtnPressed.sprite = _defBtn;

        image.sprite = _selectedBtn;
        _lastFloorBtnPressed = image;
    }
    private void ElementButtonActive(Image image)
    {
        if (_lastElementBtnPressed != null)
            _lastElementBtnPressed.sprite = _defBtn;

        image.sprite = _selectedBtn;
        _lastElementBtnPressed = image;
    }
}
public static class RectTransformExtensions
{
    public static Tweener DOOffsetMin(this RectTransform rt, Vector2 target, float duration)
    {
        return DOTween.To(
            () => rt.offsetMin,
            x => rt.offsetMin = x,
            target,
            duration
        );
    }

    public static Tweener DOOffsetMax(this RectTransform rt, Vector2 target, float duration)
    {
        return DOTween.To(
            () => rt.offsetMax,
            x => rt.offsetMax = x,
            target,
            duration
        );
    }
}