using System;
using System.Collections.Generic;
using Projectiles;
using Settings;
using ShootingSystems.Data;
using UnityEngine;
using UpgradeSystem;
using VContainer;

namespace ShootingSystems
{
    public abstract class ShootingSystem : MonoBehaviour
    {
        [SerializeField] protected Transform _projectilePivot;
        [SerializeField] private ProjectilesData _projectilesData;
        
        public IReadOnlyList<ProjectileType> ProjectileTypes => _projectileTypes.AsReadOnly();
        public event Action OnCurrentProjectileTypeChanged;
        
        protected UpgradeBroker _upgradeBroker;
        protected object _owner;
        
        private Dictionary<ProjectileType, ProjectileProps> _props;
        private readonly List<ProjectileType> _projectileTypes = new();
        private readonly List<ProjectileType> _projectilesQueue = new();
        private readonly Dictionary<ProjectileType, ProjectileFactory> _projectileFactories = new();

        [Inject]
        public void Construct(UpgradeBroker upgradeBroker)
        {
            _upgradeBroker = upgradeBroker;
        }
        
        public void Initialize(object owner)
        {
            _owner = owner;
            
            if (_projectilesData.Items.Count == 0) return;
            
            _props = _projectilesData.ExtractProps();
            _projectilesQueue.Add(_projectilesData.Items[0].Type);
            
            foreach (ProjectileItem item in _projectilesData.Items)
            {
                _projectileTypes.Add(item.Type);
                _projectileFactories.Add(item.Type, item.Type switch
                {
                    ProjectileType.AP => new APFactory(item.Props, _upgradeBroker, _owner),
                    ProjectileType.APCR => new APCRFactory(item.Props, _upgradeBroker, _owner),
                    ProjectileType.HE => new HEFactory(item.Props, _upgradeBroker, _owner),
                    ProjectileType.HEAT => new HEATFactory(item.Props, _upgradeBroker, _owner),
                    _ => throw new ArgumentOutOfRangeException()
                });
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

        public bool TryGetSelectedProjectileProps(out ProjectileProps props)
        {
            return _props.TryGetValue(_projectilesQueue[0], out props);
        }

        // TODO: Refactor GetProjectileSpeed()
        public float GetProjectileSpeed()
        {
            if (TryGetPropsByType(_projectilesQueue[0], out ProjectileProps props))
            {
                return props.Speed;
            }

            return 0f;
        }

        // TODO: Refactor GetProjectileMaxFlightDistance()
        public float GetProjectileMaxFlightDistance()
        {
            if (TryGetPropsByType(_projectilesQueue[0], out ProjectileProps props))
            {
                return props.MaxFlightDistance;
            }

            return 0f;
        }

        public float GetProjectileNormalization()
        {
            return GlobalSettings.Normalizations.GetValueOrDefault(_projectilesQueue[0], 0f);
        }

        /// <summary>
        /// Returns the angle of the projectile's ricochet or -1 if the projectile does not ricochet.
        /// </summary>
        /// <returns>Angle of the projectile's ricochet or -1.</returns>
        public float GetProjectileRicochetAngle()
        {
            return GlobalSettings.RicochetAngles.GetValueOrDefault(_projectilesQueue[0], -1f);
        }
        
        public virtual void SetNextProjectileTypeAsCurrent()
        {
            if (_projectilesQueue.Count < 2) return;
            
            _projectilesQueue.RemoveAt(0);
        }

        public abstract void Shoot();

        protected Projectile CreateProjectile()
        {
            ProjectileType type = _projectilesQueue[0];
            
            if (_projectilesQueue.Count > 1)
            {
                SetNextProjectileTypeAsCurrent();
                OnCurrentProjectileTypeChanged?.Invoke();
            }

            return _projectileFactories[type].CreateProjectile(_projectilePivot.position, _projectilePivot.forward);
        }
        
        private bool TryGetPropsByType(ProjectileType type, out ProjectileProps props)
        {
            return _props.TryGetValue(type, out props);
        }
    }
}