using UnityEngine;
using Zenject;

public class PlaySceneInstaller : MonoInstaller
{
    [SerializeField] private Transform _elementsParent;
    [SerializeField] private Transform _floorParent;

    [SerializeField] private GridElementsContainer _elementsContainer;
    [SerializeField] private GridFloorsContainer _floorsContainer;
    [SerializeField] private PlayerSkinsContainer _skinsContainer;

    [SerializeField] private MobileInput _mobileInput;

    [SerializeField] private GameCanvasManager _gameCanvas;

    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private CameraController _cameraController;

    public override void InstallBindings()
    {
        Container.Bind<GridSystem>().AsSingle().WithArguments(20, 20).NonLazy();

        Container.Bind<GridFactory>().AsSingle().WithArguments(_elementsContainer, _floorsContainer, _skinsContainer, _elementsParent, _floorParent).NonLazy();

        Container.Bind<IInputProvider>().FromInstance(_mobileInput).AsSingle().NonLazy();

        Container.Bind<GameCanvasManager>().FromInstance(_gameCanvas).AsSingle().NonLazy();

        Container.Bind<LevelLoader>().FromInstance(_levelLoader).AsSingle().NonLazy();

        Container.Bind<CameraController>().FromInstance(_cameraController).AsSingle().NonLazy();

        Container.Bind<LoadManager>().AsSingle();
    }
}