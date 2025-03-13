using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private SceneSwitcher _sceneSwitcherPrefab;
    [SerializeField] private LoadingScreen _loadingPanel;
    public override void InstallBindings()
    {
        Container.Bind<ILevelList>().To<LevelList>().AsSingle().NonLazy();

        Container.Bind<ISceneSwitcher>()
            .To<SceneSwitcher>()
            .FromInstance(_sceneSwitcherPrefab)
            .AsSingle()
            .NonLazy();

        Container.Bind<ILoadingScreen>()
            .To<LoadingScreen>()
            .FromInstance(_loadingPanel)
            .AsSingle()
            .NonLazy();

        Container.Bind<IStateMachine>().To<GameStateMachine>().AsSingle().NonLazy();
    }
}
