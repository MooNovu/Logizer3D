using UnityEngine;
using Zenject;

public class GridSceneInstaller : MonoInstaller
{
    [SerializeField] private GridElementTypeSO[] _elementConfigs;
    [SerializeField] private GridFloorTypeSO[] _floorConfigs;

    [SerializeField] private GridManager _gridManager;
    public override void InstallBindings()
    {
        Container.Bind<GridManager>().FromInstance(_gridManager).AsSingle().NonLazy();

        Container.Bind<GridSystem>()
            .AsSingle()
            .WithArguments(_gridManager.Width, _gridManager.Height, _gridManager.CellSize)
            .NonLazy();

        Container.Bind<GridFactory>()
            .AsSingle()
            .WithArguments(_elementConfigs, _floorConfigs, _gridManager.ElementsParent, _gridManager.FloorParent)
            .NonLazy();

        
        Container.BindFactory<Object, Mover, MoverFactory>().FromFactory<CustomMoverFactory>();

        Container.Bind<LevelInitializer>().AsSingle().NonLazy();
        Container.Bind<SaveLoadManager>().AsSingle().NonLazy();
    }


    private class CustomMoverFactory : IFactory<Object, Mover>
    {
        private readonly DiContainer _container;

        public CustomMoverFactory(DiContainer container)
        {
            _container = container;
        }

        public Mover Create(Object prefab)
        {
            var instance = _container.InstantiatePrefab(prefab);
            return instance.GetComponent<Mover>();
        }
    }
}