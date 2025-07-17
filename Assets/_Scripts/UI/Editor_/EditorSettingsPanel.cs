using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class EditorSettingsPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _parameterPrefab;

    private List<UiParameter> _params = new();

    private const int _maxParamsCount = 2;
    private void InitializeParams()
    {
        for (int i = 0; i < _maxParamsCount; i++)
        {
            GameObject p = Instantiate(_parameterPrefab, _parent);
            UiParameter param = new(p);
            _params.Add(param);
            param.Clear();
            param.SetActive(false);
        }
    }
    public void OpenSettings(IFloor floor)
    {
        if (floor == null) return;
        ClearParams();

    }
    public void OpenSettings(IGridElement element)
    {
        if (element == null || element is not IEditable editable) return;
        if (_params.Count < 1) InitializeParams();
        ClearParams();
        ShowPanel();

        _title.text = editable.EditorTitle;

        List<EditorParameter> parametres = editable.GetEditorParameters();

        for (int i = 0; i < parametres.Count; i++)
        {
            _params[i].SetActive(true);
            _params[i].ChangeTextTo(parametres[i].Name);
            _params[i].SetToInputField(parametres[i].Value);
            _params[i].SetValueChangedCallback(parametres[i].OnChanged);
        }
    }
    private void ShowPanel()
    {
        UIAnimationHandler.OpenAnimation(gameObject);
    }
    public void ClosePanel()
    {
        UIAnimationHandler.CloseAnimation(gameObject);
        ClearParams();
    }
    private void ClearParams()
    {
        foreach (UiParameter p in _params)
        {
            p.Clear();
            p.SetActive(false);
        }
    }
    private class UiParameter
    {
        private readonly GameObject _parameter;
        private readonly TextMeshProUGUI textTmp;
        private readonly TMP_InputField inputTmp;
        private Action<int> _onValueChanged;
        public UiParameter(GameObject parameter)
        {
            _parameter = parameter;
            textTmp = _parameter.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            inputTmp = _parameter.transform.GetChild(1).GetComponentInChildren<TMP_InputField>();

            inputTmp.onEndEdit.AddListener(OnInputChanged);
        }
        public void SetActive(bool active)
        {
            _parameter.SetActive(active);
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
}
