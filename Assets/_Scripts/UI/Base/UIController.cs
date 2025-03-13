using ModestTree;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;

public class UIController : MonoBehaviour
{
    private Dictionary<Type, IUIPanel> _panels = new();
    private IUIPanel _currentPanel;

    private void Awake()
    {
        //Кинуть в точку запуска
        DOTween.Init();
        
        RegisterPanels();
        InitializeNavigation();
    }
    private void RegisterPanels()
    {
        foreach (var panel in GetComponentsInChildren<IUIPanel>(true))
        {
            _panels.Add(panel.GetType(), panel);
        }
    }
    private void InitializeNavigation()
    {
        foreach (var panel in _panels.Values.OfType<IUINavigation>())
        {
            panel.OnPanelRequested += HandlePanelRequest;
        }
    }

    public void ShowPanel<T>() where T : IUIPanel
    {
        _currentPanel?.Hide();

        _currentPanel = _panels[typeof(T)];
        _currentPanel.Show();
    }

    private void HandlePanelRequest(Type panelType)
    {
        
        if (_panels.TryGetValue(panelType, out var panel))
        {
            _currentPanel?.Hide();
            panel.Show();
            _currentPanel = panel;
        }
    }
}
