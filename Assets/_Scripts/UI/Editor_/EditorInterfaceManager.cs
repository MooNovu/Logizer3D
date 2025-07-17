using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EditorInterfaceManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _floorButtonsPanel;
    [SerializeField] private GameObject _elementButtonsPanel;
    [SerializeField] private GameObject _buttonPrefab;

    [Header("RedactorModeButton")]
    [SerializeField] private GameObject _redactorModeButton;
    [SerializeField] private Texture2D _createImage;
    [SerializeField] private Texture2D _editImage;
    [SerializeField] private Texture2D _deleteImage;
    private Image _redactorButtonImage;

    [Header("Data")]
    [SerializeField] private GridElementsContainer _elementContainer;
    [SerializeField] private GridFloorsContainer _floorContainer;

    private void Start()
    {
        _floorButtonsPanel.SetActive(true);
        _elementButtonsPanel.SetActive(true);
        UIAnimationHandler.OpenAnimation(_floorButtonsPanel, true);
        UIAnimationHandler.CloseAnimation(_elementButtonsPanel, true);

        _redactorButtonImage = _redactorModeButton.GetComponent<Image>();
        CreateButtons();
    }

    public void ShowElementPanel()
    {
        UIAnimationHandler.CloseAnimation(_floorButtonsPanel);

        UIAnimationHandler.OpenAnimation(_elementButtonsPanel);
    }
    public void ShowFloorPanel()
    {
        UIAnimationHandler.CloseAnimation(_elementButtonsPanel);

        UIAnimationHandler.OpenAnimation(_floorButtonsPanel);
    }

    public void RedactorButtonMode(RedactorTool toolType)
    {
        switch (toolType)
        {
            case RedactorTool.Create:
                _redactorButtonImage.sprite = Sprite.Create(_createImage, 
                    new Rect(0, 0, _createImage.width, _createImage.height), 
                    Vector2.one * 0.5f);
                break;
            case RedactorTool.Edit:
                _redactorButtonImage.sprite = Sprite.Create(_editImage,
                    new Rect(0, 0, _editImage.width, _editImage.height),
                    Vector2.one * 0.5f);
                break;
            case RedactorTool.Delete:
                _redactorButtonImage.sprite = Sprite.Create(_deleteImage,
                    new Rect(0, 0, _deleteImage.width, _deleteImage.height),
                    Vector2.one * 0.5f);
                break;
        }
    }

    private void CreateButtons()
    {
        foreach (GridElementTypeSO element in _elementContainer.AllElements)
        {
            GameObject instance = Instantiate(_buttonPrefab, _elementButtonsPanel.transform.position, _elementButtonsPanel.transform.rotation, _elementButtonsPanel.transform);
            instance.GetComponent<Button>().onClick.AddListener(() => GameEvents.ElementChangeTo(element));
            Image icon = instance.GetComponent<Image>();

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
            instance.GetComponent<Button>().onClick.AddListener(() => GameEvents.FloorChangeTo(floor));
            Image icon = instance.GetComponent<Image>();

            if (icon != null && floor.EditorIcon != null)
            {
                icon.sprite = Sprite.Create(floor.EditorIcon,
                            new Rect(0, 0, floor.EditorIcon.width, floor.EditorIcon.height),
                            Vector2.one * 0.5f);
            }
        }
    }
}
