using System;
using TMPro;
using DG.Tweening;
using UnityEngine;

public class EditorParameter
{
    private GameObject _parameter;
    private TextMeshProUGUI textTmp;
    private TMP_InputField inputTmp;
    private Action<int> _onValueChanged;
    public EditorParameter(GameObject parameter)
    {
        _parameter = parameter;
        textTmp = _parameter.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        inputTmp = _parameter.transform.GetChild(1).GetComponentInChildren<TMP_InputField>();

        inputTmp.onEndEdit.AddListener(OnInputChanged);
    }
    public void SetValueChangedCallback(Action<int> callback)
    {
        _onValueChanged = callback;
    }
    private void OnInputChanged(string value)
    {
        if (int.TryParse(value, out int result))
        {
            _onValueChanged?.Invoke(result);
        }
    }
    public void ChangeTextTo(string text)
    {
        textTmp.text = text;
    }
    public void SetToInputField(int id)
    {
        inputTmp.text = $"{id}";
    }
    public void Clear()
    {
        textTmp.text = "Parameter";
        inputTmp.text = "0";
        _onValueChanged = null;
    }
}

public class EditorSettings : MonoBehaviour
{
    private CanvasGroup panelCanvasGroup;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private GameObject _p1;
    [SerializeField] private GameObject _p2;
    [SerializeField] private GameObject _p3;

    private EditorParameter _parameter1;
    private EditorParameter _parameter2;
    private EditorParameter _parameter3;

    private void Start()
    {
        gameObject.SetActive(true);
        panelCanvasGroup = GetComponent<CanvasGroup>();
        panelCanvasGroup.alpha = 0;
        panelCanvasGroup.blocksRaycasts = false;

        _parameter1 = new(_p1);
        _parameter2 = new(_p2);
        _parameter3 = new(_p3);

        _parameter1.Clear();
        _parameter2.Clear();
        _parameter3.Clear();
        _p1.SetActive(false);
        _p2.SetActive(false);
        _p3.SetActive(false);
    }

    public void OpenSettings(IFloor floor)
    {
        if (floor == null) return;
        ClearParams();

    }
    public void OpenSettings(IGridElement element)
    {
        if (element == null) return;
        ClearParams();

        if (element is Portal portal)
        {
            ShowPanel();
            _p1.SetActive(true);
            _p2.SetActive(true);
            _title.text = "Portal";
            _parameter1.ChangeTextTo("This Portal ID");
            _parameter1.SetToInputField(portal.PortalId);
            _parameter1.SetValueChangedCallback( (int id) => portal.PortalId = id );
            _parameter2.ChangeTextTo("Target Portal ID");
            _parameter2.SetToInputField(portal.TargetPortalId);
            _parameter2.SetValueChangedCallback((int id) => portal.TargetPortalId = id);
        }
    }

    private void ShowPanel()
    {
        panelCanvasGroup.DOFade(1f, 0.1f).SetEase(Ease.InQuad);
        gameObject.transform.DOScale(new Vector3(1, 1, 1), 0.1f).SetEase(Ease.InQuad);
        panelCanvasGroup.blocksRaycasts = true;
    }

    public void ClosePanel()
    {
        panelCanvasGroup.DOFade(0f, 0.1f).SetEase(Ease.InQuad);
        gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.1f).SetEase(Ease.InQuad);
        panelCanvasGroup.blocksRaycasts = false;
        ClearParams();
    }
    private void ClearParams()
    {
        _parameter1.Clear();
        _parameter2.Clear();
        _parameter3.Clear();
        _p1.SetActive(false);
        _p2.SetActive(false);
        _p3.SetActive(false);
    }
}
