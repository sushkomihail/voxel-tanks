using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Projectiles
{
    public class ProjectilesUpdater : MonoBehaviour
    {
        [SerializeField] private Tracer _tracerPrefab;
        
        public static ProjectilesUpdater Instance { get; private set; }

        private const int TracersPoolDepth = 10;
            
        private readonly Dictionary<Projectile, Tracer> _activeProjectiles = new();
        private readonly List<Projectile> _inactiveProjectiles = new();
        private ObjectPool<Tracer> _tracersPool;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            _tracersPool = new ObjectPool<Tracer>(_tracerPrefab, InitializeTracer, TracersPoolDepth);
        }
        
        private void FixedUpdate()
        {
            _inactiveProjectiles.Clear();
            
            foreach ((Projectile projectile, Tracer tracer) in _activeProjectiles)
            {
                projectile.Update(Time.fixedDeltaTime);
                tracer.UpdateLocation(projectile.CurrentPosition, projectile.Velocity);

                if (projectile.IsInactive)
                {
                    _inactiveProjectiles.Add(projectile);
                }
            }

            foreach (Projectile projectile in _inactiveProjectiles)
            {
                Tracer tracer = _activeProjectiles[projectile];
                tracer.SetEnabled(false);
                _tracersPool.Release(tracer);
                _activeProjectiles.Remove(projectile);
            }
        }

        public void AddProjectile(Projectile projectile)
        {
            Tracer tracer = _tracersPool.Get();
            tracer.Initialize(projectile.Type, projectile.Scale);
            tracer.UpdateLocation(projectile.CurrentPosition, projectile.Velocity);
            tracer.SetEnabled(true);
            _activeProjectiles.Add(projectile, tracer);
        }

        private static void InitializeTracer(Tracer tracer)
        {
            tracer.SetEnabled(false);
        }
    }
}