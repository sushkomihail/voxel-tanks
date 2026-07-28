using System.Collections.Generic;
using InputSystem;
using Tank.Modules;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EquipmentSystem
{
    public class RepairKit : EquipmentItem
    {
        private readonly TankModule[] _tankModules;
        private List<Key> _bindings;
        
        public override EquipmentItemType Type => EquipmentItemType.RepairKit;
        public IReadOnlyList<TankModule> TankModules => _tankModules;
        public IReadOnlyList<Key> Bindings => _bindings;

        public RepairKit(TankModule[] tankModules)
        {
            _tankModules = tankModules;
            InitializeBindings();
        }
        
        public override bool TryUse()
        {
            if (!base.TryUse()) return false;
            
            if (_bindings == null || _bindings.Count == 0) return false;
            
            var keyboard = Keyboard.current;
            
            if (keyboard == null) return false;

            for (int i = 0; i < _bindings.Count; i++)
            {
                if (keyboard[_bindings[i]].wasPressedThisFrame)
                {
                    if (_tankModules[i].IsNormal) return true;
                    
                    _tankModules[i].EnterNormalState();
                    Count--;
                    InvokeUsed();
                    return true;
                }
            }

            return true;
        }

        private void InitializeBindings()
        {
            if (_tankModules == null || _tankModules.Length == 0) return;
            
            int bindingsCount = Mathf.Min(_tankModules.Length, 9);
            InputBinder.TryBindDigitKeys(bindingsCount, Key.Digit1, out _bindings);
        }
    }
}