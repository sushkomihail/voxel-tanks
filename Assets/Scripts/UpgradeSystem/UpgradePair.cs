using System;
using UnityEngine;

namespace UpgradeSystem
{
    [Serializable]
    public class UpgradePair
    {
        [SerializeField] private Upgrade _leftUpgrade;
        [SerializeField] private Upgrade _rightUpgrade;
        
        public Upgrade LeftUpgrade => _leftUpgrade;
        public Upgrade RightUpgrade => _rightUpgrade;
    }
}