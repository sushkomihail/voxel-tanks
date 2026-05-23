using System;
using System.Collections.Generic;
using Projectiles;
using Settings;
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
        
        public IReadOnlyList<ProjectileType> ProjectileTypes => _projectileTypes.AsReadOnly();
        public event Action OnCurrentProjectileTypeChanged;
        
        protected readonly Dictionary<ProjectileType, ObjectPool<Projectile>> _projectilePools = new();
        
        private readonly List<ProjectileType> _projectileTypes = new();
        private readonly List<ProjectileType> _projectilesQueue = new();
        
        public void Initialize()
        {
            if (_projectilesData.Items.Count == 0) return;
            
            _projectilesData.ExtractProps();
            _projectilesQueue.Add(_projectilesData.Items[0].Type);
            
            foreach (ProjectileItem item in _projectilesData.Items)
            {
                _projectileTypes.Add(item.Type);
                _projectilePools[item.Type] =  
                    new ObjectPool<Projectile>(item.Prefab, InitializeProjectile, _projectilePoolDepth);
            }
        }

        public void AddNextProjectileTypeToQueue(ProjectileType type)
        {
            if (_projectilesQueue.Count > 1)
            {
                _projectilesQueue[1] = type;
            }
            else
            {
                _projectilesQueue.Add(type);
            }
        }

        public float GetProjectileSpeed()
        {
            if (_projectilesData.TryGetPropsByType(_projectilesQueue[0], out ProjectileProps props))
            {
                return props.Speed;
            }

            return 0f;
        }

        public int GetProjectilePenetration()
        {
            if (_projectilesData.TryGetPropsByType(_projectilesQueue[0], out ProjectileProps props))
            {
                return props.Penetration;
            }

            return -1;
        }

        public float GetProjectileNormalization()
        {
            return GlobalSettings.Normalizations.GetValueOrDefault(_projectilesQueue[0], 0);
        }

        public float GetProjectileRicochetAngle()
        {
            return GlobalSettings.RicochetAngles.GetValueOrDefault(_projectilesQueue[0], -1);
        }
        
        public void SetNextProjectileTypeAsCurrent()
        {
            if (_projectilesQueue.Count < 2) return;
            
            _projectilesQueue.RemoveAt(0);
        }

        public abstract void Shoot();

        public void OnProjectileHit(Projectile projectile)
        {
            _projectilePools[projectile.Type].Release(projectile);
        }

        protected ProjectileType LoadProjectile()
        {
            if (_projectilesQueue.Count > 1)
            {
                ProjectileType type = _projectilesQueue[0];
                SetNextProjectileTypeAsCurrent();
                OnCurrentProjectileTypeChanged?.Invoke();
                return type;
            }

            return _projectilesQueue[0];
        }

        private void InitializeProjectile(Projectile projectile)
        {
            if (_projectilesData.TryGetPropsByType(projectile.Type, out ProjectileProps props))
            {
                projectile.Initialize(props, this);
            }
        }
    }
}