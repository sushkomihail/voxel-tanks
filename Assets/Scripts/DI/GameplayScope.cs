using Databases;
using Environment.Map;
using EquipmentSystem;
using Navigation;
using Scenes;
using Spawners;
using Tank;
using UI;
using UnityEngine;
using UpgradeSystem;
using VContainer;
using VContainer.Unity;

namespace DI
{
    public class GameplayScope : LifetimeScope
    {
        [SerializeField] private TanksDatabase _tanksDatabase;
        [SerializeField] private MapGenerator _mapGenerator;
        [SerializeField] private TankCamera _tankCamera;
        [SerializeField] private CollidersUpdater _collidersUpdater;
        [SerializeField] private Pathfinder _pathfinder;
        [SerializeField] private UpgradeManager _upgradeManager;
        [SerializeField] private HealthBar _hudHealthBar;
        [SerializeField] private EquipmentPresenter _equipmentPresenter;
        [SerializeField] private UILoader _uiLoader;
        [SerializeField] private SceneBootstrap _sceneBootstrap;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_tanksDatabase);
            builder.Register<GameInput>(Lifetime.Singleton);
            builder.Register<RouterTargetRegistry>(Lifetime.Singleton).As<IRouterTargetRegistry>();
            builder.Register<PlayerSpawner>(Lifetime.Scoped);
            builder.Register<NPCSpawner>(Lifetime.Scoped);
            builder.Register<UpgradeBroker>(Lifetime.Singleton);

            builder.RegisterComponent(_mapGenerator);
            builder.RegisterComponent(_tankCamera);
            builder.RegisterComponent(_collidersUpdater);
            builder.RegisterComponent(_pathfinder);
            builder.RegisterComponent(_upgradeManager);
            builder.RegisterComponent(_hudHealthBar);
            builder.RegisterComponent(_equipmentPresenter);
            builder.RegisterComponent(_uiLoader);

            builder.RegisterComponent(_sceneBootstrap);
        }
    }
}