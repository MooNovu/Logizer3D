using UnityEngine;
using Zenject;

public class GridSceneInstaller : MonoInstaller
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private SaveManager saveManager;
    public override void InstallBindings()
    {
        Container.Bind<GridManager>().FromInstance(gridManager);
        Container.Bind<SaveManager>().FromInstance(saveManager);
    }
}