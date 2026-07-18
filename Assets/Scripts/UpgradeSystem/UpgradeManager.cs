using System;
using System.Collections.Generic;
using EquipmentSystem;
using Tank;
using UnityEngine;
using VContainer;

namespace UpgradeSystem
{
    public class UpgradeManager : MonoBehaviour
    {
        [SerializeField] private UpgradePair[] _upgradePairs;
        [SerializeField] private int _repairKitsNumberProvided = 1;
        [SerializeField] private float _healPercentAfterBaseCapture = 0.15f;
        [SerializeField] private float _damageUpgradePercentAfterKill = 0.2f;

        public event Action OnUpgradeQueueStarted;
        public event Action OnUpgradePairChanged;
        public event Action OnUpgradeQueueEnded;
        
        private readonly List<IDisposable> _disposables = new();
        private readonly Queue<UpgradePair> _upgradeQueue = new();
        private int _nextUpgradePairIndex;
        private UpgradeBroker _broker;

        [Inject]
        public void Construct(UpgradeBroker broker)
        {
            _broker = broker;
        }

        private void OnDestroy()
        {
            foreach (IDisposable disposable in _disposables)
            {
                disposable.Dispose();
            }
        }

        public void ProvideRepairKits(Equipment equipment)
        {
            equipment.AddItems(EquipmentItemType.RepairKit, _repairKitsNumberProvided);
        }

        public void Heal(TankHealth tankHealth)
        {
            tankHealth.Heal(_healPercentAfterBaseCapture);
        }

        public void UpgradeDamage(object owner)
        {
            _disposables.Add(new PercentageModifier(_broker, owner, StatType.Damage,
                _damageUpgradePercentAfterKill));
        }

        public void EnqueueUpgradePair()
        {
            _upgradeQueue.Enqueue(_upgradePairs[_nextUpgradePairIndex]);
            _nextUpgradePairIndex = (_nextUpgradePairIndex + 1) % _upgradePairs.Length;

            if (_upgradeQueue.Count == 1)
            {
                OnUpgradeQueueStarted?.Invoke();
                OnUpgradePairChanged?.Invoke();
            }
        }

        public bool TryPeekUpgradePair(out UpgradePair pair)
        {
            return _upgradeQueue.TryPeek(out pair);
        }
        
        public void ApplyUpgrade(object owner, UpgradeSide side)
        {
            if (_upgradeQueue.Count == 0) return;
            
            UpgradePair pair = _upgradeQueue.Dequeue();
            Upgrade upgrade = side switch
            {
                UpgradeSide.Right => pair.RightUpgrade,
                _ => pair.LeftUpgrade
            };
            
            _disposables.Add(upgrade.GetModifier(_broker, owner));
            
            if (_upgradeQueue.Count == 0)
            {
                OnUpgradeQueueEnded?.Invoke();
            }
            else
            {
                OnUpgradePairChanged?.Invoke();
            }
        }
    }
}