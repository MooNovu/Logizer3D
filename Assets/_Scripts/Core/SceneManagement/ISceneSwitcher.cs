using UnityEngine;

public interface ISceneSwitcher
{
    public float LoadingProgress { get; }
    public void LoadLevel();
    public void LoadMainMenu();
}
