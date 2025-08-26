using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace ShootingSystems
{
    public abstract class ShootingSystem : MonoBehaviour
    {
        [SerializeField] protected Transform _projectilePivot;
        [SerializeField] private ProjectileItem[] _projectileItems;
        
        protected readonly Dictionary<ProjectileType, ObjectPool<Projectile>> _projectilePools = new();
        protected ProjectileType _selectedProjectileType;
        
        private const int ProjectilesPoolDepth = 5;

        public void Init()
        {
            if (_projectileItems.Length != 0)
            {
                _selectedProjectileType = _projectileItems[0].Type;
                
                foreach (ProjectileItem item in _projectileItems)
                {
                    _projectilePools[item.Type] =  
                        new ObjectPool<Projectile>(item.Prefab, InitProjectile, ProjectilesPoolDepth);
                }
            }
        }

        public void OnUpdate()
        {
            
        }

        public void SetSelectedProjectileType(ProjectileType type)
        {
            foreach (var item in _projectileItems)
            {
                if (item.Type == type)
                {
                    _selectedProjectileType = type;
                    break;
                }
            }
            
            Debug.LogError("Failed to set projectile type: type not initialize for this shooting system");
        }

        public float GetProjectileSpeed()
        {
            foreach (var item in _projectileItems)
            {
                if (item.Type == _selectedProjectileType)
                {
                    return item.Props.Speed;
                }
            }

            return 0f;
        }

        public abstract void Shoot();

        public void OnProjectileHit(Projectile projectile)
        {
            _projectilePools[projectile.Type].Release(projectile);
        }

        private void InitProjectile(Projectile projectile)
        {
            foreach (var item in _projectileItems)
            {
                if (item.Type == _selectedProjectileType)
                {
                    projectile.Init(item.Type, item.Props, this);
                    break;
                }
            }
        }
    }
}