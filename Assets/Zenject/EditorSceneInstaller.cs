using UnityEngine;
using Zenject;

public class EditorSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GridSystem>().AsSingle().WithArguments(20, 20).NonLazy();

        Container.Bind<FromSceneLoader>().AsSingle().NonLazy();

        Container.Bind<SaveManager>().AsSingle();
    }
}
