using System.Collections.Generic;
using ShootingSystems.Data;
using Tools;
using UnityEngine;

namespace ShootingSystems
{
    public abstract class ShootingSystem : MonoBehaviour
    {
        [SerializeField] protected Transform _projectilePivot;
        [SerializeField] private ProjectilesData _projectilesData;
        [SerializeField] private int _projectilePoolDepth = 5;
        
        protected readonly Dictionary<ProjectileType, ObjectPool<Projectile>> _projectilePools = new();
        protected ProjectileType _selectedProjectileType;
        
        public void Initialize()
        {
            if (_projectilesData.Items.Count == 0) return;
            
            _selectedProjectileType = _projectilesData.Items[0].Type;
            
            foreach (ProjectileItem item in _projectilesData.Items)
            {
                _projectilePools[item.Type] =  
                    new ObjectPool<Projectile>(item.Prefab, InitProjectile, _projectilePoolDepth);
            }
        }

        public void OnUpdate()
        {
            
        }

        public void SetSelectedProjectileType(ProjectileType type)
        {
            foreach (var item in _projectilesData.Items)
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
            foreach (var item in _projectilesData.Items)
            {
                if (item.Type == _selectedProjectileType)
                {
                    return item.Props.Speed;
                }
            }

            return 0f;
        }

        public int GetProjectilePenetration()
        {
            foreach (var item in _projectilesData.Items)
            {
                if (item.Type == _selectedProjectileType)
                {
                    return item.Props.Penetration;
                }
            }

            return -1;
        }

        public abstract void Shoot();

        public void OnProjectileHit(Projectile projectile)
        {
            _projectilePools[projectile.Type].Release(projectile);
        }

        private void InitProjectile(Projectile projectile)
        {
            foreach (var item in _projectilesData.Items)
            {
                if (item.Type == _selectedProjectileType)
                {
                    projectile.Initialize(item.Type, item.Props, this);
                    break;
                }
            }
        }
    }
}