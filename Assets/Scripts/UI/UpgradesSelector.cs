using UnityEngine;
using UpgradeSystem;

namespace UI
{
    public class UpgradesSelector : MonoBehaviour
    {
        [SerializeField] private UpgradesSelectorItem _leftItem;
        [SerializeField] private UpgradesSelectorItem _rightItem;
        
        private UpgradeManager _upgradeManager;
        private object _owner;

        public void Initialize(UpgradeManager upgradeManager, object owner)
        {
            _upgradeManager = upgradeManager;
            _owner = owner;

            _upgradeManager.OnUpgradeQueueStarted += Activate;
            _upgradeManager.OnUpgradePairChanged += UpdateVisuals;
            _upgradeManager.OnUpgradeQueueEnded += Deactivate;
            
            _leftItem.OnSelected += SelectLeftUpgrade;
            _rightItem.OnSelected += SelectRightUpgrade;
            
            Deactivate();
        }

        private void OnDestroy()
        {
            _upgradeManager.OnUpgradeQueueStarted -= Activate;
            _upgradeManager.OnUpgradePairChanged -= UpdateVisuals;
            _upgradeManager.OnUpgradeQueueEnded -= Deactivate;
            
            _leftItem.OnSelected -= SelectLeftUpgrade;
            _rightItem.OnSelected -= SelectRightUpgrade;
        }

        private void Activate()
        {
            gameObject.SetActive(true);
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void UpdateVisuals()
        {
            if (_upgradeManager.TryPeekUpgradePair(out UpgradePair pair))
            {
                _leftItem.Initialize(pair.LeftUpgrade.Description, pair.LeftUpgrade.ModifierValue, pair.LeftUpgrade.Icon);
                _rightItem.Initialize(pair.RightUpgrade.Description, pair.RightUpgrade.ModifierValue, pair.RightUpgrade.Icon);
            }
        }

        private void SelectLeftUpgrade()
        {
            _upgradeManager.ApplyUpgrade(_owner, UpgradeSide.Left);
        }

        private void SelectRightUpgrade()
        {
            _upgradeManager.ApplyUpgrade(_owner, UpgradeSide.Right);
        }
    }
}
