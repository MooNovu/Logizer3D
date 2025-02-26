using System;

public interface IUIPanel
{
    void Show();
    void Hide();
    void HideImmediate();
    bool IsVisible { get; }
}

public interface IUINavigation
{
    event Action<Type> OnPanelRequested;
}



