using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private SceneSwitcher _sceneSwitcherPrefab;
    public override void InstallBindings()
    {
        Container.Bind<ISceneSwitcher>()
            .To<SceneSwitcher>()
            .FromInstance(_sceneSwitcherPrefab)
            .AsSingle()
            .NonLazy();

        Container.Bind<IStateMachine>().To<GameStateMachine>().AsSingle().NonLazy();
    }
}
