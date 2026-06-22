using Databases;
using Navigation;
using Scenes;
using Spawners;
using Tank;
using UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DI
{
    public class GameplayScope : LifetimeScope
    {
        [SerializeField] private TanksDatabase _tanksDatabase;
        [SerializeField] private TankCamera _tankCamera;
        [SerializeField] private CollidersUpdater _collidersUpdater;
        [SerializeField] private Pathfinder _pathfinder;
        [SerializeField] private HealthBar _hudHealthBar;
        [SerializeField] private UILoader _uiLoader;
        [SerializeField] private SceneBootstrap _sceneBootstrap;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_tanksDatabase);
            builder.Register<RouterTargetRegistry>(Lifetime.Singleton).As<IRouterTargetRegistry>();
            builder.Register<PlayerSpawner>(Lifetime.Scoped);
            builder.Register<NPCSpawner>(Lifetime.Scoped);
            
            builder.RegisterComponent(_tankCamera);
            builder.RegisterComponent(_collidersUpdater);
            builder.RegisterComponent(_pathfinder);
            builder.RegisterComponent(_hudHealthBar);
            builder.RegisterComponent(_uiLoader);

            builder.RegisterComponent(_sceneBootstrap);
        }
    }
}