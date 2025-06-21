using UnityEngine;

public interface ISceneSwitcher
{
    public float LoadingProgress { get; }
    public void SwitchScene(string sceneName);
}
